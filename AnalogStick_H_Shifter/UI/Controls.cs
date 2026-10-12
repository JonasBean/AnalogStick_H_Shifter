using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    /// <summary>Base class for the owner-drawn controls: double buffered, hover/press tracking.</summary>
    abstract class PaintedControl : Control
    {
        protected bool Hovered { get; private set; }
        protected bool Pressed { get; private set; }

        protected PaintedControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Font = Theme.Body;
            ForeColor = Theme.Text;
            BackColor = Theme.Surface;
        }

        protected override void OnMouseEnter(EventArgs e) { Hovered = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { Hovered = false; Pressed = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { if (e.Button == MouseButtons.Left) { Pressed = true; Invalidate(); } base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { Pressed = false; Invalidate(); base.OnMouseUp(e); }
        protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }
        protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }
        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

        protected void PaintFocus(Graphics g, RectangleF r, float radius)
        {
            if (Focused && ShowFocusCues)
                Theme.DrawRounded(g, Theme.WithAlpha(Theme.Accent, 200), Theme.S(1.5f), RectangleF.Inflate(r, -1, -1), radius);
        }
    }

    enum ButtonVariant { Secondary, Primary, Ghost, Danger }

    /// <summary>Flat rounded button with optional Segoe MDL2 icon.</summary>
    sealed class ModernButton : PaintedControl, IButtonControl
    {
        ButtonVariant variant;
        string icon;
        Color? accent;

        public ModernButton()
        {
            Cursor = Cursors.Hand;
            Size = new Size(Theme.S(120), Theme.S(34));
            TabStop = true;
        }

        public ButtonVariant Variant { get => variant; set { variant = value; Invalidate(); } }
        public string Icon { get => icon; set { icon = value; Invalidate(); } }

        /// <summary>Overrides the fill color of the Primary variant.</summary>
        public Color? AccentColor { get => accent; set { accent = value; Invalidate(); } }

        public DialogResult DialogResult { get; set; }
        public void NotifyDefault(bool value) { }
        public void PerformClick() { if (Enabled) OnClick(EventArgs.Empty); }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter) { PerformClick(); e.Handled = true; }
            base.OnKeyDown(e);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (DialogResult != DialogResult.None && FindForm() is Form form) form.DialogResult = DialogResult;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);
            Theme.SetupQuality(g);

            var r = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            float radius = Theme.S(7f);

            Color fill, text, border = Color.Empty;
            switch (variant)
            {
                case ButtonVariant.Primary:
                    fill = accent ?? Theme.Accent;
                    text = Theme.OnColor(fill);
                    if (Hovered) fill = Theme.Blend(fill, Color.White, 0.12f);
                    if (Pressed) fill = Theme.Blend(fill, Color.Black, 0.15f);
                    break;
                case ButtonVariant.Danger:
                    fill = Hovered ? Theme.WithAlpha(Theme.Danger, Pressed ? 90 : 60) : Theme.SurfaceRaised;
                    text = Hovered ? Color.White : Theme.Danger;
                    border = Theme.WithAlpha(Theme.Danger, 90);
                    break;
                case ButtonVariant.Ghost:
                    fill = Pressed ? Theme.SurfaceHover : Hovered ? Theme.SurfaceRaised : BackColor;
                    text = Hovered ? Theme.Text : Theme.TextMuted;
                    break;
                default:
                    fill = Pressed ? Theme.Border : Hovered ? Theme.SurfaceHover : Theme.SurfaceRaised;
                    text = Theme.Text;
                    border = Theme.Border;
                    break;
            }

            if (!Enabled) { fill = Theme.SurfaceRaised; text = Theme.TextDim; border = Color.Empty; }

            Theme.FillRounded(g, fill, r, radius);
            if (border != Color.Empty) Theme.DrawRounded(g, border, 1f, r, radius);
            PaintFocus(g, r, radius);

            // Icon + text, centered as a group.
            bool hasText = !string.IsNullOrEmpty(Text);
            bool hasIcon = !string.IsNullOrEmpty(icon);
            using (var iconFont = Theme.IconFont(Font.Size * 0.95f))
            {
                const TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter;
                Size textSize = hasText ? TextRenderer.MeasureText(g, Text, Font, Size.Empty, flags) : Size.Empty;
                Size iconSize = hasIcon ? TextRenderer.MeasureText(g, icon, iconFont, Size.Empty, flags) : Size.Empty;
                int spacing = hasText && hasIcon ? Theme.S(7) : 0;
                int x = Math.Max(Theme.S(6), (Width - (textSize.Width + iconSize.Width + spacing)) / 2);

                if (hasIcon)
                {
                    TextRenderer.DrawText(g, icon, iconFont, new Rectangle(x, 0, iconSize.Width, Height), text, flags);
                    x += iconSize.Width + spacing;
                }
                if (hasText)
                    TextRenderer.DrawText(g, Text, Font, new Rectangle(x, 0, Math.Max(0, Width - x - Theme.S(4)), Height), text,
                        flags | TextFormatFlags.EndEllipsis);
            }
        }
    }

    /// <summary>Pill-shaped switch between a few options (like a modern tab/toggle group).</summary>
    sealed class SegmentedControl : PaintedControl
    {
        string[] items = new string[0];
        int selectedIndex;
        int hoverIndex = -1;

        public event EventHandler SelectedIndexChanged;

        public SegmentedControl()
        {
            Cursor = Cursors.Hand;
            Height = Theme.S(34);
            TabStop = true;
        }

        public string[] Items { get => items; set { items = value ?? new string[0]; Invalidate(); } }

        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if (value == selectedIndex || value < 0 || value >= items.Length) return;
                selectedIndex = value;
                Invalidate();
                SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        RectangleF Segment(int i)
        {
            float pad = Theme.S(3f);
            float w = (Width - 2 * pad) / Math.Max(1, items.Length);
            return new RectangleF(pad + i * w, pad, w, Height - 2 * pad);
        }

        int IndexAt(Point p)
        {
            for (int i = 0; i < items.Length; i++) if (Segment(i).Contains(p)) return i;
            return -1;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int i = IndexAt(e.Location);
            if (i != hoverIndex) { hoverIndex = i; Invalidate(); }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e) { hoverIndex = -1; base.OnMouseLeave(e); }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            int i = IndexAt(e.Location);
            if (i >= 0 && Enabled) SelectedIndex = i;
            base.OnMouseClick(e);
        }

        protected override bool IsInputKey(Keys keyData) => keyData == Keys.Left || keyData == Keys.Right || base.IsInputKey(keyData);

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) SelectedIndex = Math.Max(0, selectedIndex - 1);
            if (e.KeyCode == Keys.Right) SelectedIndex = Math.Min(items.Length - 1, selectedIndex + 1);
            base.OnKeyDown(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);
            Theme.SetupQuality(g);

            var track = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            float radius = Theme.S(8f);
            Theme.FillRounded(g, Theme.Background, track, radius);
            Theme.DrawRounded(g, Theme.Border, 1f, track, radius);
            PaintFocus(g, track, radius);

            for (int i = 0; i < items.Length; i++)
            {
                var seg = Segment(i);
                bool selected = i == selectedIndex;
                if (selected)
                    Theme.FillRounded(g, Enabled ? Theme.SurfaceHover : Theme.SurfaceRaised, seg, radius - Theme.S(2f));
                else if (i == hoverIndex)
                    Theme.FillRounded(g, Theme.SurfaceRaised, seg, radius - Theme.S(2f));

                Color color = !Enabled ? Theme.TextDim : selected ? Theme.Text : Theme.TextMuted;
                Theme.DrawCentered(g, items[i], selected ? Theme.BodyBold : Font, color, seg);
            }
        }
    }

    /// <summary>Numeric stepper: [ − ]  value  [ + ].</summary>
    sealed class Stepper : PaintedControl
    {
        int value = 6, minimum = 1, maximum = 12;
        int hoverPart; // -1 minus, 1 plus

        public event EventHandler ValueChanged;

        public Stepper()
        {
            Height = Theme.S(40);
            TabStop = true;
        }

        public int Minimum { get => minimum; set { minimum = value; Invalidate(); } }
        public int Maximum { get => maximum; set { maximum = value; Invalidate(); } }
        public string Suffix { get; set; } = "";

        public int Value
        {
            get => value;
            set
            {
                int v = Math.Max(minimum, Math.Min(maximum, value));
                if (v == this.value) return;
                this.value = v;
                Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>Sets the value without raising <see cref="ValueChanged"/>.</summary>
        public void SetSilently(int v) { value = Math.Max(minimum, Math.Min(maximum, v)); Invalidate(); }

        RectangleF MinusRect => new RectangleF(Theme.S(4f), Theme.S(4f), Height - Theme.S(8f), Height - Theme.S(8f));
        RectangleF PlusRect => new RectangleF(Width - Height + Theme.S(4f), Theme.S(4f), Height - Theme.S(8f), Height - Theme.S(8f));

        int PartAt(Point p) => MinusRect.Contains(p) ? -1 : PlusRect.Contains(p) ? 1 : 0;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int part = PartAt(e.Location);
            Cursor = part != 0 ? Cursors.Hand : Cursors.Default;
            if (part != hoverPart) { hoverPart = part; Invalidate(); }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e) { hoverPart = 0; base.OnMouseLeave(e); }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left || !Enabled) return;
            Focus();
            Value += PartAt(e.Location);
        }

        protected override bool IsInputKey(Keys keyData) =>
            keyData == Keys.Left || keyData == Keys.Right || keyData == Keys.Up || keyData == Keys.Down || base.IsInputKey(keyData);

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus) Value++;
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Down || e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus) Value--;
            base.OnKeyDown(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);
            Theme.SetupQuality(g);

            var track = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            float radius = Theme.S(9f);
            Theme.FillRounded(g, Theme.Background, track, radius);
            Theme.DrawRounded(g, Theme.Border, 1f, track, radius);
            PaintFocus(g, track, radius);

            DrawPart(g, MinusRect, Theme.Icons.Remove, value > minimum, hoverPart == -1);
            DrawPart(g, PlusRect, Theme.Icons.Add, value < maximum, hoverPart == 1);

            var center = new RectangleF(MinusRect.Right, 0, PlusRect.Left - MinusRect.Right, Height);
            using (var big = new Font("Segoe UI Semibold", 14f))
            {
                string text = value.ToString();
                var size = g.MeasureString(text, big);
                var small = g.MeasureString(Suffix, Theme.Small);
                float total = size.Width + (Suffix.Length > 0 ? small.Width : 0);
                float x = center.X + (center.Width - total) / 2f;
                Theme.DrawCentered(g, text, big, Theme.Text, new RectangleF(x, 0, size.Width, Height));
                if (Suffix.Length > 0)
                    Theme.DrawCentered(g, Suffix, Theme.Small, Theme.TextMuted, new RectangleF(x + size.Width - Theme.S(4f), Theme.S(2f), small.Width, Height));
            }
        }

        void DrawPart(Graphics g, RectangleF r, string glyph, bool enabled, bool hover)
        {
            enabled &= Enabled;
            Theme.FillRounded(g, enabled && hover ? Theme.Accent : Theme.SurfaceRaised, r, Theme.S(7f));
            using (var font = Theme.IconFont(10f))
                Theme.DrawCentered(g, glyph, font, enabled ? (hover ? Color.White : Theme.Text) : Theme.TextDim, r);
        }
    }

    /// <summary>Rounded surface that hosts child controls, with an optional uppercase caption.</summary>
    class CardPanel : Panel
    {
        string caption;

        public CardPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = Theme.Background;
            ForeColor = Theme.Text;
            Font = Theme.Body;
            Padding = Theme.S(16, 14, 16, 14);
        }

        public string Caption
        {
            get => caption;
            set { caption = value; Padding = Theme.S(16, string.IsNullOrEmpty(value) ? 14 : 40, 16, 14); Invalidate(); }
        }

        public Color SurfaceColor { get; set; } = Theme.Surface;

        /// <summary>False for nested sections: no rounded surface/border, just a plain container.</summary>
        public bool ShowFrame { get; set; } = true;

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control.BackColor == Theme.Background || e.Control is Label) e.Control.BackColor = SurfaceColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);
            Theme.SetupQuality(g);

            if (!ShowFrame) { g.Clear(SurfaceColor); return; }

            var r = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            Theme.FillRounded(g, SurfaceColor, r, Theme.S(12f));
            Theme.DrawRounded(g, Theme.Border, 1f, r, Theme.S(12f));

            if (!string.IsNullOrEmpty(caption))
            {
                using (var brush = new SolidBrush(Theme.TextMuted))
                    g.DrawString(caption.ToUpperInvariant(), Theme.Caption, brush, Theme.S(16f), Theme.S(15f));
            }
        }
    }

    /// <summary>Dark, owner-drawn combo box.</summary>
    sealed class DarkComboBox : ComboBox
    {
        public DarkComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            FlatStyle = FlatStyle.Flat;
            BackColor = Theme.SurfaceRaised;
            ForeColor = Theme.Text;
            Font = Theme.Body;
            ItemHeight = Theme.S(24);
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        [DefaultValue("")]
        public string Placeholder { get; set; } = "";

        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); Invalidate(); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); Invalidate(); }

        /// <summary>The flat ComboBox still paints a light border and button – paint over them.</summary>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg != 0x000F /* WM_PAINT */) return;

            using (var g = Graphics.FromHwnd(Handle))
            {
                bool hot = ClientRectangle.Contains(PointToClient(Cursor.Position)) || Focused;
                int button = SystemInformation.VerticalScrollBarWidth + Theme.S(4);
                var arrowRect = new Rectangle(Width - button, 1, button - 1, Height - 2);

                using (var fill = new SolidBrush(BackColor))
                    g.FillRectangle(fill, arrowRect);
                using (var pen = new Pen(hot ? Theme.TextDim : Theme.Border))
                    g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
                using (var iconFont = Theme.IconFont(8f))
                    TextRenderer.DrawText(g, "\uE70D", iconFont, arrowRect, Enabled ? Theme.TextMuted : Theme.TextDim,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            var g = e.Graphics;
            bool selected = (e.State & DrawItemState.Selected) != 0 && (e.State & DrawItemState.ComboBoxEdit) == 0;
            using (var bg = new SolidBrush(selected ? Theme.Accent : Theme.SurfaceRaised))
                g.FillRectangle(bg, e.Bounds);

            string text = e.Index >= 0 ? GetItemText(Items[e.Index]) : Placeholder;
            Color color = e.Index >= 0 ? (selected ? Color.White : Theme.Text) : Theme.TextDim;
            TextRenderer.DrawText(g, text, Font, Rectangle.Inflate(e.Bounds, -Theme.S(4), 0), color,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }
    }

    /// <summary>Small rounded status badge with a colored dot ("● Controller connected").</summary>
    sealed class StatusPill : PaintedControl
    {
        Color dot = Theme.TextDim;

        public StatusPill()
        {
            Font = Theme.Small;
            Height = Theme.S(28);
            BackColor = Theme.Background;
        }

        public Color DotColor { get => dot; set { if (dot != value) { dot = value; Invalidate(); } } }

        /// <summary>Updates text and dot color; returns true when the width changed.</summary>
        public bool SetStatus(string text, Color color)
        {
            DotColor = color;
            if (Text == text) return false;

            Text = text;
            int width = TextRenderer.MeasureText(text, Font).Width + Theme.S(34);
            if (width == Width) return false;
            Width = width;
            return true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);
            Theme.SetupQuality(g);

            var r = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            Theme.FillRounded(g, Theme.Surface, r, Height / 2f);
            Theme.DrawRounded(g, Theme.Border, 1f, r, Height / 2f);

            float d = Theme.S(8f);
            var dotRect = new RectangleF(Theme.S(12f), (Height - d) / 2f, d, d);
            using (var glow = new SolidBrush(Theme.WithAlpha(dot, 60)))
                g.FillEllipse(glow, RectangleF.Inflate(dotRect, Theme.S(3f), Theme.S(3f)));
            using (var brush = new SolidBrush(dot))
                g.FillEllipse(brush, dotRect);

            TextRenderer.DrawText(g, Text, Font, new Rectangle(Theme.S(26), 0, Width - Theme.S(30), Height), Theme.Text,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPrefix);
        }
    }

    /// <summary>Plain label preset for the dark theme.</summary>
    sealed class ThemedLabel : Label
    {
        public ThemedLabel(string text, Font font = null, Color? color = null)
        {
            Text = text;
            Font = font ?? Theme.Small;
            ForeColor = color ?? Theme.TextMuted;
            AutoSize = false;
            Height = Theme.S(20);
            TextAlign = ContentAlignment.BottomLeft;
            UseMnemonic = false;
        }
    }
}
