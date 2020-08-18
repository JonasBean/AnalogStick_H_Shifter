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
            this.axisBox = new System.Windows.Forms.PictureBox();
            this.overlayBox = new System.Windows.Forms.PictureBox();
            this.gear1Button = new System.Windows.Forms.Button();
            this.gear2Button = new System.Windows.Forms.Button();
            this.gear3Button = new System.Windows.Forms.Button();
            this.gear4Button = new System.Windows.Forms.Button();
            this.gear5Button = new System.Windows.Forms.Button();
            this.gear6Button = new System.Windows.Forms.Button();
            this.gear9Button = new System.Windows.Forms.Button();
            this.colorPanel1 = new System.Windows.Forms.Panel();
            this.colorPanel2 = new System.Windows.Forms.Panel();
            this.colorPanel3 = new System.Windows.Forms.Panel();
            this.colorPanel4 = new System.Windows.Forms.Panel();
            this.colorPanel5 = new System.Windows.Forms.Panel();
            this.colorPanel6 = new System.Windows.Forms.Panel();
            this.colorPanelR = new System.Windows.Forms.Panel();
            this.gearRightNow = new System.Windows.Forms.Label();
            this.resetImageButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
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
            this.layoutNameBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.axisBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlayBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // activateControllerButton
            // 
            this.activateControllerButton.Location = new System.Drawing.Point(12, 12);
            this.activateControllerButton.Name = "activateControllerButton";
            this.activateControllerButton.Size = new System.Drawing.Size(114, 45);
            this.activateControllerButton.TabIndex = 0;
            this.activateControllerButton.Text = "Activate";
            this.activateControllerButton.UseVisualStyleBackColor = true;
            this.activateControllerButton.Click += new System.EventHandler(this.activateControllerButton_Click);
            // 
            // axisBox
            // 
            this.axisBox.BackColor = System.Drawing.Color.White;
            this.axisBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.axisBox.Location = new System.Drawing.Point(132, 12);
            this.axisBox.Name = "axisBox";
            this.axisBox.Size = new System.Drawing.Size(700, 700);
            this.axisBox.TabIndex = 2;
            this.axisBox.TabStop = false;
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
            // gear1Button
            // 
            this.gear1Button.Location = new System.Drawing.Point(863, 12);
            this.gear1Button.Name = "gear1Button";
            this.gear1Button.Size = new System.Drawing.Size(117, 30);
            this.gear1Button.TabIndex = 4;
            this.gear1Button.Text = "First gear";
            this.gear1Button.UseVisualStyleBackColor = true;
            this.gear1Button.Click += new System.EventHandler(this.gear1Button_Click);
            // 
            // gear2Button
            // 
            this.gear2Button.Location = new System.Drawing.Point(863, 48);
            this.gear2Button.Name = "gear2Button";
            this.gear2Button.Size = new System.Drawing.Size(117, 30);
            this.gear2Button.TabIndex = 5;
            this.gear2Button.Text = "Second gear";
            this.gear2Button.UseVisualStyleBackColor = true;
            this.gear2Button.Click += new System.EventHandler(this.gear2Button_Click);
            // 
            // gear3Button
            // 
            this.gear3Button.Location = new System.Drawing.Point(863, 84);
            this.gear3Button.Name = "gear3Button";
            this.gear3Button.Size = new System.Drawing.Size(117, 30);
            this.gear3Button.TabIndex = 6;
            this.gear3Button.Text = "Third gear";
            this.gear3Button.UseVisualStyleBackColor = true;
            this.gear3Button.Click += new System.EventHandler(this.gear3Button_Click);
            // 
            // gear4Button
            // 
            this.gear4Button.Location = new System.Drawing.Point(863, 120);
            this.gear4Button.Name = "gear4Button";
            this.gear4Button.Size = new System.Drawing.Size(117, 30);
            this.gear4Button.TabIndex = 7;
            this.gear4Button.Text = "Fourth gear";
            this.gear4Button.UseVisualStyleBackColor = true;
            this.gear4Button.Click += new System.EventHandler(this.gear4Button_Click);
            // 
            // gear5Button
            // 
            this.gear5Button.Location = new System.Drawing.Point(863, 156);
            this.gear5Button.Name = "gear5Button";
            this.gear5Button.Size = new System.Drawing.Size(117, 30);
            this.gear5Button.TabIndex = 8;
            this.gear5Button.Text = "Fifth gear";
            this.gear5Button.UseVisualStyleBackColor = true;
            this.gear5Button.Click += new System.EventHandler(this.gear5Button_Click);
            // 
            // gear6Button
            // 
            this.gear6Button.Location = new System.Drawing.Point(863, 192);
            this.gear6Button.Name = "gear6Button";
            this.gear6Button.Size = new System.Drawing.Size(117, 30);
            this.gear6Button.TabIndex = 9;
            this.gear6Button.Text = "Sixth gear";
            this.gear6Button.UseVisualStyleBackColor = true;
            this.gear6Button.Click += new System.EventHandler(this.gear6Button_Click);
            // 
            // gear9Button
            // 
            this.gear9Button.Location = new System.Drawing.Point(863, 228);
            this.gear9Button.Name = "gear9Button";
            this.gear9Button.Size = new System.Drawing.Size(117, 30);
            this.gear9Button.TabIndex = 10;
            this.gear9Button.Text = "Reverse gear";
            this.gear9Button.UseVisualStyleBackColor = true;
            this.gear9Button.Click += new System.EventHandler(this.gear9Button_Click);
            // 
            // colorPanel1
            // 
            this.colorPanel1.BackColor = System.Drawing.Color.Red;
            this.colorPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPanel1.Location = new System.Drawing.Point(838, 13);
            this.colorPanel1.Name = "colorPanel1";
            this.colorPanel1.Size = new System.Drawing.Size(19, 30);
            this.colorPanel1.TabIndex = 11;
            // 
            // colorPanel2
            // 
            this.colorPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.colorPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPanel2.Location = new System.Drawing.Point(838, 49);
            this.colorPanel2.Name = "colorPanel2";
            this.colorPanel2.Size = new System.Drawing.Size(19, 30);
            this.colorPanel2.TabIndex = 12;
            // 
            // colorPanel3
            // 
            this.colorPanel3.BackColor = System.Drawing.Color.Yellow;
            this.colorPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPanel3.Location = new System.Drawing.Point(838, 85);
            this.colorPanel3.Name = "colorPanel3";
            this.colorPanel3.Size = new System.Drawing.Size(19, 30);
            this.colorPanel3.TabIndex = 13;
            // 
            // colorPanel4
            // 
            this.colorPanel4.BackColor = System.Drawing.Color.Lime;
            this.colorPanel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPanel4.Location = new System.Drawing.Point(838, 121);
            this.colorPanel4.Name = "colorPanel4";
            this.colorPanel4.Size = new System.Drawing.Size(19, 30);
            this.colorPanel4.TabIndex = 13;
            // 
            // colorPanel5
            // 
            this.colorPanel5.BackColor = System.Drawing.Color.Aqua;
            this.colorPanel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPanel5.Location = new System.Drawing.Point(838, 157);
            this.colorPanel5.Name = "colorPanel5";
            this.colorPanel5.Size = new System.Drawing.Size(19, 30);
            this.colorPanel5.TabIndex = 13;
            // 
            // colorPanel6
            // 
            this.colorPanel6.BackColor = System.Drawing.Color.Blue;
            this.colorPanel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPanel6.Location = new System.Drawing.Point(838, 193);
            this.colorPanel6.Name = "colorPanel6";
            this.colorPanel6.Size = new System.Drawing.Size(19, 30);
            this.colorPanel6.TabIndex = 13;
            // 
            // colorPanelR
            // 
            this.colorPanelR.BackColor = System.Drawing.Color.Fuchsia;
            this.colorPanelR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPanelR.Location = new System.Drawing.Point(838, 229);
            this.colorPanelR.Name = "colorPanelR";
            this.colorPanelR.Size = new System.Drawing.Size(19, 30);
            this.colorPanelR.TabIndex = 13;
            // 
            // gearRightNow
            // 
            this.gearRightNow.AutoSize = true;
            this.gearRightNow.Location = new System.Drawing.Point(23, 630);
            this.gearRightNow.Name = "gearRightNow";
            this.gearRightNow.Size = new System.Drawing.Size(15, 13);
            this.gearRightNow.TabIndex = 16;
            this.gearRightNow.Text = "N";
            // 
            // resetImageButton
            // 
            this.resetImageButton.Location = new System.Drawing.Point(12, 64);
            this.resetImageButton.Name = "resetImageButton";
            this.resetImageButton.Size = new System.Drawing.Size(114, 45);
            this.resetImageButton.TabIndex = 18;
            this.resetImageButton.Text = "Reset image";
            this.resetImageButton.UseVisualStyleBackColor = true;
            this.resetImageButton.Click += new System.EventHandler(this.resetImageButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(12, 281);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(110, 24);
            this.saveButton.TabIndex = 19;
            this.saveButton.Text = "Save layout";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(838, 660);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(143, 52);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(838, 644);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "made by";
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
            this.xInputRadioButton.Location = new System.Drawing.Point(841, 292);
            this.xInputRadioButton.Name = "xInputRadioButton";
            this.xInputRadioButton.Size = new System.Drawing.Size(139, 17);
            this.xInputRadioButton.TabIndex = 32;
            this.xInputRadioButton.TabStop = true;
            this.xInputRadioButton.Text = "X Input (Xbox Controller)";
            this.xInputRadioButton.UseVisualStyleBackColor = true;
            this.xInputRadioButton.Click += new System.EventHandler(this.xInputRadioButton_Clicked);
            // 
            // directInputRadiobutton
            // 
            this.directInputRadiobutton.AutoSize = true;
            this.directInputRadiobutton.Location = new System.Drawing.Point(841, 315);
            this.directInputRadiobutton.Name = "directInputRadiobutton";
            this.directInputRadiobutton.Size = new System.Drawing.Size(132, 17);
            this.directInputRadiobutton.TabIndex = 33;
            this.directInputRadiobutton.Text = "Direct Input (Joysticks)";
            this.directInputRadiobutton.UseVisualStyleBackColor = true;
            this.directInputRadiobutton.Click += new System.EventHandler(this.directInputRadiobutton_Clicked);
            // 
            // joyStickListBox
            // 
            this.joyStickListBox.FormattingEnabled = true;
            this.joyStickListBox.Location = new System.Drawing.Point(856, 338);
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
            this.savedLayoutsListBox.SelectedIndexChanged += new System.EventHandler(this.savedLayoutsListBox_SelectedIndexChanged);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(12, 214);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(110, 24);
            this.loadButton.TabIndex = 36;
            this.loadButton.Text = "Load layout";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // layoutNameBox
            // 
            this.layoutNameBox.Location = new System.Drawing.Point(12, 255);
            this.layoutNameBox.Name = "layoutNameBox";
            this.layoutNameBox.Size = new System.Drawing.Size(110, 20);
            this.layoutNameBox.TabIndex = 37;
            this.layoutNameBox.Text = "LayoutName";
            // 
            // H_Shifter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(993, 727);
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
            this.Controls.Add(this.gearRightNow);
            this.Controls.Add(this.colorPanelR);
            this.Controls.Add(this.colorPanel6);
            this.Controls.Add(this.colorPanel5);
            this.Controls.Add(this.colorPanel4);
            this.Controls.Add(this.colorPanel3);
            this.Controls.Add(this.colorPanel2);
            this.Controls.Add(this.colorPanel1);
            this.Controls.Add(this.gear9Button);
            this.Controls.Add(this.gear6Button);
            this.Controls.Add(this.gear5Button);
            this.Controls.Add(this.gear4Button);
            this.Controls.Add(this.gear3Button);
            this.Controls.Add(this.gear2Button);
            this.Controls.Add(this.gear1Button);
            this.Controls.Add(this.overlayBox);
            this.Controls.Add(this.axisBox);
            this.Controls.Add(this.activateControllerButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "H_Shifter";
            this.Text = "Analogstick to 6 gear shifter";
            ((System.ComponentModel.ISupportInitialize)(this.axisBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlayBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button activateControllerButton;
        private System.Windows.Forms.PictureBox axisBox;
        private System.Windows.Forms.PictureBox overlayBox;
        private System.Windows.Forms.Button gear1Button;
        private System.Windows.Forms.Button gear2Button;
        private System.Windows.Forms.Button gear3Button;
        private System.Windows.Forms.Button gear4Button;
        private System.Windows.Forms.Button gear5Button;
        private System.Windows.Forms.Button gear6Button;
        private System.Windows.Forms.Button gear9Button;
        private System.Windows.Forms.Panel colorPanel1;
        private System.Windows.Forms.Panel colorPanel2;
        private System.Windows.Forms.Panel colorPanel3;
        private System.Windows.Forms.Panel colorPanel4;
        private System.Windows.Forms.Panel colorPanel5;
        private System.Windows.Forms.Panel colorPanel6;
        private System.Windows.Forms.Panel colorPanelR;
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
        private System.Windows.Forms.TextBox layoutNameBox;
    }
}

