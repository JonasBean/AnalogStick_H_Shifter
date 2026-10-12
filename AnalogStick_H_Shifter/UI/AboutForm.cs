using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    sealed class AboutForm : Form
    {
        const string PatreonUrl = "https://Patreon.com/JnBean";
        const string YouTubeUrl = "https://www.youtube.com/channel/UCNcViqWJenLOgRs3KMiMD_g?sub_confirmation=1";

        public AboutForm()
        {
            AutoScaleMode = AutoScaleMode.None;
            Text = "About";
            BackColor = Theme.Background;
            ForeColor = Theme.Text;
            Font = Theme.Body;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(Theme.S(440), Theme.S(420));
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch (ArgumentException) { }

            var card = new StackCard { Spacing = Theme.S(10), Location = new Point(Theme.S(16), Theme.S(16)), Width = ClientSize.Width - Theme.S(32) };

            var logo = new PictureBox
            {
                Image = Theme.Fit(Theme.Tint(Properties.Resources.LogoNeu___Kopie, Theme.Text), new Size(Theme.S(260), Theme.S(64))),
                SizeMode = PictureBoxSizeMode.CenterImage,
                Height = Theme.S(72),
                Cursor = Cursors.Hand,
                BackColor = Theme.Surface,
            };
            logo.Click += (s, e) => Open(YouTubeUrl);

            var title = new Label
            {
                Text = "AnalogStick H-Shifter",
                Font = Theme.Title,
                ForeColor = Theme.Text,
                Height = Theme.S(28),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            var version = new Label
            {
                Text = "Version " + typeof(AboutForm).Assembly.GetName().Version.ToString(3),
                Font = Theme.Small,
                ForeColor = Theme.TextMuted,
                Height = Theme.S(18),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            var text = new Label
            {
                Text = "Feel free to upload videos made with this software anywhere you like – " +
                       "please link to my YouTube channel or Patreon page.\n\n" +
                       "Found a bug? Leave a comment on YouTube.",
                ForeColor = Theme.TextMuted,
                Height = Theme.S(96),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            var patreon = new ModernButton { Text = "Patreon", Icon = Theme.Icons.Heart, Tag = HRow.Fill };
            patreon.Click += (s, e) => Open(PatreonUrl);
            var youtube = new ModernButton { Text = "YouTube", Icon = Theme.Icons.Play, Tag = HRow.Fill };
            youtube.Click += (s, e) => Open(YouTubeUrl);

            var ok = new ModernButton { Text = "Close", Variant = ButtonVariant.Primary, DialogResult = DialogResult.OK };

            card.AddRange(logo, title, version, text, new HRow(Theme.S(36), patreon, youtube), ok);
            Controls.Add(card);
            AcceptButton = ok;
            CancelButton = ok;

            ClientSize = new Size(ClientSize.Width, card.Bottom + Theme.S(16));
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Theme.UseDarkTitleBar(this);
        }

        static void Open(string url)
        {
            try { Process.Start(url); } catch (Exception ex) { Console.WriteLine("Could not open link: " + ex.Message); }
        }
    }
}
