using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    public partial class H_Shifter : Form
    {
        static readonly string Root = Path.GetDirectoryName(Application.ExecutablePath);
        static readonly string LayoutFolder = Path.Combine(Root, "Layouts");
        static readonly string SettingsPath = Path.Combine(Root, "settings.json");
        static readonly string LegacyCounterPath = Path.Combine(Root, "shiftCount");

        const string YouTubeUrl = "https://www.youtube.com/c/BeanJ?sub_confirmation=1";

        readonly AppSettings settings;
        readonly ShifterEngine engine = new ShifterEngine();
        ShifterLayout layout;
        bool settingsDirty;
        bool loading = true;

        // header
        Label titleLabel, subtitleLabel;
        StatusPill statusPill;
        ModernButton activateButton;

        // left column
        SegmentedControl inputModeSwitch, stickSideSwitch, invertYSwitch;
        DarkComboBox deviceCombo, axesCombo, layoutCombo;
        Control xinputSection, dinputSection;
        Stepper gearStepper;

        // center / right
        ShifterCanvas canvas;
        GearIndicator indicator;
        GearListView gearList;
        Label totalShiftsLabel;

        readonly Timer uiTimer = new Timer { Interval = 16 };
        readonly Timer saveTimer = new Timer { Interval = 2000 };

        sealed class DeviceItem
        {
            public Guid Guid;
            public string Name;
            public override string ToString() => Name;
        }

        public H_Shifter()
        {
            Theme.InitDpi(this);
            settings = AppSettings.Load(SettingsPath, LegacyCounterPath);

            BuildUi();
            LoadInitialLayout();
            ApplySettingsToUi();
            ApplyInputSource();

            uiTimer.Tick += (s, e) => UpdateLiveState();
            saveTimer.Tick += (s, e) => SaveSettingsIfDirty();
            uiTimer.Start();
            saveTimer.Start();
            loading = false;
        }

        // ================================================================== UI construction

        void BuildUi()
        {
            SuspendLayout();

            AutoScaleMode = AutoScaleMode.None;
            Text = "AnalogStick H-Shifter";
            BackColor = Theme.Background;
            ForeColor = Theme.Text;
            Font = Theme.Body;
            DoubleBuffered = true;
            MinimumSize = new Size(Theme.S(1080), Theme.S(700));
            Size = new Size(Theme.S(1320), Theme.S(860));
            StartPosition = FormStartPosition.CenterScreen;
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch (ArgumentException) { }

            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 2,
                Padding = Theme.S(16, 12, 16, 16),
                BackColor = Theme.Background,
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, Theme.S(280)));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, Theme.S(330)));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, Theme.S(64)));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var header = BuildHeader();
            grid.Controls.Add(header, 0, 0);
            grid.SetColumnSpan(header, 3);

            var left = BuildLeftColumn();
            left.Margin = new Padding(0, 0, Theme.S(12), 0);
            grid.Controls.Add(left, 0, 1);

            canvas = new ShifterCanvas { Dock = DockStyle.Fill, Margin = new Padding(0) };
            canvas.LayoutEdited += (s, e) => MarkDirty();
            canvas.SelectedChanged += (s, gear) => gearList.Selected = gear;
            grid.Controls.Add(canvas, 1, 1);

            var right = BuildRightColumn();
            right.Margin = new Padding(Theme.S(12), 0, 0, 0);
            grid.Controls.Add(right, 2, 1);

            Controls.Add(grid);
            ResumeLayout(true);
        }

        Control BuildHeader()
        {
            var header = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Background, Margin = new Padding(0, 0, 0, Theme.S(8)) };

            titleLabel = new Label
            {
                Text = "H-Shifter",
                Font = new Font("Segoe UI Semibold", 17f),
                ForeColor = Theme.Text,
                AutoSize = true,
                Location = new Point(0, Theme.S(2)),
            };
            subtitleLabel = new Label
            {
                Text = "Turn an analog stick into an H-pattern gearbox",
                Font = Theme.Small,
                ForeColor = Theme.TextMuted,
                AutoSize = true,
                Location = new Point(Theme.S(2), Theme.S(36)),
            };

            statusPill = new StatusPill();
            statusPill.SetStatus("Searching for controller…", Theme.TextDim);

            activateButton = new ModernButton
            {
                Variant = ButtonVariant.Primary,
                Text = "Activate",
                Icon = Theme.Icons.Play,
                Size = new Size(Theme.S(168), Theme.S(42)),
                Font = Theme.BodyBold,
            };
            activateButton.Click += (s, e) => SetOutputEnabled(!engine.OutputEnabled);

            header.Controls.AddRange(new Control[] { titleLabel, subtitleLabel, statusPill, activateButton });
            header.Resize += (s, e) => LayoutHeader(header);
            return header;
        }

        void LayoutHeader(Control header)
        {
            int y = (header.Height - activateButton.Height) / 2;
            activateButton.Location = new Point(header.Width - activateButton.Width, y);
            statusPill.Location = new Point(activateButton.Left - Theme.S(14) - statusPill.Width, (header.Height - statusPill.Height) / 2);
        }

        Control BuildLeftColumn()
        {
            var column = new ScrollStack { Dock = DockStyle.Fill };

            // ---- Controller
            inputModeSwitch = new SegmentedControl { Items = new[] { "XInput", "DirectInput" } };
            inputModeSwitch.SelectedIndexChanged += (s, e) =>
            {
                settings.InputMode = (InputMode)inputModeSwitch.SelectedIndex;
                UpdateInputSections();
                ApplyInputSource();
            };

            stickSideSwitch = new SegmentedControl { Items = new[] { "Left stick", "Right stick" }, Height = Theme.S(32) };
            stickSideSwitch.SelectedIndexChanged += (s, e) =>
            {
                settings.XInputStick = stickSideSwitch.SelectedIndex == 0 ? StickSide.Left : StickSide.Right;
                ApplyInputSource();
            };

            var xinputCard = new StackCard { ShowFrame = false, Padding = new Padding(0), Spacing = Theme.S(6) };
            xinputCard.AddRange(new ThemedLabel("Stick (Xbox-compatible controller)"), stickSideSwitch);
            xinputSection = xinputCard;

            deviceCombo = new DarkComboBox { Tag = HRow.Fill, Placeholder = "No device found" };
            deviceCombo.SelectedIndexChanged += (s, e) =>
            {
                if (deviceCombo.SelectedItem is DeviceItem item) settings.DirectInputDevice = item.Guid.ToString();
                ApplyInputSource();
            };
            var refreshButton = new ModernButton { Icon = Theme.Icons.Refresh, Width = Theme.S(34) };
            refreshButton.Click += (s, e) => RefreshDevices();
            new ToolTip(components).SetToolTip(refreshButton, "Search for devices again");

            axesCombo = new DarkComboBox { Tag = HRow.Fill };
            axesCombo.Items.AddRange(new object[] { "X / Y", "Rx / Ry", "Z / Rz" });
            axesCombo.SelectedIndexChanged += (s, e) =>
            {
                settings.DirectInputAxes = (AxisPair)Math.Max(0, axesCombo.SelectedIndex);
                ApplyInputSource();
            };

            invertYSwitch = new SegmentedControl { Items = new[] { "Normal", "Inverted" }, Height = Theme.S(32), Width = Theme.S(140) };
            invertYSwitch.SelectedIndexChanged += (s, e) =>
            {
                settings.InvertY = invertYSwitch.SelectedIndex == 1;
                ApplyInputSource();
            };

            var dinputCard = new StackCard { ShowFrame = false, Padding = new Padding(0), Spacing = Theme.S(6) };
            dinputCard.AddRange(
                new ThemedLabel("Device"),
                new HRow(Theme.S(34), deviceCombo, refreshButton),
                new ThemedLabel("Axes"),
                new HRow(Theme.S(34), axesCombo),
                new ThemedLabel("Y axis"),
                invertYSwitch);
            dinputSection = dinputCard;

            var controllerCard = new StackCard { Caption = "Controller" };
            controllerCard.AddRange(inputModeSwitch, xinputSection, dinputSection);

            // ---- Shift pattern
            gearStepper = new Stepper
            {
                Minimum = ShifterLayout.MinForwardGears,
                Maximum = ShifterLayout.MaxForwardGears,
                Suffix = " + R",
                Height = Theme.S(44),
            };
            gearStepper.ValueChanged += (s, e) => SetLayout(layout.WithForwardGears(gearStepper.Value));

            var arrangeButton = new ModernButton { Text = "Auto-arrange", Icon = Theme.Icons.Grid, Tag = HRow.Fill };
            arrangeButton.Click += (s, e) => SetLayout(layout.WithForwardGears(layout.ForwardCount));
            new ToolTip(components).SetToolTip(arrangeButton, "Re-arrange all zones into a standard H-pattern (key bindings are kept)");

            var clearTrailButton = new ModernButton { Text = "Clear trail", Icon = Theme.Icons.Erase, Tag = HRow.Fill };
            clearTrailButton.Click += (s, e) => canvas.ClearTrail();

            var patternCard = new StackCard { Caption = "Shift pattern" };
            patternCard.AddRange(
                new ThemedLabel("Forward gears"),
                gearStepper,
                new HRow(Theme.S(34), arrangeButton, clearTrailButton));

            // ---- Saved layouts
            layoutCombo = new DarkComboBox { Tag = HRow.Fill, Placeholder = "No saved layouts" };
            layoutCombo.SelectionChangeCommitted += (s, e) => LoadSelectedLayout();

            var saveButton = new ModernButton { Text = "Save", Icon = Theme.Icons.Save, Tag = HRow.Fill, Variant = ButtonVariant.Primary };
            saveButton.Click += (s, e) => SaveCurrentLayout(askForName: false);
            new ToolTip(components).SetToolTip(saveButton, "Save changes to the selected layout");
            var saveAsButton = new ModernButton { Text = "Save as…", Tag = HRow.Fill };
            saveAsButton.Click += (s, e) => SaveCurrentLayout(askForName: true);
            var deleteButton = new ModernButton { Icon = Theme.Icons.Delete, Width = Theme.S(38), Variant = ButtonVariant.Danger };
            deleteButton.Click += (s, e) => DeleteSelectedLayout();
            new ToolTip(components).SetToolTip(deleteButton, "Delete the selected layout");

            var layoutsCard = new StackCard { Caption = "Saved layouts" };
            layoutsCard.AddRange(
                new HRow(Theme.S(34), layoutCombo),
                new HRow(Theme.S(34), saveButton, saveAsButton, deleteButton));

            // ---- footer
            var aboutButton = new ModernButton { Text = "About", Icon = Theme.Icons.Info, Variant = ButtonVariant.Ghost, Tag = HRow.Fill };
            aboutButton.Click += (s, e) => { using (var about = new AboutForm()) about.ShowDialog(this); };
            var youtubeButton = new ModernButton { Text = "YouTube", Icon = Theme.Icons.Play, Variant = ButtonVariant.Ghost, Tag = HRow.Fill };
            youtubeButton.Click += (s, e) => OpenUrl(YouTubeUrl);

            var signature = new PictureBox
            {
                Image = Theme.Fit(Theme.Tint(Properties.Resources.LogoNeu, Theme.TextMuted), new Size(Theme.S(150), Theme.S(44))),
                SizeMode = PictureBoxSizeMode.CenterImage,
                Height = Theme.S(52),
                BackColor = Theme.Surface,
                Cursor = Cursors.Hand,
            };
            signature.Click += (s, e) => OpenUrl(YouTubeUrl);
            new ToolTip(components).SetToolTip(signature, "Made by Bean");

            var footerCard = new StackCard { Spacing = Theme.S(4) };
            footerCard.AddRange(signature, new HRow(Theme.S(34), aboutButton, youtubeButton));

            column.AddRange(controllerCard, patternCard, layoutsCard, footerCard);
            return column;
        }

        Control BuildRightColumn()
        {
            var column = new ScrollStack { Dock = DockStyle.Fill };

            indicator = new GearIndicator();
            var indicatorCard = new StackCard();
            indicatorCard.AddRange(indicator);

            totalShiftsLabel = new Label
            {
                Font = Theme.Stat,
                ForeColor = Theme.Text,
                AutoSize = false,
                Height = Theme.S(40),
                Tag = HRow.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Theme.Surface,
            };
            var resetButton = new ModernButton { Text = "Reset", Icon = Theme.Icons.Reset, Variant = ButtonVariant.Ghost, Width = Theme.S(92) };
            resetButton.Click += (s, e) => ResetStatistics();

            var statsCard = new StackCard { Caption = "Total shifts" };
            statsCard.AddRange(new HRow(Theme.S(40), totalShiftsLabel, resetButton));

            gearList = new GearListView();
            gearList.SelectedGearChanged += (s, gear) => canvas.Selected = gear;
            gearList.KeyAssigned += (s, gear) => { canvas.Invalidate(); MarkDirty(); };

            var gearsCard = new StackCard { Caption = "Gears & keys" };
            gearsCard.AddRange(new ThemedLabel("Click a key to rebind it · Esc cancels"), gearList);

            column.AddRange(indicatorCard, statsCard, gearsCard);
            return column;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Theme.UseDarkTitleBar(this);
        }

        // ================================================================== settings <-> UI

        void ApplySettingsToUi()
        {
            if (settings.WindowWidth > 0 && settings.WindowHeight > 0)
            {
                var area = Screen.FromControl(this).WorkingArea;
                Size = new Size(Math.Min(settings.WindowWidth, area.Width), Math.Min(settings.WindowHeight, area.Height));
            }
            if (settings.WindowMaximized) WindowState = FormWindowState.Maximized;

            inputModeSwitch.SelectedIndex = (int)settings.InputMode;
            stickSideSwitch.SelectedIndex = settings.XInputStick == StickSide.Left ? 0 : 1;
            axesCombo.SelectedIndex = (int)settings.DirectInputAxes;
            invertYSwitch.SelectedIndex = settings.InvertY ? 1 : 0;
            RefreshDevices();
            UpdateInputSections();
            UpdateStatistics();
        }

        void UpdateInputSections()
        {
            bool xinput = settings.InputMode == InputMode.XInput;
            xinputSection.Visible = xinput;
            dinputSection.Visible = !xinput;
            xinputSection.Parent?.PerformLayout();
        }

        void RefreshDevices()
        {
            var devices = DirectInputStickSource.GetDevices();
            deviceCombo.BeginUpdate();
            deviceCombo.Items.Clear();
            foreach (var d in devices)
                deviceCombo.Items.Add(new DeviceItem { Guid = d.InstanceGuid, Name = d.InstanceName.Trim() });
            deviceCombo.EndUpdate();

            var wanted = deviceCombo.Items.Cast<DeviceItem>().FirstOrDefault(d => d.Guid.ToString() == settings.DirectInputDevice);
            if (wanted != null) deviceCombo.SelectedItem = wanted;
            else if (deviceCombo.Items.Count > 0) deviceCombo.SelectedIndex = 0;
            else ApplyInputSource();
        }

        void ApplyInputSource()
        {
            IStickSource source;
            if (settings.InputMode == InputMode.XInput)
            {
                source = new XInputStickSource(settings.XInputStick);
            }
            else
            {
                var device = deviceCombo.SelectedItem as DeviceItem;
                source = new DirectInputStickSource(device?.Guid ?? Guid.Empty, settings.DirectInputAxes, settings.InvertY);
            }
            engine.SetSource(source);
            MarkDirty();
        }

        // ================================================================== layouts

        void LoadInitialLayout()
        {
            ShifterLayout initial = settings.Layout;
            var saved = ShifterLayout.ListSaved(LayoutFolder);

            if (initial == null || initial.Gears == null || initial.Gears.Count == 0)
            {
                initial = null;
                string name = settings.LayoutName != null && saved.ContainsKey(settings.LayoutName) ? settings.LayoutName : saved.Keys.FirstOrDefault();
                if (name != null)
                {
                    try { initial = ShifterLayout.Load(saved[name]); settings.LayoutName = name; }
                    catch (Exception ex) { Console.WriteLine("Could not load layout: " + ex.Message); }
                }
            }
            else
            {
                initial.Normalize();
            }

            SetLayout(initial ?? ShifterLayout.CreateDefault(6));
            RefreshLayoutList(settings.LayoutName);
        }

        void SetLayout(ShifterLayout newLayout)
        {
            layout = newLayout;
            canvas.ShiftLayout = layout;
            gearList.SetGears(layout.Gears, g => settings.GetCount(g.Name));
            gearList.Selected = canvas.Selected;
            gearStepper.SetSilently(layout.ForwardCount);
            engine.SetZones(layout.Gears.ToArray());
            gearList.Parent?.PerformLayout();
            MarkDirty();
        }

        void RefreshLayoutList(string select)
        {
            var saved = ShifterLayout.ListSaved(LayoutFolder);
            layoutCombo.BeginUpdate();
            layoutCombo.Items.Clear();
            layoutCombo.Items.AddRange(saved.Keys.Cast<object>().ToArray());
            layoutCombo.EndUpdate();
            layoutCombo.SelectedItem = select != null && saved.ContainsKey(select) ? select : null;
        }

        void LoadSelectedLayout()
        {
            if (!(layoutCombo.SelectedItem is string name)) return;
            if (!ShifterLayout.ListSaved(LayoutFolder).TryGetValue(name, out string path)) return;

            try
            {
                SetLayout(ShifterLayout.Load(path));
                settings.LayoutName = name;
            }
            catch (Exception ex)
            {
                ShowError("The layout could not be loaded.\n\n" + ex.Message);
            }
        }

        void SaveCurrentLayout(bool askForName)
        {
            string name = layoutCombo.SelectedItem as string;
            if (askForName || name == null)
            {
                name = InputDialog.Show(this, "Save layout", "Name of the layout", name ?? "My layout", ValidateLayoutName);
                if (name == null) return;

                string target = Path.Combine(LayoutFolder, name + ShifterLayout.FileExtension);
                if (File.Exists(target) &&
                    MessageBox.Show(this, "A layout called \"" + name + "\" already exists. Overwrite it?", "Save layout",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            try
            {
                layout.Save(Path.Combine(LayoutFolder, name + ShifterLayout.FileExtension));
                settings.LayoutName = name;
                RefreshLayoutList(name);
                MarkDirty();
            }
            catch (Exception ex)
            {
                ShowError("The layout could not be saved.\n\n" + ex.Message);
            }
        }

        static string ValidateLayoutName(string name)
        {
            if (name.Length == 0) return "Please enter a name.";
            if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                return "These characters are not allowed: " + string.Join(" ", Path.GetInvalidFileNameChars().Where(c => c > 31));
            return null;
        }

        void DeleteSelectedLayout()
        {
            if (!(layoutCombo.SelectedItem is string name)) return;
            if (!ShifterLayout.ListSaved(LayoutFolder).TryGetValue(name, out string path)) return;

            if (MessageBox.Show(this, "Delete the layout \"" + name + "\"?\nThis cannot be undone.", "Delete layout",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                File.Delete(path);
                if (settings.LayoutName == name) settings.LayoutName = null;
                RefreshLayoutList(null);
                MarkDirty();
            }
            catch (Exception ex)
            {
                ShowError("The layout could not be deleted.\n\n" + ex.Message);
            }
        }

        // ================================================================== live state

        void SetOutputEnabled(bool enabled)
        {
            engine.OutputEnabled = enabled;
            activateButton.Text = enabled ? "Active" : "Activate";
            activateButton.Icon = enabled ? Theme.Icons.Stop : Theme.Icons.Play;
            activateButton.AccentColor = enabled ? Theme.Success : (Color?)null;
        }

        void UpdateLiveState()
        {
            engine.GetState(out var position, out bool connected, out var engaged);

            canvas.SetStick(position, connected, engaged);
            indicator.SetState(engaged, engine.OutputEnabled, connected);
            gearList.Engaged = engaged;

            bool resized = connected
                ? statusPill.SetStatus(settings.InputMode == InputMode.XInput ? "Controller connected" : "Device connected", Theme.Success)
                : statusPill.SetStatus("No controller", Theme.Danger);
            if (resized) LayoutHeader(activateButton.Parent);

            bool counted = false;
            while (engine.Shifts.TryDequeue(out var gear))
            {
                settings.Increment(gear.Name);
                counted = true;
            }
            if (counted)
            {
                UpdateStatistics();
                MarkDirty();
            }
        }

        void UpdateStatistics()
        {
            totalShiftsLabel.Text = settings.TotalShifts.ToString("N0");
            gearList.Invalidate();
        }

        void ResetStatistics()
        {
            if (MessageBox.Show(this, "Reset all shift counters to zero?", "Reset statistics",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            settings.ShiftCounts.Clear();
            UpdateStatistics();
            MarkDirty();
        }

        // ================================================================== persistence

        void MarkDirty()
        {
            if (!loading) settingsDirty = true;
        }

        void SaveSettingsIfDirty()
        {
            if (!settingsDirty) return;
            settingsDirty = false;

            settings.Layout = layout;
            if (WindowState == FormWindowState.Normal)
            {
                settings.WindowWidth = Width;
                settings.WindowHeight = Height;
            }
            settings.WindowMaximized = WindowState == FormWindowState.Maximized;

            try { settings.Save(SettingsPath); }
            catch (Exception ex) { Console.WriteLine("Could not save settings: " + ex.Message); }
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            MarkDirty();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            MarkDirty();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            uiTimer.Stop();
            saveTimer.Stop();
            settingsDirty = true;
            SaveSettingsIfDirty();
            engine.Dispose();
            base.OnFormClosing(e);
        }

        // ================================================================== helpers

        static void OpenUrl(string url)
        {
            try { Process.Start(url); } catch (Exception ex) { Console.WriteLine("Could not open link: " + ex.Message); }
        }

        void ShowError(string message) => MessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
