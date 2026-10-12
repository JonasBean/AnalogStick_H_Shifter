using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    /// <summary>
    /// List of all gears with their key binding and shift count.
    /// Clicking the key chip starts listening for a new key (Esc cancels).
    /// </summary>
    sealed class GearListView : PaintedControl
    {
        IList<GearZone> gears = new List<GearZone>();
        Func<GearZone, int> countOf = _ => 0;
        GearZone engaged, selected;
        int hoverIndex = -1, listeningIndex = -1;
        bool hoverChip;

        public event EventHandler<GearZone> SelectedGearChanged;
        public event EventHandler<GearZone> KeyAssigned;

        public GearListView()
        {
            TabStop = true;
        }

        int RowHeight => Theme.S(40);

        public void SetGears(IList<GearZone> list, Func<GearZone, int> counter)
        {
            gears = list ?? new List<GearZone>();
            countOf = counter ?? (_ => 0);
            listeningIndex = -1;
            Height = Math.Max(RowHeight, gears.Count * RowHeight);
            Invalidate();
        }

        public GearZone Engaged { get => engaged; set { if (engaged != value) { engaged = value; Invalidate(); } } }
        public GearZone Selected { get => selected; set { if (selected != value) { selected = value; Invalidate(); } } }

        public bool IsListening => listeningIndex >= 0;

        Rectangle RowRect(int i) => new Rectangle(0, i * RowHeight, Width, RowHeight);

        Rectangle ChipRect(int i)
        {
            var row = RowRect(i);
            int w = Theme.S(92), h = Theme.S(26);
            return new Rectangle(row.Right - Theme.S(64) - w, row.Y + (row.Height - h) / 2, w, h);
        }

        int IndexAt(Point p) => p.Y >= 0 && p.Y < gears.Count * RowHeight ? p.Y / RowHeight : -1;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int i = IndexAt(e.Location);
            bool chip = i >= 0 && ChipRect(i).Contains(e.Location);
            if (i != hoverIndex || chip != hoverChip)
            {
                hoverIndex = i;
                hoverChip = chip;
                Cursor = i >= 0 ? Cursors.Hand : Cursors.Default;
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e) { hoverIndex = -1; hoverChip = false; base.OnMouseLeave(e); }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            int i = IndexAt(e.Location);
            if (i < 0 || e.Button != MouseButtons.Left) return;

            Focus();
            Selected = gears[i];
            SelectedGearChanged?.Invoke(this, gears[i]);

            listeningIndex = ChipRect(i).Contains(e.Location) && listeningIndex != i ? i : -1;
            Invalidate();
        }

        protected override bool IsInputKey(Keys keyData) => IsListening || base.IsInputKey(keyData);

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (IsListening)
            {
                e.Handled = e.SuppressKeyPress = true;
                var key = e.KeyCode;
                if (key == Keys.Escape) { listeningIndex = -1; Invalidate(); return; }
                // Modifiers only arrive as generic Shift/Ctrl/Alt – resolve the physical side.
                if (key == Keys.ShiftKey) key = IsDown(Keys.RShiftKey) ? Keys.RShiftKey : Keys.LShiftKey;
                if (key == Keys.ControlKey) key = IsDown(Keys.RControlKey) ? Keys.RControlKey : Keys.LControlKey;
                if (key == Keys.Menu) key = IsDown(Keys.RMenu) ? Keys.RMenu : Keys.LMenu;

                var gear = gears[listeningIndex];
                gear.Key = key;
                listeningIndex = -1;
                Invalidate();
                KeyAssigned?.Invoke(this, gear);
                return;
            }

            if (e.KeyCode == Keys.Enter && selected != null)
            {
                listeningIndex = gears.IndexOf(selected);
                Invalidate();
            }
            base.OnKeyDown(e);
        }

        static bool IsDown(Keys key) => (GetKeyState((int)key) & 0x8000) != 0;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern short GetKeyState(int key);

        protected override void OnLostFocus(EventArgs e)
        {
            listeningIndex = -1;
            base.OnLostFocus(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);
            Theme.SetupQuality(g);

            var duplicates = new HashSet<Keys>(gears.GroupBy(x => x.Key).Where(x => x.Count() > 1 && x.Key != Keys.None).Select(x => x.Key));

            for (int i = 0; i < gears.Count; i++)
            {
                var gear = gears[i];
                var row = RowRect(i);
                var inner = new RectangleF(row.X + 0.5f, row.Y + Theme.S(2f), row.Width - 1.5f, row.Height - Theme.S(4f));
                Color color = Theme.GearColor(gear);
                bool isEngaged = gear == engaged;

                if (isEngaged)
                    Theme.FillRounded(g, Theme.WithAlpha(color, 38), inner, Theme.S(8f));
                else if (gear == selected)
                    Theme.FillRounded(g, Theme.SurfaceRaised, inner, Theme.S(8f));
                else if (i == hoverIndex)
                    Theme.FillRounded(g, Theme.WithAlpha(Theme.SurfaceRaised, 160), inner, Theme.S(8f));

                if (gear == selected)
                    Theme.DrawRounded(g, Theme.WithAlpha(color, 140), 1f, inner, Theme.S(8f));

                // Gear badge
                float badge = Theme.S(26f);
                var badgeRect = new RectangleF(row.X + Theme.S(8f), row.Y + (row.Height - badge) / 2f, badge, badge);
                Theme.FillRounded(g, isEngaged ? color : Theme.WithAlpha(color, 45), badgeRect, Theme.S(7f));
                Theme.DrawCentered(g, gear.Name, Theme.BodyBold, isEngaged ? Theme.OnColor(color) : color, badgeRect);

                string label = gear.IsReverse ? "Reverse" : "Gear " + gear.Name;
                TextRenderer.DrawText(g, label, Theme.Body, new Rectangle(row.X + Theme.S(44), row.Y, Theme.S(90), row.Height),
                    Theme.Text, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

                // Key chip
                var chip = ChipRect(i);
                bool listening = i == listeningIndex;
                bool duplicate = duplicates.Contains(gear.Key);
                Color chipFill = listening ? Theme.Accent : (i == hoverIndex && hoverChip ? Theme.SurfaceHover : Theme.Background);
                Color chipBorder = listening ? Theme.Accent : duplicate ? Theme.Danger : Theme.Border;
                Theme.FillRounded(g, chipFill, chip, Theme.S(6f));
                Theme.DrawRounded(g, chipBorder, 1f, chip, Theme.S(6f));

                string chipText = listening ? "Press a key…" : KeySender.GetDisplayName(gear.Key);
                using (var iconFont = Theme.IconFont(9f))
                {
                    if (!listening)
                        Theme.DrawCentered(g, Theme.Icons.Keyboard, iconFont, Theme.TextDim, new RectangleF(chip.X + Theme.S(4), chip.Y, Theme.S(20), chip.Height));
                }
                var chipTextRect = listening ? chip : new Rectangle(chip.X + Theme.S(22), chip.Y, chip.Width - Theme.S(26), chip.Height);
                Theme.DrawCentered(g, chipText, listening ? Theme.Small : Theme.BodyBold,
                    listening ? Color.White : duplicate ? Theme.Danger : Theme.Text, chipTextRect);

                // Shift count
                TextRenderer.DrawText(g, countOf(gear).ToString("N0"), Theme.Body,
                    new Rectangle(chip.Right, row.Y, row.Right - chip.Right - Theme.S(10), row.Height),
                    Theme.TextMuted, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            }
        }
    }
}
