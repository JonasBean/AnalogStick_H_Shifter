using System.Drawing;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    partial class H_Shifter
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(H_Shifter));
            this.activateControllerButton = new System.Windows.Forms.Button();
            this.gearRightNow = new System.Windows.Forms.Label();
            this.resetImageButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.frstGearLabel = new System.Windows.Forms.Label();
            this.thrdGearLabel = new System.Windows.Forms.Label();
            this.ffthGearLabel = new System.Windows.Forms.Label();
            this.rvrsGearLabel = new System.Windows.Forms.Label();
            this.scndGearLabel = new System.Windows.Forms.Label();
            this.frthGearLabel = new System.Windows.Forms.Label();
            this.sxthGearLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ttlshift = new System.Windows.Forms.Label();
            this.xInputRadioButton = new System.Windows.Forms.RadioButton();
            this.directInputRadiobutton = new System.Windows.Forms.RadioButton();
            this.joyStickListBox = new System.Windows.Forms.ComboBox();
            this.savedLayoutsListBox = new System.Windows.Forms.ListBox();
            this.loadButton = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.gearDisplayPanel = new System.Windows.Forms.Panel();
            this.layoutNameBox = new System.Windows.Forms.TextBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.overlayBox = new System.Windows.Forms.PictureBox();
            this.axisBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlayBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axisBox)).BeginInit();
            this.SuspendLayout();
            // 
            // activateControllerButton
            // 
            this.activateControllerButton.Location = new System.Drawing.Point(12, 12);
            this.activateControllerButton.Name = "activateControllerButton";
            this.activateControllerButton.Size = new System.Drawing.Size(114, 45);
            this.activateControllerButton.TabIndex = 0;
            this.activateControllerButton.Text = "Activate";
            this.activateControllerButton.FlatStyle = FlatStyle.Flat;
            this.activateControllerButton.BackColor = Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.activateControllerButton.ForeColor = Color.White;
            this.activateControllerButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.activateControllerButton.FlatAppearance.BorderSize = 1;
            this.activateControllerButton.FlatAppearance.BorderColor = Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(70)))));
            this.activateControllerButton.Cursor = Cursors.Hand;
            this.activateControllerButton.Click += new System.EventHandler(this.activateControllerButton_Click);
            // 
            // 
            // gearDisplayPanel
            // 
            this.gearDisplayPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(50)))));
            this.gearDisplayPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gearDisplayPanel.Controls.Add(this.gearRightNow);
            this.gearDisplayPanel.Location = new System.Drawing.Point(20, 620);
            this.gearDisplayPanel.Name = "gearDisplayPanel";
            this.gearDisplayPanel.Size = new System.Drawing.Size(100, 100);
            this.gearDisplayPanel.TabIndex = 40;
            // 
            // gearRightNow
            // 
            this.gearRightNow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gearRightNow.Location = new System.Drawing.Point(0, 0);
            this.gearRightNow.Name = "gearRightNow";
            this.gearRightNow.Size = new System.Drawing.Size(78, 78);
            this.gearRightNow.TabIndex = 16;
            this.gearRightNow.Text = "N";
            this.gearRightNow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // resetImageButton
            // 
            this.resetImageButton.Location = new System.Drawing.Point(12, 64);
            this.resetImageButton.Name = "resetImageButton";
            this.resetImageButton.Size = new System.Drawing.Size(114, 45);
            this.resetImageButton.TabIndex = 18;
            this.resetImageButton.Text = "Clear Pattern";
            this.resetImageButton.FlatStyle = FlatStyle.Flat;
            this.resetImageButton.BackColor = Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.resetImageButton.ForeColor = Color.White;
            this.resetImageButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.resetImageButton.FlatAppearance.BorderSize = 1;
            this.resetImageButton.FlatAppearance.BorderColor = Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(70)))));
            this.resetImageButton.Cursor = Cursors.Hand;
            this.resetImageButton.Click += new System.EventHandler(this.resetImageButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(12, 281);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(110, 32);
            this.saveButton.TabIndex = 19;
            this.saveButton.Text = "Save Layout";
            this.saveButton.FlatStyle = FlatStyle.Flat;
            this.saveButton.BackColor = Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.saveButton.ForeColor = Color.White;
            this.saveButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.saveButton.FlatAppearance.BorderSize = 1;
            this.saveButton.FlatAppearance.BorderColor = Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(70)))));
            this.saveButton.Cursor = Cursors.Hand;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(835, 647);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "made by";
            // 
            // frstGearLabel
            // 
            this.frstGearLabel.AutoSize = true;
            this.frstGearLabel.Location = new System.Drawing.Point(33, 367);
            this.frstGearLabel.Name = "frstGearLabel";
            this.frstGearLabel.Size = new System.Drawing.Size(13, 13);
            this.frstGearLabel.TabIndex = 23;
            this.frstGearLabel.Text = "1";
            this.frstGearLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // thrdGearLabel
            // 
            this.thrdGearLabel.AutoSize = true;
            this.thrdGearLabel.Location = new System.Drawing.Point(66, 367);
            this.thrdGearLabel.Name = "thrdGearLabel";
            this.thrdGearLabel.Size = new System.Drawing.Size(13, 13);
            this.thrdGearLabel.TabIndex = 24;
            this.thrdGearLabel.Text = "3";
            this.thrdGearLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ffthGearLabel
            // 
            this.ffthGearLabel.AutoSize = true;
            this.ffthGearLabel.Location = new System.Drawing.Point(101, 367);
            this.ffthGearLabel.Name = "ffthGearLabel";
            this.ffthGearLabel.Size = new System.Drawing.Size(13, 13);
            this.ffthGearLabel.TabIndex = 25;
            this.ffthGearLabel.Text = "5";
            this.ffthGearLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rvrsGearLabel
            // 
            this.rvrsGearLabel.AutoSize = true;
            this.rvrsGearLabel.Location = new System.Drawing.Point(9, 393);
            this.rvrsGearLabel.Name = "rvrsGearLabel";
            this.rvrsGearLabel.Size = new System.Drawing.Size(15, 13);
            this.rvrsGearLabel.TabIndex = 26;
            this.rvrsGearLabel.Text = "R";
            this.rvrsGearLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scndGearLabel
            // 
            this.scndGearLabel.AutoSize = true;
            this.scndGearLabel.Location = new System.Drawing.Point(33, 521);
            this.scndGearLabel.Name = "scndGearLabel";
            this.scndGearLabel.Size = new System.Drawing.Size(13, 13);
            this.scndGearLabel.TabIndex = 27;
            this.scndGearLabel.Text = "2";
            this.scndGearLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frthGearLabel
            // 
            this.frthGearLabel.AutoSize = true;
            this.frthGearLabel.Location = new System.Drawing.Point(66, 521);
            this.frthGearLabel.Name = "frthGearLabel";
            this.frthGearLabel.Size = new System.Drawing.Size(13, 13);
            this.frthGearLabel.TabIndex = 28;
            this.frthGearLabel.Text = "4";
            this.frthGearLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sxthGearLabel
            // 
            this.sxthGearLabel.AutoSize = true;
            this.sxthGearLabel.Location = new System.Drawing.Point(101, 521);
            this.sxthGearLabel.Name = "sxthGearLabel";
            this.sxthGearLabel.Size = new System.Drawing.Size(13, 13);
            this.sxthGearLabel.TabIndex = 29;
            this.sxthGearLabel.Text = "6";
            this.sxthGearLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 546);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Total shifts:";
            // 
            // ttlshift
            // 
            this.ttlshift.AutoSize = true;
            this.ttlshift.Location = new System.Drawing.Point(12, 568);
            this.ttlshift.Name = "ttlshift";
            this.ttlshift.Size = new System.Drawing.Size(13, 13);
            this.ttlshift.TabIndex = 31;
            this.ttlshift.Text = "0";
            // 
            // xInputRadioButton
            // 
            this.xInputRadioButton.AutoSize = true;
            this.xInputRadioButton.Checked = true;
            this.xInputRadioButton.Location = new System.Drawing.Point(842, 26);
            this.xInputRadioButton.Name = "xInputRadioButton";
            this.xInputRadioButton.Size = new System.Drawing.Size(180, 17);
            this.xInputRadioButton.TabIndex = 32;
            this.xInputRadioButton.TabStop = true;
            this.xInputRadioButton.Text = "X Input (Xbox Controller)";
            this.xInputRadioButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.xInputRadioButton.ForeColor = Color.White;
            this.xInputRadioButton.UseVisualStyleBackColor = true;
            this.xInputRadioButton.Click += new System.EventHandler(this.xInputRadioButton_Clicked);
            // 
            // directInputRadiobutton
            // 
            this.directInputRadiobutton.AutoSize = true;
            this.directInputRadiobutton.Location = new System.Drawing.Point(842, 49);
            this.directInputRadiobutton.Name = "directInputRadiobutton";
            this.directInputRadiobutton.Size = new System.Drawing.Size(180, 17);
            this.directInputRadiobutton.TabIndex = 33;
            this.directInputRadiobutton.Text = "Direct Input (Joysticks)";
            this.directInputRadiobutton.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.directInputRadiobutton.ForeColor = Color.White;
            this.directInputRadiobutton.UseVisualStyleBackColor = true;
            this.directInputRadiobutton.Click += new System.EventHandler(this.directInputRadiobutton_Clicked);
            // 
            // joyStickListBox
            // 
            this.joyStickListBox.FormattingEnabled = true;
            this.joyStickListBox.Location = new System.Drawing.Point(857, 72);
            this.joyStickListBox.Name = "joyStickListBox";
            this.joyStickListBox.Size = new System.Drawing.Size(117, 21);
            this.joyStickListBox.TabIndex = 34;
            this.joyStickListBox.SelectedIndexChanged += new System.EventHandler(this.joyStickListBox_SelectedIndexChanged);
            // 
            // savedLayoutsListBox
            // 
            this.savedLayoutsListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.savedLayoutsListBox.FormattingEnabled = true;
            this.savedLayoutsListBox.Location = new System.Drawing.Point(12, 115);
            this.savedLayoutsListBox.Name = "savedLayoutsListBox";
            this.savedLayoutsListBox.Size = new System.Drawing.Size(114, 93);
            this.savedLayoutsListBox.TabIndex = 35;
            this.savedLayoutsListBox.BackColor = Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(50)))));
            this.savedLayoutsListBox.ForeColor = Color.White;
            this.savedLayoutsListBox.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.savedLayoutsListBox.SelectedIndexChanged += new System.EventHandler(this.savedLayoutsListBox_SelectedIndexChanged);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(12, 214);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(110, 32);
            this.loadButton.TabIndex = 36;
            this.loadButton.Text = "Load Layout";
            this.loadButton.FlatStyle = FlatStyle.Flat;
            this.loadButton.BackColor = Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.loadButton.ForeColor = Color.White;
            this.loadButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.loadButton.FlatAppearance.BorderSize = 1;
            this.loadButton.FlatAppearance.BorderColor = Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(70)))));
            this.loadButton.Cursor = Cursors.Hand;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // layoutNameBox
            // 
            this.layoutNameBox.Location = new System.Drawing.Point(12, 255);
            this.layoutNameBox.Name = "layoutNameBox";
            this.layoutNameBox.Size = new System.Drawing.Size(110, 25);
            this.layoutNameBox.TabIndex = 37;
            this.layoutNameBox.Text = "LayoutName";
            this.layoutNameBox.BackColor = Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(50)))));
            this.layoutNameBox.ForeColor = Color.White;
            this.layoutNameBox.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.layoutNameBox.BorderStyle = BorderStyle.FixedSingle;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox3.Image = global::AnalogStick_H_Shifter.Properties.Resources.YTLogo;
            this.pictureBox3.Location = new System.Drawing.Point(838, 581);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(65, 62);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 38;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(12, 393);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(114, 120);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 22;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(838, 663);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(143, 49);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // overlayBox
            // 
            this.overlayBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.overlayBox.Location = new System.Drawing.Point(132, 12);
            this.overlayBox.Name = "overlayBox";
            this.overlayBox.Size = new System.Drawing.Size(700, 700);
            this.overlayBox.TabIndex = 3;
            this.overlayBox.TabStop = false;
            this.overlayBox.Click += new System.EventHandler(this.overlayBox_Click);
            // 
            // axisBox
            // 
            this.axisBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(58)))));
            this.axisBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.axisBox.Location = new System.Drawing.Point(132, 12);
            this.axisBox.Name = "axisBox";
            this.axisBox.Size = new System.Drawing.Size(700, 700);
            this.axisBox.TabIndex = 2;
            this.axisBox.TabStop = false;
            // 
            // H_Shifter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(993, 727);
            this.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = Color.White;
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.layoutNameBox);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.savedLayoutsListBox);
            this.Controls.Add(this.joyStickListBox);
            this.Controls.Add(this.directInputRadiobutton);
            this.Controls.Add(this.xInputRadioButton);
            this.Controls.Add(this.ttlshift);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sxthGearLabel);
            this.Controls.Add(this.frthGearLabel);
            this.Controls.Add(this.scndGearLabel);
            this.Controls.Add(this.rvrsGearLabel);
            this.Controls.Add(this.ffthGearLabel);
            this.Controls.Add(this.thrdGearLabel);
            this.Controls.Add(this.frstGearLabel);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.resetImageButton);
            this.Controls.Add(this.gearDisplayPanel);
            this.Controls.Add(this.overlayBox);
            this.Controls.Add(this.axisBox);
            this.Controls.Add(this.activateControllerButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "H_Shifter";
            this.Text = "Analogstick to 6 gear shifter";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlayBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axisBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button activateControllerButton;
        private System.Windows.Forms.PictureBox axisBox;
        private System.Windows.Forms.PictureBox overlayBox;
        private System.Windows.Forms.Label gearRightNow;
        private System.Windows.Forms.Button resetImageButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label frstGearLabel;
        private System.Windows.Forms.Label thrdGearLabel;
        private System.Windows.Forms.Label ffthGearLabel;
        private System.Windows.Forms.Label rvrsGearLabel;
        private System.Windows.Forms.Label scndGearLabel;
        private System.Windows.Forms.Label frthGearLabel;
        private System.Windows.Forms.Label sxthGearLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ttlshift;
        private System.Windows.Forms.RadioButton xInputRadioButton;
        private System.Windows.Forms.RadioButton directInputRadiobutton;
        private System.Windows.Forms.ComboBox joyStickListBox;
        private System.Windows.Forms.ListBox savedLayoutsListBox;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel gearDisplayPanel;
        private System.Windows.Forms.TextBox layoutNameBox;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}

