/* 
 * Halo Anticheat 2
 * ================
 * Name: HACValidate.Designer.cs
 * Description: Form Design for HACValidate
 *
 * This work is licensed under the Creative Commons Attribution-ShareAlike 3.0 Unported License.
 * View copy of License at: http://creativecommons.org/licenses/by-sa/3.0/
 * 
 * Copyright (C) 2010 Souless Productions
 * 
 */
namespace HAC2Beta2
{
    partial class HACValidate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CloseButton = new System.Windows.Forms.PictureBox();
            this.WindowTitle = new System.Windows.Forms.Label();
            this.OutputTxt = new System.Windows.Forms.TextBox();
            this.BGProg = new System.Windows.Forms.PictureBox();
            this.FGProg = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BGProg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FGProg)).BeginInit();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CloseButton.Image = global::HAC2Beta2.Properties.Resources.HACGlobal_CloseButton;
            this.CloseButton.Location = new System.Drawing.Point(294, 10);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(12, 12);
            this.CloseButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CloseButton.TabIndex = 15;
            this.CloseButton.TabStop = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // WindowTitle
            // 
            this.WindowTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WindowTitle.ForeColor = System.Drawing.Color.Black;
            this.WindowTitle.Image = global::HAC2Beta2.Properties.Resources.HACValidate_HandleBG;
            this.WindowTitle.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.WindowTitle.Location = new System.Drawing.Point(2, 2);
            this.WindowTitle.Name = "WindowTitle";
            this.WindowTitle.Size = new System.Drawing.Size(310, 26);
            this.WindowTitle.TabIndex = 14;
            this.WindowTitle.Text = "WindowTitle";
            this.WindowTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.WindowTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowTitle_MouseDown);
            // 
            // OutputTxt
            // 
            this.OutputTxt.Location = new System.Drawing.Point(8, 75);
            this.OutputTxt.Multiline = true;
            this.OutputTxt.Name = "OutputTxt";
            this.OutputTxt.Size = new System.Drawing.Size(300, 63);
            this.OutputTxt.TabIndex = 16;
            // 
            // BGProg
            // 
            this.BGProg.BackColor = System.Drawing.Color.Black;
            this.BGProg.Image = global::HAC2Beta2.Properties.Resources.HACValidate_BackProg;
            this.BGProg.Location = new System.Drawing.Point(8, 33);
            this.BGProg.Name = "BGProg";
            this.BGProg.Size = new System.Drawing.Size(300, 38);
            this.BGProg.TabIndex = 17;
            this.BGProg.TabStop = false;
            // 
            // FGProg
            // 
            this.FGProg.BackColor = System.Drawing.Color.DimGray;
            this.FGProg.Image = global::HAC2Beta2.Properties.Resources.HACValidate_FrontProg;
            this.FGProg.Location = new System.Drawing.Point(10, 35);
            this.FGProg.Name = "FGProg";
            this.FGProg.Size = new System.Drawing.Size(20, 34);
            this.FGProg.TabIndex = 18;
            this.FGProg.TabStop = false;
            // 
            // HACValidate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::HAC2Beta2.Properties.Resources.HACValidate_FormBG;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(318, 151);
            this.Controls.Add(this.FGProg);
            this.Controls.Add(this.BGProg);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.WindowTitle);
            this.Controls.Add(this.OutputTxt);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HACValidate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HAC :: Checking Maps";
            this.Load += new System.EventHandler(this.HACValidate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BGProg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FGProg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label WindowTitle;
        private System.Windows.Forms.PictureBox CloseButton;
        private System.Windows.Forms.TextBox OutputTxt;
        private System.Windows.Forms.PictureBox BGProg;
        private System.Windows.Forms.PictureBox FGProg;
    }
}