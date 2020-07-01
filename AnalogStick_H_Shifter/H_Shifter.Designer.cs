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
            ((System.ComponentModel.ISupportInitialize)(this.axisBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlayBox)).BeginInit();
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
            this.gear1Button.Size = new System.Drawing.Size(117, 45);
            this.gear1Button.TabIndex = 4;
            this.gear1Button.Text = "First gear";
            this.gear1Button.UseVisualStyleBackColor = true;
            this.gear1Button.Click += new System.EventHandler(this.gear1Button_Click);
            // 
            // gear2Button
            // 
            this.gear2Button.Location = new System.Drawing.Point(863, 63);
            this.gear2Button.Name = "gear2Button";
            this.gear2Button.Size = new System.Drawing.Size(117, 45);
            this.gear2Button.TabIndex = 5;
            this.gear2Button.Text = "Second gear";
            this.gear2Button.UseVisualStyleBackColor = true;
            this.gear2Button.Click += new System.EventHandler(this.gear2Button_Click);
            // 
            // gear3Button
            // 
            this.gear3Button.Location = new System.Drawing.Point(863, 114);
            this.gear3Button.Name = "gear3Button";
            this.gear3Button.Size = new System.Drawing.Size(117, 45);
            this.gear3Button.TabIndex = 6;
            this.gear3Button.Text = "Third gear";
            this.gear3Button.UseVisualStyleBackColor = true;
            this.gear3Button.Click += new System.EventHandler(this.gear3Button_Click);
            // 
            // gear4Button
            // 
            this.gear4Button.Location = new System.Drawing.Point(863, 165);
            this.gear4Button.Name = "gear4Button";
            this.gear4Button.Size = new System.Drawing.Size(117, 45);
            this.gear4Button.TabIndex = 7;
            this.gear4Button.Text = "Fourth gear";
            this.gear4Button.UseVisualStyleBackColor = true;
            this.gear4Button.Click += new System.EventHandler(this.gear4Button_Click);
            // 
            // gear5Button
            // 
            this.gear5Button.Location = new System.Drawing.Point(863, 216);
            this.gear5Button.Name = "gear5Button";
            this.gear5Button.Size = new System.Drawing.Size(117, 45);
            this.gear5Button.TabIndex = 8;
            this.gear5Button.Text = "Fifth gear";
            this.gear5Button.UseVisualStyleBackColor = true;
            this.gear5Button.Click += new System.EventHandler(this.gear5Button_Click);
            // 
            // gear6Button
            // 
            this.gear6Button.Location = new System.Drawing.Point(863, 267);
            this.gear6Button.Name = "gear6Button";
            this.gear6Button.Size = new System.Drawing.Size(117, 45);
            this.gear6Button.TabIndex = 9;
            this.gear6Button.Text = "Sixth gear";
            this.gear6Button.UseVisualStyleBackColor = true;
            this.gear6Button.Click += new System.EventHandler(this.gear6Button_Click);
            // 
            // gear9Button
            // 
            this.gear9Button.Location = new System.Drawing.Point(863, 318);
            this.gear9Button.Name = "gear9Button";
            this.gear9Button.Size = new System.Drawing.Size(117, 45);
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
            this.colorPanel1.Size = new System.Drawing.Size(19, 43);
            this.colorPanel1.TabIndex = 11;
            // 
            // colorPanel2
            // 
            this.colorPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.colorPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPanel2.Location = new System.Drawing.Point(838, 64);
            this.colorPanel2.Name = "colorPanel2";
            this.colorPanel2.Size = new System.Drawing.Size(19, 43);
            this.colorPanel2.TabIndex = 12;
            // 
            // colorPanel3
            // 
            this.colorPanel3.BackColor = System.Drawing.Color.Yellow;
            this.colorPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPanel3.Location = new System.Drawing.Point(838, 115);
            this.colorPanel3.Name = "colorPanel3";
            this.colorPanel3.Size = new System.Drawing.Size(19, 43);
            this.colorPanel3.TabIndex = 13;
            // 
            // colorPanel4
            // 
            this.colorPanel4.BackColor = System.Drawing.Color.Lime;
            this.colorPanel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPanel4.Location = new System.Drawing.Point(838, 166);
            this.colorPanel4.Name = "colorPanel4";
            this.colorPanel4.Size = new System.Drawing.Size(19, 43);
            this.colorPanel4.TabIndex = 13;
            // 
            // colorPanel5
            // 
            this.colorPanel5.BackColor = System.Drawing.Color.Aqua;
            this.colorPanel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPanel5.Location = new System.Drawing.Point(838, 217);
            this.colorPanel5.Name = "colorPanel5";
            this.colorPanel5.Size = new System.Drawing.Size(19, 43);
            this.colorPanel5.TabIndex = 13;
            // 
            // colorPanel6
            // 
            this.colorPanel6.BackColor = System.Drawing.Color.Blue;
            this.colorPanel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPanel6.Location = new System.Drawing.Point(838, 268);
            this.colorPanel6.Name = "colorPanel6";
            this.colorPanel6.Size = new System.Drawing.Size(19, 43);
            this.colorPanel6.TabIndex = 13;
            // 
            // colorPanelR
            // 
            this.colorPanelR.BackColor = System.Drawing.Color.Fuchsia;
            this.colorPanelR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPanelR.Location = new System.Drawing.Point(838, 319);
            this.colorPanelR.Name = "colorPanelR";
            this.colorPanelR.Size = new System.Drawing.Size(19, 43);
            this.colorPanelR.TabIndex = 13;
            // 
            // gearRightNow
            // 
            this.gearRightNow.AutoSize = true;
            this.gearRightNow.Location = new System.Drawing.Point(23, 532);
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
            // H_Shifter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(993, 727);
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
            this.Name = "H_Shifter";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.axisBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlayBox)).EndInit();
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
    }
}

