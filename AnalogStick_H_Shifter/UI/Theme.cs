using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    /// <summary>Colors, fonts, DPI scaling and drawing helpers shared by all custom controls.</summary>
    static class Theme
    {
        public static readonly Color Background = Color.FromArgb(14, 16, 20);
        public static readonly Color Surface = Color.FromArgb(22, 25, 32);
        public static readonly Color SurfaceRaised = Color.FromArgb(30, 34, 43);
        public static readonly Color SurfaceHover = Color.FromArgb(40, 45, 57);
        public static readonly Color Border = Color.FromArgb(44, 49, 61);
        public static readonly Color Text = Color.FromArgb(232, 235, 241);
        public static readonly Color TextMuted = Color.FromArgb(138, 146, 165);
        public static readonly Color TextDim = Color.FromArgb(92, 99, 116);
        public static readonly Color Accent = Color.FromArgb(91, 140, 255);
        public static readonly Color Success = Color.FromArgb(34, 197, 94);
        public static readonly Color Danger = Color.FromArgb(239, 68, 68);

        static readonly Color[] gearPalette =
        {
            Color.FromArgb(34, 197, 94),   // 1 green
            Color.FromArgb(245, 158, 11),  // 2 amber
            Color.FromArgb(56, 189, 248),  // 3 sky
            Color.FromArgb(167, 139, 250), // 4 violet
            Color.FromArgb(244, 114, 182), // 5 pink
            Color.FromArgb(250, 204, 21),  // 6 yellow
            Color.FromArgb(45, 212, 191),  // 7 teal
            Color.FromArgb(251, 146, 60),  // 8 orange
            Color.FromArgb(129, 140, 248), // 9 indigo
            Color.FromArgb(163, 230, 53),  // 10 lime
            Color.FromArgb(232, 121, 249), // 11 fuchsia
            Color.FromArgb(148, 163, 184), // 12 slate
        };

        public static Color GearColor(GearZone gear)
        {
            if (gear == null) return TextMuted;
            if (gear.IsReverse) return Danger;
            int n = gear.Number;
            return n >= 1 ? gearPalette[(n - 1) % gearPalette.Length] : TextMuted;
        }

        // ------------------------------------------------------------------ fonts

        public static readonly Font Body = new Font("Segoe UI", 9.75f);
        public static readonly Font BodyBold = new Font("Segoe UI Semibold", 9.75f);
        public static readonly Font Small = new Font("Segoe UI", 8.5f);
        public static readonly Font Caption = new Font("Segoe UI Semibold", 8f);
        public static readonly Font Title = new Font("Segoe UI Semibold", 13f);
        public static readonly Font Stat = new Font("Segoe UI Semibold", 20f);

        /// <summary>Segoe MDL2 Assets ships with Windows 10/11 and gives us crisp vector icons.</summary>
        public static readonly bool HasIconFont = IsFontInstalled("Segoe MDL2 Assets");

        public static Font IconFont(float size) => new Font(HasIconFont ? "Segoe MDL2 Assets" : "Segoe UI Symbol", size);

        public static class Icons
        {
            public const string Play = "\uE768";
            public const string Stop = "\uE71A";
            public const string Add = "\uE710";
            public const string Remove = "\uE738";
            public const string Save = "\uE74E";
            public const string Open = "\uE8E5";
            public const string Delete = "\uE74D";
            public const string Refresh = "\uE72C";
            public const string Erase = "\uE75C";
            public const string Grid = "\uE80A";
            public const string Info = "\uE946";
            public const string Reset = "\uE777";
            public const string Keyboard = "\uE765";
            public const string Gamepad = "\uE7FC";
            public const string Heart = "\uEB51";
        }

        static bool IsFontInstalled(string name)
        {
            using (var f = new Font(name, 10f)) return f.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
        }

        // ------------------------------------------------------------------ DPI

        static float scale = 1f;

        public static void InitDpi(Control control)
        {
            using (var g = control.CreateGraphics()) scale = g.DpiX / 96f;
        }

        public static int S(int px) => (int)Math.Round(px * scale);
        public static float S(float px) => px * scale;
        public static Padding S(int l, int t, int r, int b) => new Padding(S(l), S(t), S(r), S(b));

        // ------------------------------------------------------------------ drawing helpers

        public static GraphicsPath RoundedRect(RectangleF r, float radius)
        {
            var path = new GraphicsPath();
            float d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            if (d <= 0.5f) { path.AddRectangle(r); return path; }

            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static void FillRounded(Graphics g, Color color, RectangleF r, float radius)
        {
            using (var path = RoundedRect(r, radius))
            using (var brush = new SolidBrush(color))
                g.FillPath(brush, path);
        }

        public static void DrawRounded(Graphics g, Color color, float width, RectangleF r, float radius)
        {
            using (var path = RoundedRect(r, radius))
            using (var pen = new Pen(color, width))
                g.DrawPath(pen, path);
        }

        public static Color WithAlpha(Color c, int alpha) => Color.FromArgb(Math.Max(0, Math.Min(255, alpha)), c);

        public static Color Blend(Color a, Color b, float t) => Color.FromArgb(
            (int)(a.A + (b.A - a.A) * t), (int)(a.R + (b.R - a.R) * t),
            (int)(a.G + (b.G - a.G) * t), (int)(a.B + (b.B - a.B) * t));

        /// <summary>Readable text color (dark/light) for a given background.</summary>
        public static Color OnColor(Color background) =>
            background.R * 0.299 + background.G * 0.587 + background.B * 0.114 > 150 ? Color.FromArgb(15, 18, 24) : Color.White;

        public static void SetupQuality(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        }

        public static void DrawCentered(Graphics g, string text, Font font, Color color, RectangleF r)
        {
            using (var brush = new SolidBrush(color))
            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap })
                g.DrawString(text, font, brush, r, format);
        }

        /// <summary>
        /// Recolors a monochrome logo (dark artwork on white or transparent) into <paramref name="tint"/>
        /// on a transparent background, so the black logos work on the dark theme.
        /// </summary>
        public static Bitmap Tint(Image source, Color tint)
        {
            var bmp = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp)) g.DrawImage(source, 0, 0, source.Width, source.Height);

            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var data = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            var pixels = new byte[data.Stride * data.Height];
            Marshal.Copy(data.Scan0, pixels, 0, pixels.Length);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                int b = pixels[i], gr = pixels[i + 1], r = pixels[i + 2], a = pixels[i + 3];
                int luminance = (r * 299 + gr * 587 + b * 114) / 1000;
                pixels[i] = tint.B;
                pixels[i + 1] = tint.G;
                pixels[i + 2] = tint.R;
                pixels[i + 3] = (byte)(a * (255 - luminance) / 255);
            }

            Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);
            bmp.UnlockBits(data);
            return bmp;
        }

        /// <summary>Downscales a large image once (high quality) so painting it later is cheap and crisp.</summary>
        public static Bitmap Fit(Image source, Size max)
        {
            float f = Math.Min((float)max.Width / source.Width, (float)max.Height / source.Height);
            var size = new Size(Math.Max(1, (int)(source.Width * f)), Math.Max(1, (int)(source.Height * f)));
            var bmp = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(source, new Rectangle(Point.Empty, size));
            }
            return bmp;
        }

        // ------------------------------------------------------------------ window chrome

        [DllImport("dwmapi.dll")]
        static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);

        /// <summary>Dark title bar on Windows 10 (1809+) and Windows 11.</summary>
        public static void UseDarkTitleBar(Form form)
        {
            try
            {
                int on = 1;
                if (DwmSetWindowAttribute(form.Handle, 20, ref on, sizeof(int)) != 0)
                    DwmSetWindowAttribute(form.Handle, 19, ref on, sizeof(int));

                // Windows 11: caption color matching the app background (ignored on Windows 10).
                int caption = ColorTranslator.ToWin32(Background);
                DwmSetWindowAttribute(form.Handle, 35, ref caption, sizeof(int));
            }
            catch (DllNotFoundException) { }
            catch (EntryPointNotFoundException) { }
        }
    }
}
