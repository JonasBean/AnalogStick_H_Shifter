using System;
using System.Linq;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    /// <summary>Stacks visible children vertically at full width (keeping each child's height).</summary>
    static class StackLayout
    {
        public static int Arrange(Control parent, int spacing, int scrollOffset = 0)
        {
            var pad = parent.Padding;
            int width = parent.ClientSize.Width - pad.Horizontal;
            if (parent is ScrollableControl sc && sc.VerticalScroll.Visible) width -= SystemInformation.VerticalScrollBarWidth;

            int y = pad.Top + scrollOffset;
            bool first = true;
            foreach (Control child in parent.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
            {
                if (!child.Visible) continue;
                if (!first) y += spacing;
                y += child.Margin.Top;
                child.SetBounds(pad.Left, y, Math.Max(0, width), child.Height);
                y += child.Height + child.Margin.Bottom;
                first = false;
            }
            return y - scrollOffset + pad.Bottom;
        }
    }

    /// <summary>Card whose children are stacked vertically; the card grows/shrinks to fit them.</summary>
    sealed class StackCard : CardPanel
    {
        bool arranging;

        public int Spacing { get; set; } = Theme.S(8);

        public void AddRange(params Control[] controls)
        {
            int index = Controls.Count;
            foreach (var c in controls)
            {
                c.TabIndex = index++;
                Controls.Add(c);
            }
            PerformLayout();
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            if (arranging) return;
            arranging = true;
            try
            {
                int height = StackLayout.Arrange(this, Spacing);
                if (Height != height) Height = height;
            }
            finally { arranging = false; }
            Parent?.PerformLayout();
        }
    }

    /// <summary>Scrollable vertical stack (used for the side columns).</summary>
    sealed class ScrollStack : Panel
    {
        bool arranging;

        public ScrollStack()
        {
            AutoScroll = true;
            BackColor = Theme.Background;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

        public int Spacing { get; set; } = Theme.S(12);

        public void AddRange(params Control[] controls)
        {
            int index = Controls.Count;
            foreach (var c in controls)
            {
                c.TabIndex = index++;
                Controls.Add(c);
            }
            PerformLayout();
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            if (arranging) return;
            arranging = true;
            try
            {
                int height = StackLayout.Arrange(this, Spacing, AutoScrollPosition.Y);
                AutoScrollMinSize = new System.Drawing.Size(0, height);
            }
            finally { arranging = false; }
        }
    }

    /// <summary>Lays children out horizontally; children with <see cref="Fill"/> share the remaining width.</summary>
    sealed class HRow : Panel
    {
        public const string Fill = "fill";

        public HRow(int height, params Control[] controls)
        {
            Height = height;
            BackColor = Theme.Surface;
            for (int i = 0; i < controls.Length; i++)
            {
                controls[i].TabIndex = i;
                Controls.Add(controls[i]);
            }
        }

        public int Spacing { get; set; } = Theme.S(8);

        protected override void OnLayout(LayoutEventArgs e)
        {
            var visible = Controls.Cast<Control>().Where(c => c.Visible).OrderBy(c => c.TabIndex).ToList();
            if (visible.Count == 0) return;

            int fixedWidth = visible.Where(c => !Fill.Equals(c.Tag)).Sum(c => c.Width);
            int fillCount = visible.Count(c => Fill.Equals(c.Tag));
            int fillWidth = fillCount == 0 ? 0 : Math.Max(0, (ClientSize.Width - fixedWidth - Spacing * (visible.Count - 1)) / fillCount);

            int x = 0;
            foreach (var c in visible)
            {
                int w = Fill.Equals(c.Tag) ? fillWidth : c.Width;
                int h = c is ComboBox ? c.Height : Height;
                c.SetBounds(x, (Height - h) / 2, w, h);
                x += w + Spacing;
            }
        }
    }
}
