using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    /// <summary>Big round display of the currently engaged gear.</summary>
    sealed class GearIndicator : PaintedControl
    {
        GearZone gear;
        bool outputEnabled;
        bool connected;

        public GearIndicator()
        {
            Height = Theme.S(180);
        }

        public void SetState(GearZone engaged, bool output, bool isConnected)
        {
            if (engaged == gear && output == outputEnabled && isConnected == connected) return;
            gear = engaged;
            outputEnabled = output;
            connected = isConnected;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);
            Theme.SetupQuality(g);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            float size = System.Math.Min(Width, Height - Theme.S(44f)) - Theme.S(8f);
            var circle = new RectangleF((Width - size) / 2f, Theme.S(4f), size, size);

            Color color = gear != null ? Theme.GearColor(gear) : Theme.TextDim;
            if (gear != null && !outputEnabled) color = Theme.Blend(color, Theme.Surface, 0.45f);

            // Soft glow behind the ring when a gear is engaged.
            if (gear != null)
            {
                using (var path = new GraphicsPath())
                {
                    path.AddEllipse(RectangleF.Inflate(circle, Theme.S(4f), Theme.S(4f)));
                    using (var glow = new PathGradientBrush(path)
                    {
                        CenterColor = Theme.WithAlpha(color, outputEnabled ? 70 : 30),
                        SurroundColors = new[] { Theme.WithAlpha(color, 0) },
                    })
                        g.FillPath(glow, path);
                }
            }

            var inner = RectangleF.Inflate(circle, -Theme.S(12f), -Theme.S(12f));
            using (var fill = new SolidBrush(Theme.Background))
                g.FillEllipse(fill, inner);
            using (var track = new Pen(Theme.SurfaceRaised, Theme.S(6f)))
                g.DrawEllipse(track, inner);
            if (gear != null)
                using (var ring = new Pen(color, Theme.S(6f)))
                    g.DrawEllipse(ring, inner);

            string text = gear?.Name ?? "N";
            using (var font = new Font("Segoe UI Semibold", System.Math.Max(8f, size * 0.36f), GraphicsUnit.Pixel))
                Theme.DrawCentered(g, text, font, gear != null ? color : Theme.TextMuted, new RectangleF(inner.X, inner.Y + Theme.S(2f), inner.Width, inner.Height));

            string status;
            if (!connected) status = "NO CONTROLLER";
            else if (gear == null) status = "NEUTRAL";
            else status = (gear.IsReverse ? "REVERSE" : "GEAR " + gear.Name) + "  ·  KEY " + KeySender.GetDisplayName(gear.Key).ToUpperInvariant();

            var statusRect = new RectangleF(0, circle.Bottom + Theme.S(8f), Width, Theme.S(20f));
            Theme.DrawCentered(g, status, Theme.Caption, Theme.TextMuted, statusRect);

            if (!outputEnabled)
                Theme.DrawCentered(g, "Output paused – press Activate", Theme.Small, Theme.TextDim,
                    new RectangleF(0, statusRect.Bottom, Width, Theme.S(18f)));
        }
    }
}
