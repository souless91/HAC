/* 
 * Halo Anticheat 2
 * ================
 * Name: HACMain.Designer.cs
 * Description: Form Design for HACMain
 *
 * This work is licensed under the Creative Commons Attribution-ShareAlike 3.0 Unported License.
 * View copy of License at: http://creativecommons.org/licenses/by-sa/3.0/
 * 
 * Copyright (C) 2010 Souless Productions
 * 
 */
namespace HAC2Beta2
{
    partial class HACMain
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
            this.MenuHAC = new System.Windows.Forms.PictureBox();
            this.MenuServers = new System.Windows.Forms.PictureBox();
            this.MenuChat = new System.Windows.Forms.PictureBox();
            this.Tabbings = new System.Windows.Forms.TabControl();
            this.HACPage = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ping = new System.Windows.Forms.Label();
            this.ServerPass = new System.Windows.Forms.TextBox();
            this.ServerIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Arguments = new System.Windows.Forms.TextBox();
            this.BigBoy = new System.Windows.Forms.Button();
            this.ServersPage = new System.Windows.Forms.TabPage();
            this.filtering = new System.Windows.Forms.GroupBox();
            this.MapCombo = new System.Windows.Forms.ComboBox();
            this.ResetButton = new System.Windows.Forms.Button();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.checkBox13 = new System.Windows.Forms.CheckBox();
            this.checkBox12 = new System.Windows.Forms.CheckBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.GametypeCombo = new System.Windows.Forms.ComboBox();
            this.VersionCombo = new System.Windows.Forms.ComboBox();
            this.ServerBrowserList = new HAC2Beta2.ObjectListView();
            this.olvColumn1 = ((HAC2Beta2.OLVColumn)(new HAC2Beta2.OLVColumn()));
            this.olvColumn2 = ((HAC2Beta2.OLVColumn)(new HAC2Beta2.OLVColumn()));
            this.olvColumn3 = ((HAC2Beta2.OLVColumn)(new HAC2Beta2.OLVColumn()));
            this.olvColumn4 = ((HAC2Beta2.OLVColumn)(new HAC2Beta2.OLVColumn()));
            this.olvColumn5 = ((HAC2Beta2.OLVColumn)(new HAC2Beta2.OLVColumn()));
            this.olvColumn6 = ((HAC2Beta2.OLVColumn)(new HAC2Beta2.OLVColumn()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.GametypePicture = new System.Windows.Forms.PictureBox();
            this.MapPicture = new System.Windows.Forms.PictureBox();
            this.GamenameTxt = new System.Windows.Forms.Label();
            this.GameNameDetail = new System.Windows.Forms.Label();
            this.VersionTxt = new System.Windows.Forms.Label();
            this.VersionDetail = new System.Windows.Forms.Label();
            this.PlayersTxt = new System.Windows.Forms.Label();
            this.NameTxt = new System.Windows.Forms.Label();
            this.PlayersDetail = new System.Windows.Forms.Label();
            this.NameDetail = new System.Windows.Forms.Label();
            this.ChatPage = new System.Windows.Forms.TabPage();
            this.ServerCount = new System.Windows.Forms.Label();
            this.ServersLbl = new System.Windows.Forms.Label();
            this.PlayersLbl = new System.Windows.Forms.Label();
            this.PlayerCount = new System.Windows.Forms.Label();
            this.WarningLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MenuHAC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MenuServers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MenuChat)).BeginInit();
            this.Tabbings.SuspendLayout();
            this.HACPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.ServersPage.SuspendLayout();
            this.filtering.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ServerBrowserList)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GametypePicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CloseButton.Image = global::HAC2Beta2.Properties.Resources.HACGlobal_CloseButton;
            this.CloseButton.Location = new System.Drawing.Point(757, 12);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(12, 12);
            this.CloseButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CloseButton.TabIndex = 17;
            this.CloseButton.TabStop = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // WindowTitle
            // 
            this.WindowTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WindowTitle.ForeColor = System.Drawing.Color.Black;
            this.WindowTitle.Image = global::HAC2Beta2.Properties.Resources.HACMain_HandleBG;
            this.WindowTitle.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.WindowTitle.Location = new System.Drawing.Point(3, 3);
            this.WindowTitle.Name = "WindowTitle";
            this.WindowTitle.Size = new System.Drawing.Size(771, 45);
            this.WindowTitle.TabIndex = 16;
            this.WindowTitle.Text = "WindowTitle";
            this.WindowTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.WindowTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowTitle_MouseDown);
            // 
            // MenuHAC
            // 
            this.MenuHAC.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MenuHAC.Image = global::HAC2Beta2.Properties.Resources.HACMain_MenuHomeH;
            this.MenuHAC.Location = new System.Drawing.Point(9, 47);
            this.MenuHAC.Name = "MenuHAC";
            this.MenuHAC.Size = new System.Drawing.Size(103, 23);
            this.MenuHAC.TabIndex = 19;
            this.MenuHAC.TabStop = false;
            this.MenuHAC.Click += new System.EventHandler(this.MenuHAC_Click);
            this.MenuHAC.MouseEnter += new System.EventHandler(this.MenuHAC_MouseEnter);
            this.MenuHAC.MouseLeave += new System.EventHandler(this.MenuHAC_MouseLeave);
            // 
            // MenuServers
            // 
            this.MenuServers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MenuServers.Image = global::HAC2Beta2.Properties.Resources.HACMain_MenuServersI;
            this.MenuServers.Location = new System.Drawing.Point(111, 55);
            this.MenuServers.Name = "MenuServers";
            this.MenuServers.Size = new System.Drawing.Size(103, 23);
            this.MenuServers.TabIndex = 19;
            this.MenuServers.TabStop = false;
            this.MenuServers.Click += new System.EventHandler(this.MenuServers_Click);
            this.MenuServers.MouseEnter += new System.EventHandler(this.MenuServers_MouseEnter);
            this.MenuServers.MouseLeave += new System.EventHandler(this.MenuServers_MouseLeave);
            // 
            // MenuChat
            // 
            this.MenuChat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MenuChat.Image = global::HAC2Beta2.Properties.Resources.HACMain_MenuChatI;
            this.MenuChat.Location = new System.Drawing.Point(213, 55);
            this.MenuChat.Name = "MenuChat";
            this.MenuChat.Size = new System.Drawing.Size(103, 23);
            this.MenuChat.TabIndex = 20;
            this.MenuChat.TabStop = false;
            this.MenuChat.Click += new System.EventHandler(this.MenuChat_Click);
            this.MenuChat.MouseEnter += new System.EventHandler(this.MenuChat_MouseEnter);
            this.MenuChat.MouseLeave += new System.EventHandler(this.MenuChat_MouseLeave);
            // 
            // Tabbings
            // 
            this.Tabbings.Controls.Add(this.HACPage);
            this.Tabbings.Controls.Add(this.ServersPage);
            this.Tabbings.Controls.Add(this.ChatPage);
            this.Tabbings.Location = new System.Drawing.Point(9, 56);
            this.Tabbings.Name = "Tabbings";
            this.Tabbings.SelectedIndex = 0;
            this.Tabbings.Size = new System.Drawing.Size(764, 434);
            this.Tabbings.TabIndex = 21;
            // 
            // HACPage
            // 
            this.HACPage.Controls.Add(this.groupBox1);
            this.HACPage.Controls.Add(this.button2);
            this.HACPage.Controls.Add(this.checkBox10);
            this.HACPage.Controls.Add(this.checkBox9);
            this.HACPage.Controls.Add(this.checkBox8);
            this.HACPage.Controls.Add(this.checkBox7);
            this.HACPage.Controls.Add(this.checkBox6);
            this.HACPage.Controls.Add(this.checkBox5);
            this.HACPage.Controls.Add(this.checkBox4);
            this.HACPage.Controls.Add(this.checkBox3);
            this.HACPage.Controls.Add(this.checkBox2);
            this.HACPage.Controls.Add(this.checkBox1);
            this.HACPage.Controls.Add(this.label1);
            this.HACPage.Controls.Add(this.Arguments);
            this.HACPage.Controls.Add(this.BigBoy);
            this.HACPage.Location = new System.Drawing.Point(4, 22);
            this.HACPage.Name = "HACPage";
            this.HACPage.Padding = new System.Windows.Forms.Padding(3);
            this.HACPage.Size = new System.Drawing.Size(756, 408);
            this.HACPage.TabIndex = 0;
            this.HACPage.Text = "HAC";
            this.HACPage.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.ping);
            this.groupBox1.Controls.Add(this.ServerPass);
            this.groupBox1.Controls.Add(this.ServerIP);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(13, 174);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(314, 75);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Direct Connect";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(255, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Ping:";
            // 
            // ping
            // 
            this.ping.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ping.Location = new System.Drawing.Point(234, 30);
            this.ping.Name = "ping";
            this.ping.Size = new System.Drawing.Size(74, 33);
            this.ping.TabIndex = 18;
            this.ping.Text = "N/A";
            this.ping.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ServerPass
            // 
            this.ServerPass.Location = new System.Drawing.Point(84, 45);
            this.ServerPass.Name = "ServerPass";
            this.ServerPass.Size = new System.Drawing.Size(150, 20);
            this.ServerPass.TabIndex = 17;
            // 
            // ServerIP
            // 
            this.ServerIP.Location = new System.Drawing.Point(84, 19);
            this.ServerIP.Name = "ServerIP";
            this.ServerIP.Size = new System.Drawing.Size(150, 20);
            this.ServerIP.TabIndex = 16;
            this.ServerIP.Leave += new System.EventHandler(this.ServerIP_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Server IP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Server Pass:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(407, 93);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(154, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Clear Arguments";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.Location = new System.Drawing.Point(175, 148);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(74, 17);
            this.checkBox10.TabIndex = 12;
            this.checkBox10.Text = "Safemode";
            this.checkBox10.UseVisualStyleBackColor = true;
            this.checkBox10.CheckedChanged += new System.EventHandler(this.checkBox10_CheckedChanged);
            // 
            // checkBox9
            // 
            this.checkBox9.AutoSize = true;
            this.checkBox9.Location = new System.Drawing.Point(175, 124);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(125, 17);
            this.checkBox9.TabIndex = 11;
            this.checkBox9.Text = "Force Fixed Function";
            this.checkBox9.UseVisualStyleBackColor = true;
            this.checkBox9.CheckedChanged += new System.EventHandler(this.checkBox9_CheckedChanged);
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.Location = new System.Drawing.Point(175, 100);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(108, 17);
            this.checkBox8.TabIndex = 10;
            this.checkBox8.Text = "Force 1.1 Shader";
            this.checkBox8.UseVisualStyleBackColor = true;
            this.checkBox8.CheckedChanged += new System.EventHandler(this.checkBox8_CheckedChanged);
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(175, 76);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(108, 17);
            this.checkBox7.TabIndex = 9;
            this.checkBox7.Text = "Force 1.4 Shader";
            this.checkBox7.UseVisualStyleBackColor = true;
            this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(175, 52);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(108, 17);
            this.checkBox6.TabIndex = 8;
            this.checkBox6.Text = "Force 2.0 Shader";
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(9, 148);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(109, 17);
            this.checkBox5.TabIndex = 7;
            this.checkBox5.Text = "Disable All Sound";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(9, 124);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(140, 17);
            this.checkBox4.TabIndex = 6;
            this.checkBox4.Text = "Disable Joystick support";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(9, 100);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(120, 17);
            this.checkBox3.TabIndex = 5;
            this.checkBox3.Text = "Disable Intro Videos";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(9, 76);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(107, 17);
            this.checkBox2.TabIndex = 4;
            this.checkBox2.Text = "Windowed Mode";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 52);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(64, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Console";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Arguments";
            // 
            // Arguments
            // 
            this.Arguments.Location = new System.Drawing.Point(9, 23);
            this.Arguments.Name = "Arguments";
            this.Arguments.Size = new System.Drawing.Size(382, 20);
            this.Arguments.TabIndex = 1;
            // 
            // BigBoy
            // 
            this.BigBoy.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BigBoy.Image = global::HAC2Beta2.Properties.Resources.MasterChiefIcon;
            this.BigBoy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BigBoy.Location = new System.Drawing.Point(407, 23);
            this.BigBoy.Name = "BigBoy";
            this.BigBoy.Size = new System.Drawing.Size(154, 64);
            this.BigBoy.TabIndex = 0;
            this.BigBoy.Text = "Start Halo";
            this.BigBoy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BigBoy.UseVisualStyleBackColor = true;
            this.BigBoy.Click += new System.EventHandler(this.RunHACBitch);
            // 
            // ServersPage
            // 
            this.ServersPage.Controls.Add(this.filtering);
            this.ServersPage.Controls.Add(this.ServerBrowserList);
            this.ServersPage.Controls.Add(this.groupBox2);
            this.ServersPage.Location = new System.Drawing.Point(4, 22);
            this.ServersPage.Name = "ServersPage";
            this.ServersPage.Padding = new System.Windows.Forms.Padding(3);
            this.ServersPage.Size = new System.Drawing.Size(756, 408);
            this.ServersPage.TabIndex = 1;
            this.ServersPage.Text = "Servers";
            this.ServersPage.UseVisualStyleBackColor = true;
            // 
            // filtering
            // 
            this.filtering.Controls.Add(this.MapCombo);
            this.filtering.Controls.Add(this.ResetButton);
            this.filtering.Controls.Add(this.SearchButton);
            this.filtering.Controls.Add(this.SearchTextBox);
            this.filtering.Controls.Add(this.checkBox13);
            this.filtering.Controls.Add(this.checkBox12);
            this.filtering.Controls.Add(this.checkBox11);
            this.filtering.Controls.Add(this.GametypeCombo);
            this.filtering.Controls.Add(this.VersionCombo);
            this.filtering.Location = new System.Drawing.Point(622, 10);
            this.filtering.Name = "filtering";
            this.filtering.Size = new System.Drawing.Size(126, 253);
            this.filtering.TabIndex = 3;
            this.filtering.TabStop = false;
            this.filtering.Text = "Filters";
            // 
            // MapCombo
            // 
            this.MapCombo.FormattingEnabled = true;
            this.MapCombo.Items.AddRange(new object[] {
            "-ANY Map-",
            "beavercreek",
            "bloodgulch",
            "boardingaction",
            "carousel",
            "chillout",
            "damnation",
            "dangercanyon",
            "deathisland",
            "gephyrophobia",
            "hangemhigh",
            "icefields",
            "infinity",
            "longest",
            "prisoner",
            "putput",
            "sidewinder",
            "timberland",
            "wizard"});
            this.MapCombo.Location = new System.Drawing.Point(6, 75);
            this.MapCombo.Name = "MapCombo";
            this.MapCombo.Size = new System.Drawing.Size(110, 21);
            this.MapCombo.TabIndex = 8;
            this.MapCombo.Text = "Map";
            this.MapCombo.SelectedIndexChanged += new System.EventHandler(this.MapCombo_changeIndex);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(7, 224);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(113, 23);
            this.ResetButton.TabIndex = 7;
            this.ResetButton.Text = "Reset Search";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetSearch_Click);
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(7, 194);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(113, 23);
            this.SearchButton.TabIndex = 6;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(7, 171);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(113, 20);
            this.SearchTextBox.TabIndex = 5;
            // 
            // checkBox13
            // 
            this.checkBox13.AutoSize = true;
            this.checkBox13.Location = new System.Drawing.Point(6, 148);
            this.checkBox13.Name = "checkBox13";
            this.checkBox13.Size = new System.Drawing.Size(90, 17);
            this.checkBox13.TabIndex = 4;
            this.checkBox13.Text = "HAC Enabled";
            this.checkBox13.UseVisualStyleBackColor = true;
            // 
            // checkBox12
            // 
            this.checkBox12.AutoSize = true;
            this.checkBox12.Location = new System.Drawing.Point(6, 125);
            this.checkBox12.Name = "checkBox12";
            this.checkBox12.Size = new System.Drawing.Size(111, 17);
            this.checkBox12.TabIndex = 3;
            this.checkBox12.Text = "No Empty Servers";
            this.checkBox12.UseVisualStyleBackColor = true;
            // 
            // checkBox11
            // 
            this.checkBox11.AutoSize = true;
            this.checkBox11.Location = new System.Drawing.Point(6, 102);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.Size = new System.Drawing.Size(98, 17);
            this.checkBox11.TabIndex = 2;
            this.checkBox11.Text = "No Full Servers";
            this.checkBox11.UseVisualStyleBackColor = true;
            // 
            // GametypeCombo
            // 
            this.GametypeCombo.FormattingEnabled = true;
            this.GametypeCombo.Items.AddRange(new object[] {
            "-ANY Gametype-",
            "CTF",
            "Slayer",
            "Oddball",
            "King",
            "Race"});
            this.GametypeCombo.Location = new System.Drawing.Point(7, 48);
            this.GametypeCombo.Name = "GametypeCombo";
            this.GametypeCombo.Size = new System.Drawing.Size(113, 21);
            this.GametypeCombo.TabIndex = 1;
            this.GametypeCombo.Text = "Gametype";
            this.GametypeCombo.SelectedIndexChanged += new System.EventHandler(this.GametypeCombo_changeIndex);
            // 
            // VersionCombo
            // 
            this.VersionCombo.FormattingEnabled = true;
            this.VersionCombo.Items.AddRange(new object[] {
            "-Any Version-",
            "1.04",
            "1.08",
            "1.09"});
            this.VersionCombo.Location = new System.Drawing.Point(7, 20);
            this.VersionCombo.Name = "VersionCombo";
            this.VersionCombo.Size = new System.Drawing.Size(113, 21);
            this.VersionCombo.TabIndex = 0;
            this.VersionCombo.Text = "Version";
            this.VersionCombo.SelectedIndexChanged += new System.EventHandler(this.VersionCombo_changeIndex);
            // 
            // ServerBrowserList
            // 
            this.ServerBrowserList.AllColumns.Add(this.olvColumn1);
            this.ServerBrowserList.AllColumns.Add(this.olvColumn2);
            this.ServerBrowserList.AllColumns.Add(this.olvColumn3);
            this.ServerBrowserList.AllColumns.Add(this.olvColumn4);
            this.ServerBrowserList.AllColumns.Add(this.olvColumn5);
            this.ServerBrowserList.AllColumns.Add(this.olvColumn6);
            this.ServerBrowserList.AlternateRowBackColor = System.Drawing.Color.Gainsboro;
            this.ServerBrowserList.BackColor = System.Drawing.Color.White;
            this.ServerBrowserList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ServerBrowserList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2,
            this.olvColumn3,
            this.olvColumn4,
            this.olvColumn5,
            this.olvColumn6});
            this.ServerBrowserList.FullRowSelect = true;
            this.ServerBrowserList.GridLines = true;
            this.ServerBrowserList.HighlightBackgroundColor = System.Drawing.Color.Black;
            this.ServerBrowserList.HighlightForegroundColor = System.Drawing.Color.Maroon;
            this.ServerBrowserList.Location = new System.Drawing.Point(9, 10);
            this.ServerBrowserList.MultiSelect = false;
            this.ServerBrowserList.Name = "ServerBrowserList";
            this.ServerBrowserList.SelectAllOnControlA = false;
            this.ServerBrowserList.SelectedColumnTint = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.ServerBrowserList.ShowGroups = false;
            this.ServerBrowserList.Size = new System.Drawing.Size(607, 253);
            this.ServerBrowserList.SortGroupItemsByPrimaryColumn = false;
            this.ServerBrowserList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ServerBrowserList.TabIndex = 2;
            this.ServerBrowserList.TintSortColumn = true;
            this.ServerBrowserList.UnfocusedHighlightBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.ServerBrowserList.UnfocusedHighlightForegroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ServerBrowserList.UseAlternatingBackColors = true;
            this.ServerBrowserList.UseCompatibleStateImageBehavior = false;
            this.ServerBrowserList.UseFiltering = true;
            this.ServerBrowserList.UseTranslucentSelection = true;
            this.ServerBrowserList.View = System.Windows.Forms.View.Details;
            this.ServerBrowserList.SelectedIndexChanged += new System.EventHandler(this.ServerBrowser_SelectServer);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "addr";
            this.olvColumn1.Text = "IPAddress";
            this.olvColumn1.Width = 0;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "pass";
            this.olvColumn2.Text = " ";
            this.olvColumn2.Width = 24;
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "Name";
            this.olvColumn3.Text = "Name";
            this.olvColumn3.Width = 220;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "Map";
            this.olvColumn4.Text = "Map";
            this.olvColumn4.Width = 90;
            // 
            // olvColumn5
            // 
            this.olvColumn5.AspectName = "AspectPlayers";
            this.olvColumn5.Text = "Players";
            // 
            // olvColumn6
            // 
            this.olvColumn6.AspectName = "AspectGametype";
            this.olvColumn6.Text = "Gametype";
            this.olvColumn6.Width = 120;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.GametypePicture);
            this.groupBox2.Controls.Add(this.MapPicture);
            this.groupBox2.Controls.Add(this.GamenameTxt);
            this.groupBox2.Controls.Add(this.GameNameDetail);
            this.groupBox2.Controls.Add(this.VersionTxt);
            this.groupBox2.Controls.Add(this.VersionDetail);
            this.groupBox2.Controls.Add(this.PlayersTxt);
            this.groupBox2.Controls.Add(this.NameTxt);
            this.groupBox2.Controls.Add(this.PlayersDetail);
            this.groupBox2.Controls.Add(this.NameDetail);
            this.groupBox2.Location = new System.Drawing.Point(7, 269);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(732, 133);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Server Details";
            // 
            // GametypePicture
            // 
            this.GametypePicture.Location = new System.Drawing.Point(187, 20);
            this.GametypePicture.Name = "GametypePicture";
            this.GametypePicture.Size = new System.Drawing.Size(50, 50);
            this.GametypePicture.TabIndex = 13;
            this.GametypePicture.TabStop = false;
            // 
            // MapPicture
            // 
            this.MapPicture.Location = new System.Drawing.Point(10, 20);
            this.MapPicture.Name = "MapPicture";
            this.MapPicture.Size = new System.Drawing.Size(170, 102);
            this.MapPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.MapPicture.TabIndex = 12;
            this.MapPicture.TabStop = false;
            // 
            // GamenameTxt
            // 
            this.GamenameTxt.AutoSize = true;
            this.GamenameTxt.Location = new System.Drawing.Point(310, 86);
            this.GamenameTxt.Name = "GamenameTxt";
            this.GamenameTxt.Size = new System.Drawing.Size(10, 13);
            this.GamenameTxt.TabIndex = 11;
            this.GamenameTxt.Text = " ";
            // 
            // GameNameDetail
            // 
            this.GameNameDetail.AutoSize = true;
            this.GameNameDetail.Location = new System.Drawing.Point(243, 86);
            this.GameNameDetail.Name = "GameNameDetail";
            this.GameNameDetail.Size = new System.Drawing.Size(55, 13);
            this.GameNameDetail.TabIndex = 10;
            this.GameNameDetail.Text = "Gametype";
            this.GameNameDetail.Visible = false;
            // 
            // VersionTxt
            // 
            this.VersionTxt.AutoSize = true;
            this.VersionTxt.Location = new System.Drawing.Point(310, 64);
            this.VersionTxt.Name = "VersionTxt";
            this.VersionTxt.Size = new System.Drawing.Size(10, 13);
            this.VersionTxt.TabIndex = 9;
            this.VersionTxt.Text = " ";
            // 
            // VersionDetail
            // 
            this.VersionDetail.AutoSize = true;
            this.VersionDetail.Location = new System.Drawing.Point(243, 64);
            this.VersionDetail.Name = "VersionDetail";
            this.VersionDetail.Size = new System.Drawing.Size(42, 13);
            this.VersionDetail.TabIndex = 8;
            this.VersionDetail.Text = "Version";
            this.VersionDetail.Visible = false;
            // 
            // PlayersTxt
            // 
            this.PlayersTxt.AutoSize = true;
            this.PlayersTxt.Location = new System.Drawing.Point(310, 42);
            this.PlayersTxt.Name = "PlayersTxt";
            this.PlayersTxt.Size = new System.Drawing.Size(10, 13);
            this.PlayersTxt.TabIndex = 7;
            this.PlayersTxt.Text = " ";
            // 
            // NameTxt
            // 
            this.NameTxt.AutoSize = true;
            this.NameTxt.Location = new System.Drawing.Point(310, 20);
            this.NameTxt.Name = "NameTxt";
            this.NameTxt.Size = new System.Drawing.Size(10, 13);
            this.NameTxt.TabIndex = 4;
            this.NameTxt.Text = " ";
            // 
            // PlayersDetail
            // 
            this.PlayersDetail.AutoSize = true;
            this.PlayersDetail.Location = new System.Drawing.Point(243, 42);
            this.PlayersDetail.Name = "PlayersDetail";
            this.PlayersDetail.Size = new System.Drawing.Size(41, 13);
            this.PlayersDetail.TabIndex = 2;
            this.PlayersDetail.Text = "Players";
            this.PlayersDetail.Visible = false;
            // 
            // NameDetail
            // 
            this.NameDetail.AutoSize = true;
            this.NameDetail.Location = new System.Drawing.Point(243, 20);
            this.NameDetail.Name = "NameDetail";
            this.NameDetail.Size = new System.Drawing.Size(35, 13);
            this.NameDetail.TabIndex = 0;
            this.NameDetail.Text = "Name";
            this.NameDetail.Visible = false;
            // 
            // ChatPage
            // 
            this.ChatPage.Location = new System.Drawing.Point(4, 22);
            this.ChatPage.Name = "ChatPage";
            this.ChatPage.Padding = new System.Windows.Forms.Padding(3);
            this.ChatPage.Size = new System.Drawing.Size(756, 408);
            this.ChatPage.TabIndex = 2;
            this.ChatPage.Text = "Chat";
            this.ChatPage.UseVisualStyleBackColor = true;
            // 
            // ServerCount
            // 
            this.ServerCount.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerCount.Location = new System.Drawing.Point(721, 51);
            this.ServerCount.Name = "ServerCount";
            this.ServerCount.Size = new System.Drawing.Size(47, 23);
            this.ServerCount.TabIndex = 10;
            this.ServerCount.Text = "0";
            this.ServerCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ServersLbl
            // 
            this.ServersLbl.AutoSize = true;
            this.ServersLbl.Location = new System.Drawing.Point(678, 55);
            this.ServersLbl.Name = "ServersLbl";
            this.ServersLbl.Size = new System.Drawing.Size(43, 13);
            this.ServersLbl.TabIndex = 22;
            this.ServersLbl.Text = "Servers";
            // 
            // PlayersLbl
            // 
            this.PlayersLbl.AutoSize = true;
            this.PlayersLbl.Location = new System.Drawing.Point(587, 55);
            this.PlayersLbl.Name = "PlayersLbl";
            this.PlayersLbl.Size = new System.Drawing.Size(41, 13);
            this.PlayersLbl.TabIndex = 23;
            this.PlayersLbl.Text = "Players";
            // 
            // PlayerCount
            // 
            this.PlayerCount.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayerCount.Location = new System.Drawing.Point(629, 51);
            this.PlayerCount.Name = "PlayerCount";
            this.PlayerCount.Size = new System.Drawing.Size(43, 23);
            this.PlayerCount.TabIndex = 24;
            this.PlayerCount.Text = "0";
            this.PlayerCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WarningLabel
            // 
            this.WarningLabel.AutoSize = true;
            this.WarningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WarningLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.WarningLabel.Location = new System.Drawing.Point(317, 56);
            this.WarningLabel.Name = "WarningLabel";
            this.WarningLabel.Size = new System.Drawing.Size(191, 15);
            this.WarningLabel.TabIndex = 25;
            this.WarningLabel.Text = "Data collection in progress...";
            // 
            // HACMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::HAC2Beta2.Properties.Resources.HACMain_FormBG;
            this.ClientSize = new System.Drawing.Size(781, 501);
            this.Controls.Add(this.WarningLabel);
            this.Controls.Add(this.PlayerCount);
            this.Controls.Add(this.PlayersLbl);
            this.Controls.Add(this.ServersLbl);
            this.Controls.Add(this.ServerCount);
            this.Controls.Add(this.MenuHAC);
            this.Controls.Add(this.MenuChat);
            this.Controls.Add(this.MenuServers);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.WindowTitle);
            this.Controls.Add(this.Tabbings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HACMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Halo Anticheat";
            this.Load += new System.EventHandler(this.HACMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MenuHAC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MenuServers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MenuChat)).EndInit();
            this.Tabbings.ResumeLayout(false);
            this.HACPage.ResumeLayout(false);
            this.HACPage.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ServersPage.ResumeLayout(false);
            this.filtering.ResumeLayout(false);
            this.filtering.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ServerBrowserList)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GametypePicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox CloseButton;
        private System.Windows.Forms.Label WindowTitle;
        private System.Windows.Forms.PictureBox MenuHAC;
        private System.Windows.Forms.PictureBox MenuServers;
        private System.Windows.Forms.PictureBox MenuChat;
        private System.Windows.Forms.TabControl Tabbings;
        private System.Windows.Forms.TabPage HACPage;
        private System.Windows.Forms.TabPage ChatPage;
        private System.Windows.Forms.Button BigBoy;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox ServerPass;
        private System.Windows.Forms.TextBox ServerIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox10;
        private System.Windows.Forms.CheckBox checkBox9;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Arguments;
        private System.Windows.Forms.Label ping;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage ServersPage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label PlayersTxt;
        private System.Windows.Forms.Label NameTxt;
        private System.Windows.Forms.Label PlayersDetail;
        private System.Windows.Forms.Label NameDetail;
        private System.Windows.Forms.Label VersionTxt;
        private System.Windows.Forms.Label VersionDetail;
        private System.Windows.Forms.Label ServerCount;
        private System.Windows.Forms.Label ServersLbl;
        private System.Windows.Forms.Label PlayersLbl;
        private System.Windows.Forms.Label PlayerCount;
        private System.Windows.Forms.Label WarningLabel;
        private System.Windows.Forms.Label GamenameTxt;
        private System.Windows.Forms.Label GameNameDetail;
        private System.Windows.Forms.PictureBox MapPicture;
        private System.Windows.Forms.PictureBox GametypePicture;
        private ObjectListView ServerBrowserList;
        private OLVColumn olvColumn1;
        private OLVColumn olvColumn2;
        private OLVColumn olvColumn3;
        private OLVColumn olvColumn4;
        private OLVColumn olvColumn5;
        private OLVColumn olvColumn6;
        private System.Windows.Forms.GroupBox filtering;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.TextBox SearchTextBox;
        private System.Windows.Forms.CheckBox checkBox13;
        private System.Windows.Forms.CheckBox checkBox12;
        private System.Windows.Forms.CheckBox checkBox11;
        private System.Windows.Forms.ComboBox GametypeCombo;
        private System.Windows.Forms.ComboBox VersionCombo;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.ComboBox MapCombo;
    }
}