using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    /// <summary>
    /// Interactive view of the stick range: shows the gear zones, the live stick position and a trail
    /// of recent positions. Zones can be moved (drag) and resized (drag a corner) with the mouse,
    /// or nudged with the arrow keys.
    /// </summary>
    sealed class ShifterCanvas : PaintedControl
    {
        const float MinZoneSize = 0.04f;
        const int MaxTrailPoints = 6000;

        enum DragMode { None, Move, TopLeft, TopRight, BottomLeft, BottomRight }

        ShifterLayout layout = new ShifterLayout();
        GearZone selected, hovered, engaged;
        PointF stick = new PointF(0.5f, 0.5f);
        bool connected;

        DragMode dragMode;
        GearZone dragZone;
        PointF dragStart;
        RectangleF dragStartBounds;
        bool dragged;

        readonly List<KeyValuePair<PointF, Color>> trail = new List<KeyValuePair<PointF, Color>>();
        Bitmap trailBitmap;
        int trailDrawn;
        readonly Dictionary<int, Font> fontCache = new Dictionary<int, Font>();

        public event EventHandler<GearZone> SelectedChanged;
        public event EventHandler LayoutEdited;

        public ShifterCanvas()
        {
            BackColor = Theme.Background;
            TabStop = true;
        }

        public ShifterLayout ShiftLayout
        {
            get => layout;
            set
            {
                layout = value ?? new ShifterLayout();
                if (!layout.Gears.Contains(selected)) selected = null;
                hovered = null;
                Invalidate();
            }
        }

        public GearZone Selected
        {
            get => selected;
            set { if (selected != value) { selected = value; Invalidate(); } }
        }

        public bool ShowTrail { get; set; } = true;

        public void SetStick(PointF position, bool isConnected, GearZone engagedZone)
        {
            bool moved = Math.Abs(position.X - stick.X) > 0.0005f || Math.Abs(position.Y - stick.Y) > 0.0005f;
            if (!moved && isConnected == connected && engagedZone == engaged) return;

            stick = position;
            connected = isConnected;
            engaged = engagedZone;

            if (moved && connected && ShowTrail)
            {
                trail.Add(new KeyValuePair<PointF, Color>(position, engaged != null ? Theme.GearColor(engaged) : Theme.TextMuted));
                if (trail.Count > MaxTrailPoints)
                {
                    trail.RemoveRange(0, MaxTrailPoints / 4);
                    trailDrawn = 0;
                    ClearTrailBitmap();
                }
            }

            Invalidate();
        }

        public void ClearTrail()
        {
            trail.Clear();
            trailDrawn = 0;
            ClearTrailBitmap();
            Invalidate();
        }

        // ------------------------------------------------------------------ geometry

        RectangleF Plot
        {
            get
            {
                float margin = Theme.S(30f);
                float size = Math.Max(10f, Math.Min(Width, Height) - 2 * margin);
                return new RectangleF((Width - size) / 2f, (Height - size) / 2f, size, size);
            }
        }

        PointF ToScreen(PointF n)
        {
            var p = Plot;
            return new PointF(p.X + n.X * p.Width, p.Y + n.Y * p.Height);
        }

        RectangleF ToScreen(RectangleF n)
        {
            var p = Plot;
            return new RectangleF(p.X + n.X * p.Width, p.Y + n.Y * p.Height, n.Width * p.Width, n.Height * p.Height);
        }

        PointF ToNormalized(Point s)
        {
            var p = Plot;
            return new PointF((s.X - p.X) / p.Width, (s.Y - p.Y) / p.Height);
        }

        static PointF[] Corners(RectangleF r) => new[]
        {
            new PointF(r.Left, r.Top), new PointF(r.Right, r.Top), new PointF(r.Left, r.Bottom), new PointF(r.Right, r.Bottom),
        };

        /// <summary>Finds what is under the mouse: a corner handle beats a zone body; later zones are on top.</summary>
        DragMode HitTest(Point p, out GearZone zone)
        {
            float grab = Theme.S(9f);
            var order = new List<GearZone>(layout.Gears);
            if (selected != null && order.Remove(selected)) order.Add(selected);

            for (int i = order.Count - 1; i >= 0; i--)
            {
                var corners = Corners(ToScreen(order[i].Bounds));
                for (int c = 0; c < 4; c++)
                {
                    if (Math.Abs(p.X - corners[c].X) <= grab && Math.Abs(p.Y - corners[c].Y) <= grab)
                    {
                        zone = order[i];
                        return DragMode.TopLeft + c;
                    }
                }
            }

            for (int i = order.Count - 1; i >= 0; i--)
            {
                if (ToScreen(order[i].Bounds).Contains(p)) { zone = order[i]; return DragMode.Move; }
            }

            zone = null;
            return DragMode.None;
        }

        static Cursor CursorFor(DragMode mode)
        {
            switch (mode)
            {
                case DragMode.Move: return Cursors.SizeAll;
                case DragMode.TopLeft:
                case DragMode.BottomRight: return Cursors.SizeNWSE;
                case DragMode.TopRight:
                case DragMode.BottomLeft: return Cursors.SizeNESW;
                default: return Cursors.Default;
            }
        }

        // ------------------------------------------------------------------ mouse & keyboard editing

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
            if (e.Button != MouseButtons.Left) return;

            dragMode = HitTest(e.Location, out dragZone);
            if (dragZone != selected)
            {
                selected = dragZone;
                SelectedChanged?.Invoke(this, selected);
            }

            if (dragZone != null)
            {
                dragStart = ToNormalized(e.Location);
                dragStartBounds = dragZone.Bounds;
                dragged = false;
                Capture = true;
            }
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (dragMode == DragMode.None || dragZone == null)
            {
                var mode = HitTest(e.Location, out var zone);
                Cursor = CursorFor(mode);
                if (zone != hovered) { hovered = zone; Invalidate(); }
                return;
            }

            var now = ToNormalized(e.Location);
            float dx = now.X - dragStart.X, dy = now.Y - dragStart.Y;
            var b = dragStartBounds;
            float left = b.Left, top = b.Top, right = b.Right, bottom = b.Bottom;

            switch (dragMode)
            {
                case DragMode.Move:
                    left = Clamp(b.Left + dx, 0f, 1f - b.Width);
                    top = Clamp(b.Top + dy, 0f, 1f - b.Height);
                    right = left + b.Width;
                    bottom = top + b.Height;
                    break;
                case DragMode.TopLeft: left = Clamp(b.Left + dx, 0f, right - MinZoneSize); top = Clamp(b.Top + dy, 0f, bottom - MinZoneSize); break;
                case DragMode.TopRight: right = Clamp(b.Right + dx, left + MinZoneSize, 1f); top = Clamp(b.Top + dy, 0f, bottom - MinZoneSize); break;
                case DragMode.BottomLeft: left = Clamp(b.Left + dx, 0f, right - MinZoneSize); bottom = Clamp(b.Bottom + dy, top + MinZoneSize, 1f); break;
                case DragMode.BottomRight: right = Clamp(b.Right + dx, left + MinZoneSize, 1f); bottom = Clamp(b.Bottom + dy, top + MinZoneSize, 1f); break;
            }

            dragZone.Bounds = RectangleF.FromLTRB(left, top, right, bottom);
            dragged = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Capture = false;
            bool edited = dragged;
            dragMode = DragMode.None;
            dragZone = null;
            dragged = false;
            if (edited) LayoutEdited?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (hovered != null && dragMode == DragMode.None) { hovered = null; Invalidate(); }
            base.OnMouseLeave(e);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            var key = keyData & Keys.KeyCode;
            return key == Keys.Left || key == Keys.Right || key == Keys.Up || key == Keys.Down || base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (selected == null) return;

            float step = e.Shift ? 0.02f : 0.004f;
            float dx = 0, dy = 0;
            switch (e.KeyCode)
            {
                case Keys.Left: dx = -step; break;
                case Keys.Right: dx = step; break;
                case Keys.Up: dy = -step; break;
                case Keys.Down: dy = step; break;
                case Keys.Escape: selected = null; SelectedChanged?.Invoke(this, null); Invalidate(); return;
                default: return;
            }

            var b = selected.Bounds;
            if (e.Control)
            {
                // Ctrl + arrows resize from the bottom-right corner.
                b.Width = Clamp(b.Width + dx, MinZoneSize, 1f - b.X);
                b.Height = Clamp(b.Height + dy, MinZoneSize, 1f - b.Y);
            }
            else
            {
                b.X = Clamp(b.X + dx, 0f, 1f - b.Width);
                b.Y = Clamp(b.Y + dy, 0f, 1f - b.Height);
            }
            selected.Bounds = b;
            e.Handled = true;
            Invalidate();
            LayoutEdited?.Invoke(this, EventArgs.Empty);
        }

        static float Clamp(float v, float min, float max) => v < min ? min : (v > max ? max : v);

        // ------------------------------------------------------------------ painting

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            trailBitmap?.Dispose();
            trailBitmap = null;
            trailDrawn = 0;
        }

        void ClearTrailBitmap()
        {
            if (trailBitmap == null) return;
            using (var g = Graphics.FromImage(trailBitmap)) g.Clear(Color.Transparent);
        }

        void UpdateTrailBitmap()
        {
            var plot = Plot;
            int w = (int)Math.Ceiling(plot.Width) + 1, h = (int)Math.Ceiling(plot.Height) + 1;
            if (trailBitmap == null || trailBitmap.Width != w || trailBitmap.Height != h)
            {
                trailBitmap?.Dispose();
                trailBitmap = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                trailDrawn = 0;
            }
            if (trailDrawn >= trail.Count) return;

            float d = Theme.S(4f);
            using (var g = Graphics.FromImage(trailBitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                for (int i = trailDrawn; i < trail.Count; i++)
                {
                    var p = trail[i].Key;
                    using (var brush = new SolidBrush(Theme.WithAlpha(trail[i].Value, 110)))
                        g.FillEllipse(brush, p.X * plot.Width - d / 2, p.Y * plot.Height - d / 2, d, d);
                }
            }
            trailDrawn = trail.Count;
        }

        Font ZoneFont(float pixelSize)
        {
            int key = Math.Max(10, Math.Min(96, (int)pixelSize));
            if (!fontCache.TryGetValue(key, out var font))
                fontCache[key] = font = new Font("Segoe UI Semibold", key, GraphicsUnit.Pixel);
            return font;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);
            Theme.SetupQuality(g);

            var card = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            Theme.FillRounded(g, Theme.Surface, card, Theme.S(12f));
            Theme.DrawRounded(g, Focused ? Theme.WithAlpha(Theme.Accent, 120) : Theme.Border, 1f, card, Theme.S(12f));

            var plot = Plot;
            PaintGrid(g, plot);

            if (ShowTrail && trail.Count > 0)
            {
                UpdateTrailBitmap();
                g.DrawImageUnscaled(trailBitmap, (int)plot.X, (int)plot.Y);
            }

            foreach (var zone in layout.Gears)
                if (zone != selected) PaintZone(g, zone);
            if (selected != null) PaintZone(g, selected);

            PaintStick(g, plot);

            TextRenderer.DrawText(g, "Drag a zone to move it  ·  drag a corner to resize  ·  arrow keys nudge (Ctrl = resize)",
                Theme.Small, new Rectangle(Theme.S(14), Height - Theme.S(26), Width - Theme.S(28), Theme.S(20)),
                Theme.TextDim, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        void PaintGrid(Graphics g, RectangleF plot)
        {
            Theme.FillRounded(g, Theme.Background, plot, Theme.S(8f));

            using (var minor = new Pen(Theme.WithAlpha(Theme.Border, 90), 1f))
            {
                for (int i = 1; i < 8; i++)
                {
                    float x = plot.X + plot.Width * i / 8f, y = plot.Y + plot.Height * i / 8f;
                    if (i == 4) continue;
                    g.DrawLine(minor, x, plot.Top, x, plot.Bottom);
                    g.DrawLine(minor, plot.Left, y, plot.Right, y);
                }
            }

            using (var axis = new Pen(Theme.Border, 1f))
            {
                g.DrawLine(axis, plot.X + plot.Width / 2, plot.Top, plot.X + plot.Width / 2, plot.Bottom);
                g.DrawLine(axis, plot.Left, plot.Y + plot.Height / 2, plot.Right, plot.Y + plot.Height / 2);
            }

            // Physical range of a round stick gate.
            using (var gate = new Pen(Theme.WithAlpha(Theme.TextDim, 120), 1f) { DashStyle = DashStyle.Dash })
                g.DrawEllipse(gate, plot);

            Theme.DrawRounded(g, Theme.Border, 1f, plot, Theme.S(8f));
        }

        void PaintZone(Graphics g, GearZone zone)
        {
            var r = ToScreen(zone.Bounds);
            if (r.Width < 2 || r.Height < 2) return;

            Color color = Theme.GearColor(zone);
            bool isEngaged = zone == engaged;
            bool isSelected = zone == selected;
            bool isHovered = zone == hovered;
            float radius = Math.Min(Theme.S(10f), Math.Min(r.Width, r.Height) / 4f);

            if (isEngaged)
            {
                using (var glow = new Pen(Theme.WithAlpha(color, 60), Theme.S(8f)))
                using (var path = Theme.RoundedRect(r, radius))
                    g.DrawPath(glow, path);
            }

            Theme.FillRounded(g, Theme.WithAlpha(color, isEngaged ? 95 : isHovered || isSelected ? 48 : 30), r, radius);
            Theme.DrawRounded(g, Theme.WithAlpha(color, isEngaged || isSelected ? 255 : isHovered ? 220 : 170),
                isEngaged ? Theme.S(2.5f) : Theme.S(1.5f), r, radius);

            // Gear label + key hint
            float labelSize = Math.Min(r.Width, r.Height) * 0.38f;
            var font = ZoneFont(Math.Min(labelSize, Theme.S(54f)));
            var textColor = isEngaged ? Color.White : Theme.WithAlpha(color, 235);
            bool showKey = r.Height > font.Height + Theme.S(28f);
            var labelRect = new RectangleF(r.X, r.Y - (showKey ? Theme.S(8f) : 0), r.Width, r.Height);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            Theme.DrawCentered(g, zone.Name, font, textColor, labelRect);

            if (showKey)
            {
                var keyRect = new RectangleF(r.X, r.Y + r.Height / 2f + font.Height / 2f - Theme.S(8f), r.Width, Theme.S(18f));
                Theme.DrawCentered(g, "Key " + KeySender.GetDisplayName(zone.Key), Theme.Small,
                    isEngaged ? Theme.WithAlpha(Color.White, 220) : Theme.TextMuted, keyRect);
            }

            if (isSelected || isHovered)
            {
                float h = Theme.S(isSelected ? 9f : 7f);
                foreach (var c in Corners(r))
                {
                    var hr = new RectangleF(c.X - h / 2, c.Y - h / 2, h, h);
                    using (var fill = new SolidBrush(isSelected ? Color.White : Theme.Surface))
                        g.FillEllipse(fill, hr);
                    using (var pen = new Pen(color, Theme.S(1.5f)))
                        g.DrawEllipse(pen, hr);
                }
            }
        }

        void PaintStick(Graphics g, RectangleF plot)
        {
            if (!connected)
            {
                var box = new RectangleF(plot.X + plot.Width / 2 - Theme.S(130f), plot.Y + plot.Height / 2 - Theme.S(20f), Theme.S(260f), Theme.S(40f));
                Theme.FillRounded(g, Theme.WithAlpha(Theme.Surface, 230), box, Theme.S(8f));
                Theme.DrawRounded(g, Theme.Border, 1f, box, Theme.S(8f));
                Theme.DrawCentered(g, "No controller detected", Theme.Body, Theme.TextMuted, box);
                return;
            }

            var center = new PointF(plot.X + plot.Width / 2, plot.Y + plot.Height / 2);
            var p = ToScreen(stick);
            Color color = engaged != null ? Theme.GearColor(engaged) : Color.White;

            using (var line = new Pen(Theme.WithAlpha(color, 90), Theme.S(1.5f)) { DashStyle = DashStyle.Dot })
                g.DrawLine(line, center, p);

            float outer = Theme.S(22f), inner = Theme.S(10f);
            using (var glow = new SolidBrush(Theme.WithAlpha(color, 50)))
                g.FillEllipse(glow, p.X - outer / 2, p.Y - outer / 2, outer, outer);
            using (var ring = new Pen(color, Theme.S(2f)))
                g.DrawEllipse(ring, p.X - outer / 2 + 2, p.Y - outer / 2 + 2, outer - 4, outer - 4);
            using (var dot = new SolidBrush(color))
                g.FillEllipse(dot, p.X - inner / 2, p.Y - inner / 2, inner, inner);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                trailBitmap?.Dispose();
                foreach (var f in fontCache.Values) f.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
