using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    public partial class aboutForm : Form
    {
        public aboutForm()
        {
            InitializeComponent();

            header.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            versionLabel.Text = "V " + Application.ProductVersion;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://Patreon.com/JnBean"); 
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCNcViqWJenLOgRs3KMiMD_g?sub_confirmation=1");
        }
    }
}
