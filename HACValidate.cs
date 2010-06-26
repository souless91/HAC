/* 
 * Halo Anticheat 2
 * ================
 * Name: HACValidate.cs
 * Description: Form that does the necessary map scanning
 *
 * Copyright 2010 (C) Souless Productions
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace HAC2Beta2
{
    public partial class HACValidate : Form
    {
        #region Cute Stuff Here
        // Improved custom window handle (moving form around screen)
        public const int WM_NCLBUTTONDOWN = 0xA1;
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        // Creates rounded form effect
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
            (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse, 
            int nHeightEllipse
        );
        #endregion

        public HACValidate()
        {
            // Cute trick to change progress / display status from across threads
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

            // Lets have a nice 10px radius rounded form
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 10, 10));
        }

        #region Form Events
        private void HACValidate_Load(object sender, EventArgs e)
        {
            WindowTitle.Text = " " + this.Text;

            // Fake Mapscanning checks in a new form
            Thread t0 = new Thread(new ThreadStart(MapValidation));
            t0.Start();
            // End checks
        }

        /// <summary>
        /// Moves the Window with custom handle
        /// </summary>
        private void WindowTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        /// <summary>
        /// Close button
        /// </summary>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region The Real Meat

        /// <summary>
        /// Method that takes the final steps after the mapscan - either success or failure
        /// </summary>
        /// <param name="success">Boolean indicating success/failure.</param>
        private void CompletedMapValidation(Boolean success)
        {
            if (!success) return;
            Thread t1 = new Thread(new ThreadStart(RunHACMain));
            t1.Start();
            this.Close();
        }

        /// <summary>
        /// Placeholder mapscanning method - kills some time so we can enjoy the Form's design
        /// </summary>
        private void MapValidation()
        {
            OutputTxt.Text += "Beavercreek...";
            OutputTxt.Text += "validated" + Environment.NewLine;
            ChangeProgressBar(10);
            Thread.Sleep(1000);

            OutputTxt.Text += "Damnation...";
            Thread.Sleep(300);
            OutputTxt.Text += "validated" + Environment.NewLine;
            ChangeProgressBar(40);
            Thread.Sleep(1000);

            OutputTxt.Text += "Wizard...";
            Thread.Sleep(250);
            OutputTxt.Text += "validated" + Environment.NewLine;
            ChangeProgressBar(70);
            Thread.Sleep(1000);

            OutputTxt.Text += "Deleting your CDrive...";
            Thread.Sleep(1000);
            OutputTxt.Text += "done" + Environment.NewLine;
            ChangeProgressBar(100);

            // When we're done with map scanning, let's send the results to CompletedMapValidation
            CompletedMapValidation(true);
        }

        /// <summary>
        /// Method to start next form - this should be launched in a new Thread
        /// </summary>
        private void RunHACMain()
        {
            Application.Run(new HACMain());
        }

        /// <summary>
        /// Modifies the pseudo-progressbar value to given value
        /// </summary>
        /// <param name="value">int value between 0 and 100</param>
        private void ChangeProgressBar(int value)
        {
            int updateTo = (value < 100) ? (value < 0) ? 0 : value : 100;
            int total = BGProg.Width - 4;
            FGProg.Width = (updateTo * total) / 100;
        }
        #endregion


    }
}
