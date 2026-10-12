using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    /// <summary>Small dark prompt asking for a single line of text.</summary>
    sealed class InputDialog : Form
    {
        readonly TextBox input;
        readonly Label error;
        readonly Func<string, string> validate;

        InputDialog(string title, string prompt, string value, Func<string, string> validate)
        {
            this.validate = validate;

            AutoScaleMode = AutoScaleMode.None;
            Text = title;
            BackColor = Theme.Background;
            ForeColor = Theme.Text;
            Font = Theme.Body;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(Theme.S(380), Theme.S(100));

            input = new TextBox
            {
                Text = value,
                BackColor = Theme.SurfaceRaised,
                ForeColor = Theme.Text,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 11f),
                Tag = HRow.Fill,
            };
            input.TextChanged += (s, e) => error.Text = "";

            error = new ThemedLabel("", Theme.Small, Theme.Danger) { Height = Theme.S(18) };

            var ok = new ModernButton { Text = "Save", Variant = ButtonVariant.Primary, Width = Theme.S(100) };
            ok.Click += (s, e) => Accept();
            var cancel = new ModernButton { Text = "Cancel", Width = Theme.S(100), DialogResult = DialogResult.Cancel };

            var spacer = new Panel { Tag = HRow.Fill, BackColor = Theme.Surface };
            var card = new StackCard { Spacing = Theme.S(8), Location = new Point(Theme.S(12), Theme.S(12)), Width = ClientSize.Width - Theme.S(24) };
            card.AddRange(
                new ThemedLabel(prompt),
                new HRow(input.PreferredHeight, input),
                error,
                new HRow(Theme.S(34), spacer, cancel, ok));
            Controls.Add(card);

            AcceptButton = ok;
            CancelButton = cancel;
            ClientSize = new Size(ClientSize.Width, card.Bottom + Theme.S(12));
        }

        void Accept()
        {
            string value = input.Text.Trim();
            string problem = validate?.Invoke(value);
            if (problem != null)
            {
                error.Text = problem;
                input.Focus();
                return;
            }
            input.Text = value;
            DialogResult = DialogResult.OK;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            input.Focus();
            input.SelectAll();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Theme.UseDarkTitleBar(this);
        }

        /// <summary>Returns the entered text, or null if the dialog was cancelled.</summary>
        public static string Show(IWin32Window owner, string title, string prompt, string value, Func<string, string> validate = null)
        {
            using (var dialog = new InputDialog(title, prompt, value, validate))
                return dialog.ShowDialog(owner) == DialogResult.OK ? dialog.input.Text : null;
        }
    }
}
