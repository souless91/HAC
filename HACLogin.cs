/* 
 * Halo Anticheat 2
 * ================
 * Name: HACValidate.cs
 * Description: Form that does the necessary map scanning
 * 
 * This work is licensed under the Creative Commons Attribution-ShareAlike 3.0 Unported License.
 * View copy of License at: http://creativecommons.org/licenses/by-sa/3.0/
 * 
 * Copyright (C) 2010 Souless Productions
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
using System.Web;
using System.Net;
using System.IO;

namespace HAC2Beta2
{
    public partial class HACLogin : Form
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

        public HACLogin()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }
        #region Form Events

        private void HACLogin_Load(object sender, EventArgs e)
        {
            WindowTitle.Text = this.Text;
        }

        private void WindowTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("user="+textBox1.Text+"&pass="+textBox2.Text);

            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create("http://souless.me/hac/login.php");

            WebReq.Method = "POST";

            WebReq.ContentType = "application/x-www-form-urlencoded";

            WebReq.ContentLength = buffer.Length;

            Stream PostData = WebReq.GetRequestStream();

            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            Stream Answer = WebResp.GetResponseStream();
            StreamReader _Answer = new StreamReader(Answer);
            string responseString =  _Answer.ReadToEnd();

            switch (responseString)
            {
                case "0":
                    break;
                case "-1":
                    MessageBox.Show("User not found,\ndid you register for HAC?");
                    return;
                case "-2":
                    MessageBox.Show("Incorrect Password,\ndid you forget your password?");
                    return;
                case "-3":
                    MessageBox.Show("Please complete required fields");
                    return;
                default:
                    MessageBox.Show("Fatal Server Error - contact server admin");
                    return;
            }

            // Get next form without ugly Form hiding / Form dialoging
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(RunHACValidate));
            t.Start();
            this.Close();
        }

        #endregion

        #region More Methods to Come
        public static void RunHACValidate()
        {
            Application.Run(new HACValidate());
        }
        #endregion
    }
}
