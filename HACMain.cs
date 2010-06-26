/* 
 * Halo Anticheat 2
 * ================
 * Name: HACMain.cs
 * Description: Main screen for HAC - functionality includes Server browsing, chat, buddy list and halo launcher
 *
 * Copyright 2010 (C) Souless Productions
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Drawing.Imaging;

namespace HAC2Beta2
{
    public partial class HACMain : Form
    {
        #region CuteStuff Here
        // Collection of filters to filter out server browser
        private Hashtable Filters = new Hashtable();

        // Collection of map pictures for server browsing
        private Hashtable MapPictures = new Hashtable();

        // Collection of gametype pictures for server browsing
        private Hashtable GTPictures = new Hashtable();

        // Collection of servers - filled up by Gamespy master list and pushed to ServerBrowserList
        private Hashtable Servers = new Hashtable();

        // Boolean to hold frozen server updates
        private Boolean midProcess = false;

        // Selected Index of ServerBrowserList
        private int selNdx;

        // Current Tab
        private int currentTab = 0;

        // Previously ping'd IP - to prevent spam
        private String oldPingedIP = "";

        // Total player count - acquired from querying servers in the master list
        private int playercounter = 0;

        // The masterlist - an array of IPs
        private System.Net.IPEndPoint[] masterlist;

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

        public HACMain()
        {
            // Cute trick to change progress / display status from across threads
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        #region Form Events
        private void HACMain_Load(object sender, EventArgs e)
        {
            // Load the MapPictures Collection with resourced images
            MapPictures.Add("beavercreek",Properties.Resources.HACMain_MapBattlecreek);
            MapPictures.Add("bloodgulch", Properties.Resources.HACMain_MapBloodgulch);
            MapPictures.Add("boardingaction", Properties.Resources.HACMain_MapBoardingaction);
            MapPictures.Add("chillout", Properties.Resources.HACMain_MapChillout);
            MapPictures.Add("putput", Properties.Resources.HACMain_MapChiron);
            MapPictures.Add("damnation", Properties.Resources.HACMain_MapDamnation);
            MapPictures.Add("dangercanyon", Properties.Resources.HACMain_MapDangercanyon);
            MapPictures.Add("deathisland", Properties.Resources.HACMain_MapDeathisland);
            MapPictures.Add("carousel", Properties.Resources.HACMain_MapDerelict);
            MapPictures.Add("gephyrophobia", Properties.Resources.HACMain_MapGephyrophobia);
            MapPictures.Add("hangemhigh", Properties.Resources.HACMain_MapHangemhigh);
            MapPictures.Add("icefields", Properties.Resources.HACMain_MapIceFields);
            MapPictures.Add("infinity", Properties.Resources.HACMain_MapInfinity);
            MapPictures.Add("longest", Properties.Resources.HACMain_MapLongest);
            MapPictures.Add("prisoner", Properties.Resources.HACMain_MapPrisoner);
            MapPictures.Add("ratrace", Properties.Resources.HACMain_MapRatrace);
            MapPictures.Add("sidewinder", Properties.Resources.HACMain_MapSidewinder);
            MapPictures.Add("timberland", Properties.Resources.HACMain_MapTimberland);
            MapPictures.Add("wizard", Properties.Resources.HACMain_MapWizard);

            // Load the GTPictures Collection with resourced images
            GTPictures.Add("ctf", Properties.Resources.HACMain_GametypeCTF);
            GTPictures.Add("king", Properties.Resources.HACMain_GametypeKing);
            GTPictures.Add("race", Properties.Resources.HACMain_GametypeRace);
            GTPictures.Add("slayer", Properties.Resources.HACMain_GametypeSlayer);
            GTPictures.Add("oddball", Properties.Resources.HACMain_GametypeOddball);

            WindowTitle.Text = "   " + Text;

            // Initialize ServerBrowserList in new thread
            Thread t = new Thread(new ThreadStart(LoadServers));
            t.Start();

            WarningLabel.Text = "Contacting Gamespy for Master List...";
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

        /// <summary>
        /// *Enter Methods to highlight button, *Leave to unhighlight, *Click to select and change pager
        /// </summary>
        #region HAC Menu Tab Methods
        private void MenuHAC_MouseEnter(object sender, EventArgs e)
        {
            if (currentTab != 0)
                MenuHAC.Image = Properties.Resources.HACMain_MenuHome;
        }

        private void MenuServers_MouseEnter(object sender, EventArgs e)
        {
            if (currentTab != 1)
                MenuServers.Image = Properties.Resources.HACMain_MenuServers;
        }

        private void MenuChat_MouseEnter(object sender, EventArgs e)
        {
            if (currentTab != 2)
                MenuChat.Image = Properties.Resources.HACMain_MenuChat;
        }

        private void MenuChat_MouseLeave(object sender, EventArgs e)
        {
            if(currentTab != 2)
                MenuChat.Image = Properties.Resources.HACMain_MenuChatI;
        }

        private void MenuServers_MouseLeave(object sender, EventArgs e)
        {
            if (currentTab != 1)
                MenuServers.Image = Properties.Resources.HACMain_MenuServersI;
        }

        private void MenuHAC_MouseLeave(object sender, EventArgs e)
        {
            if (currentTab != 0)
                MenuHAC.Image = Properties.Resources.HACMain_MenuHomeI;
        }

        private void MenuHAC_Click(object sender, EventArgs e)
        {
            currentTab = 0;
            MenuHAC.Image = Properties.Resources.HACMain_MenuHomeH;
            MenuServers.Image = Properties.Resources.HACMain_MenuServersI;
            MenuChat.Image = Properties.Resources.HACMain_MenuChatI;
            Tabbings.SelectTab(0);
            WindowTitle.Text = Text;
        }

        private void MenuServers_Click(object sender, EventArgs e)
        {
            currentTab = 1;
            MenuHAC.Image = Properties.Resources.HACMain_MenuHomeI;
            MenuServers.Image = Properties.Resources.HACMain_MenuServersH;
            MenuChat.Image = Properties.Resources.HACMain_MenuChatI;
            Tabbings.SelectTab(1);
            WindowTitle.Text = Text + " - Servers";
        }
        
        private void MenuChat_Click(object sender, EventArgs e)
        {
            currentTab = 2;
            MenuHAC.Image = Properties.Resources.HACMain_MenuHomeI;
            MenuServers.Image = Properties.Resources.HACMain_MenuServersI;
            MenuChat.Image = Properties.Resources.HACMain_MenuChatH;
            Tabbings.SelectTab(2);
            WindowTitle.Text = Text + " - Chat";
        }

        #endregion

        #region Halo Arguments/Checkbox Handling
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            HaloArguments("-console", sender);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            HaloArguments("-window", sender);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            HaloArguments("-novideo", sender);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            HaloArguments("-nojoystick", sender);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            HaloArguments("-nosound", sender);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            HaloArguments("-use20", sender);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            HaloArguments("-use14", sender);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            HaloArguments("-use11", sender);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            HaloArguments("-useff", sender);
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            HaloArguments("-safemode", sender);
        }
       
        #endregion

        private void RunHACBitch(object sender, EventArgs e)
        {
            // Implement HAC Launch + hooks here
        }

        private void ServerIP_Leave(object sender, EventArgs e)
        {
            // Ping server
            if (ServerIP.Text != "" && ServerIP.Text != oldPingedIP)
            {
                // Don't spam pings, kthx
                oldPingedIP = ServerIP.Text;
                try
                {
                    Ping pingSender = new Ping();
                    PingOptions options = new PingOptions();
                    options.DontFragment = true;
                    string data = "12345678901234567890123456789012";
                    byte[] buffer = Encoding.ASCII.GetBytes(data);
                    PingReply reply = pingSender.Send(ServerIP.Text.Split(':')[0], 5, buffer, options);
                    if (reply.Status == IPStatus.Success)
                    {
                        ping.Text = reply.RoundtripTime+"";
                    }
                    else
                    {

                    }
                }
                catch
                {

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Arguments.Text = "";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox9.Checked = false;
            checkBox10.Checked = false;
        }

        private void ServerBrowser_SelectServer(object sender, MouseEventArgs e)
        {
            // If we're in the middle of updating a server's information, gtfo
            if (midProcess) return;

            // If there are no servers in list, gtfo
            if (ServerBrowserList.SelectedIndices.Count < 1) return;

            // If they select the same damn server over and over again, gtfo
            if (ServerBrowserList.SelectedIndices[0] == selNdx) return;

            // Show the cute details group
            NameDetail.Visible = true;
            PlayersDetail.Visible = true;
            GameNameDetail.Visible = true;
            VersionDetail.Visible = true;
            selNdx = ServerBrowserList.SelectedIndices[0]; // Update current index
            midProcess = true; // Turn on midprocess flag

            // Update Server Information in new thread
            Thread t = new Thread(new ThreadStart(UpdateServer));
            t.Start();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            // Clear Search Filter
            Filters.Remove("Custom");
            if (SearchTextBox.Text == "")
            {
                MessageBox.Show("Search for something!");
                SearchTextBox.Focus();
                return;
            }
            else
            {
                // Add search Filter - and show message if query is empty
                Filters.Add("Custom", SearchTextBox.Text);
                
                //ServerBrowserList.ModelFilter = new TextMatchFilter(ServerBrowserList, SearchTextBox.Text);
            }
            // Update the List
            UpdateFilters();
        }

        private void VersionCombo_changeIndex(object sender, EventArgs e)
        {
            // Clear Version Filter
            Filters.Remove("Version");
            if (VersionCombo.SelectedIndex != 0)
            {
                // Add Version Filter - as long as "ANY Version" is not selected
                switch (VersionCombo.SelectedItem.ToString())
                {
                    case "1.01":
                        Filters.Add("Version", "01.00.01.0580");
                        break;
                    case "1.02":
                        Filters.Add("Version", "01.00.02.0581");
                        break;
                    case "1.03":
                        Filters.Add("Version", "01.00.03.0605");
                        break;
                    case "1.04":
                        Filters.Add("Version", "01.00.04.0607");
                        break;
                    case "1.05":
                        Filters.Add("Version", "01.00.05.0610");
                        break;
                    case "1.06":
                        Filters.Add("Version", "01.00.06.0612");
                        break;
                    case "1.07":
                        Filters.Add("Version", "01.00.07.0613");
                        break;
                    case "1.08":
                        Filters.Add("Version", "01.00.08.0616");
                        break;
                    case "1.09":
                        Filters.Add("Version", "01.00.09.0620");
                        break;
                    default:
                        break;
                }
            }
            // Update the List
            UpdateFilters();
        }

        private void GametypeCombo_changeIndex(object sender, EventArgs e)
        {
            // Clear Gametype Filter
            Filters.Remove("Gametype");
            if (GametypeCombo.SelectedIndex != 0)
            {
                // Add Gametype Filter - as long as "ANY Gametype" is not selected
                Filters.Add("Gametype", GametypeCombo.SelectedText);
            }
            // Update the List
            UpdateFilters();
        }

        private void MapCombo_changeIndex(object sender, EventArgs e)
        {
            // Clear Map Filter
            Filters.Remove("Map");
            if (MapCombo.SelectedIndex != 0)
            {
                // Add Map Filter - as long as "ANY Map" is not selected
                Filters.Add("Map", MapCombo.SelectedText);
            }
            // Update the List
            UpdateFilters();
        }

        private void ResetSearch_Click(object sender, EventArgs e)
        {
            ServerBrowserList.ModelFilter = null;
        }

        #endregion

        #region Beautiful Code
        private void LoadServers()
        {
            // Load the masterlist into our global variable
            masterlist = GameSpy.GetMasterServerList("halor", "e4Rd9J", GameSpy.EncType.Advanced2, "");

            // Bad workaround here - we constantly attempt to get masterlist on failure
            if (masterlist.Length < 0) { LoadServers(); return; }

            WarningLabel.Text = "Acquiring server list information...";

            // Threadpooling logic here - List of ManualResetEvents
            List<ManualResetEvent> events = new List<ManualResetEvent>();
            
            // Go through all the IP addresses in masterlist and add them to threadpool
            for (int i = 0; i < masterlist.Length; i++)
            {
                ThreadPoolObj obj = new ThreadPoolObj();
                obj.ObjectID = i;
                obj.signal = new ManualResetEvent(false);
                events.Add(obj.signal);

                WaitCallback callback = new WaitCallback(LoadServerData);

                // We queue the obj and c# takes care of the rest
                ThreadPool.QueueUserWorkItem(callback, obj);
            }

            // Let's wait until the threadpool is empty, then we can move on with this method
            WaitForAll(events.ToArray());
            WarningLabel.Text = "";
        }

        /// <summary>
        /// Modify arguments textbox with selected checkboxes
        /// </summary>
        /// <param name="command">Require command string to be changed from arguments</param>
        /// <param name="sender">Must be trigged by event, so sender must be forwarded to this method</param>
        private void HaloArguments(String command, object sender)
        {
            CheckBox obj = sender as CheckBox;
            Arguments.Text = (obj.Checked) ? Arguments.Text + " " + command : Arguments.Text.Replace(" " + command, "").Replace(command, "");
        }

        /// <summary>
        /// Souless powered Server Class Object
        /// </summary>
        private class Server
        {
            // Not gonna comment anything in here. too lazy. figure it out yourself.
            public string addr;
            public string pass;
            public string Name;
            public string Map;
            public string[][] Players;
            public string MaxPlayers;
            public string Gametype;
            public string GameName;
            public string Version;
            public string AspectPlayers;
            public string AspectGametype;

            public Server()
            {

            }

            public Server(string address, string data)
            {
                addr = address;
                string[] arr = data.Split('\0');
                pass = (data.Split('\0')[10] == "1") ? "P" : "";
                Name = ParseServerName(arr[2], 0);
                MaxPlayers = arr[8];
                Map = arr[12];
                Gametype = arr[22];
                GameName = arr[26];
                Version = arr[4];
                Players = new string[Convert.ToInt32(arr[20])][];
                int loc = 40;
                for (int i = 0; i < Convert.ToInt32(arr[20]); i++)
                {
                    Players[i] = new string[4];
                    Players[i][0] = arr[loc]; loc++;
                    Players[i][1] = arr[loc]; loc++;
                    Players[i][2] = arr[loc]; loc++;
                    Players[i][3] = arr[loc]; loc++;
                }
                AspectPlayers = (Players.Length < 10) ? "0" + Players.Length + "/" + MaxPlayers : Players.Length + "/" + MaxPlayers;
                AspectGametype = Gametype + " - " + GameName;
            }
            public string GetVar(string something)
            {
                switch (something.ToLower())
                {
                    case "name":
                        return Name;
                    case "pass":
                    case "password":
                        return pass;
                    case "addr":
                    case "address":
                    case "ipaddress":
                        return addr;
                    case "map":
                        return Map;
                    case "version":
                        return Version;
                    case "numplayer":
                    case "playercount":
                    case "players":
                        return Players.Length + "";
                    case "maxplayers":
                    case "playerlimit":
                        return MaxPlayers;
                    case "gametype":
                        return Gametype;
                    case "gamename":
                        return GameName;
                    default:
                        return "";
                }
            }

            public Boolean Find(string query)
            {
                if (Name.Contains(query)) return true;
                if (Map.Contains(query)) return true;
                if (Gametype.Contains(query)) return true;
                if (GameName.Contains(query)) return true;
                if (Version.Contains(query)) return true;
                for (int i = 0; i < Players.Length; i++)
                {
                    if (Players[i][0].Contains(query))
                        return true;
                }
                return false;
            }

            private string ParseServerName(string servername, int white)
            {
                string newname = "";
                for (int i = 0; i < servername.Length; i++)
                {
                    if ((byte)servername[i] > 31)
                    {
                        if (white == 1 && servername[i] != ' ')
                        {
                            newname += servername[i];
                        }
                        else
                        {
                            newname += servername[i];
                        }
                    }
                }
                return newname;
            }
        }

        // The almighty ThreadPoolObj class object
        private class ThreadPoolObj
        {
            public int ObjectID;
            public ManualResetEvent signal;
        }

        private bool WaitForAll(ManualResetEvent[] events)
        {
            bool result = false;
            try
            {
                if (events != null)
                {
                    for (int i = 0; i < events.Length; i++)
                    {
                        events[i].WaitOne(250);
                        PlayerCount.Text = playercounter + "";
                        ServerCount.Text = (i + 1) + "/" + events.Length;
                    }
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// A thread-pooled method that will query all servers in masterlist
        /// </summary>
        /// <param name="block">Must be a ThreadPoolObj class object</param>
        private void LoadServerData(object block)
        {
            try
            {
                ThreadPoolObj obj = block as ThreadPoolObj;
                System.Net.IPEndPoint ipaddress = masterlist[obj.ObjectID];
                UdpClient udp = new UdpClient();
                // This is the data you send to a halo server - the query string
                byte[] sendBytes4 = { 254, 253, 0, 119, 106, 157, 157, 255, 255, 255, 255 };

                // Send Data
                udp.Send(sendBytes4, sendBytes4.Length, ipaddress);

                // We don't wanna wait forever for a reply - so lets do this asynchronously
                var asyncResult = udp.BeginReceive(null, null);

                // Set TimeToLive for 250ms (you could say Aussie servers won't show up in US clients)
                asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(250));

                // Recieve data
                byte[] receiveBytes = udp.EndReceive(asyncResult, ref ipaddress);

                // Make it readable
                string returnData = Encoding.ASCII.GetString(receiveBytes, 0, receiveBytes.Length);

                // Data must have \0 byte, for parsing, and must be a public server (openplaying)
                if (returnData.IndexOf('\0') != -1 && returnData.Split('\0')[16] == "openplaying")
                {
                    // Counting players
                    playercounter += Convert.ToInt32(returnData.Split('\0')[20]);

                    // Lets make a temporary Server class object
                    Server temp = new Server(ipaddress.ToString(), returnData);

                    // Add the temp to Servers Collection
                    Servers.Add(ipaddress.ToString(), temp);

                    // Add the temp to ServerBrowserList as well
                    ServerBrowserList.AddObject( Servers[ipaddress.ToString()] );
                }
                udp.Close();
                // Important - so the WaitForAll can move on with life
                obj.signal.Set();
            }
            catch
            {

            }
        }

        /// <summary>
        /// Filters the ServerBrowserList to required filters
        /// </summary>
        private void UpdateFilters()
        {
            if (Filters.Count < 1)
            {
                // If all filters are removed, or not set, then clear filters (show everything)
                ServerBrowserList.ModelFilter = null;
            }
            else
            {
                // A delegated method that returns true/false, a custom filter we can write
                ServerBrowserList.ModelFilter = new ModelFilter(delegate(object x)
                {
                    foreach (string property in Filters.Keys)
                    {
                        // We have to do a special function for search queries
                        if (property == "Custom") return ((Server)x).Find(Filters["Custom"] as string);

                        // For normal filters (version, gametype, and map) we can check directly
                        if (((Server)x).GetVar(property) != Filters[property] as string) return false;
                    }
                    return true;
                });
            }
        }

        /// <summary>
        /// Updates Server information - must have a single selected index in ServerBrowserList
        /// </summary>
        private void UpdateServer()
        {
            if (ServerBrowserList.SelectedIndices.Count != 1) return;

            UdpClient udp = new UdpClient();

            // This is the data you send to a halo server - the query string
            byte[] sendBytes4 = { 254, 253, 0, 119, 106, 157, 157, 255, 255, 255, 255 };

            // Borrow the Gamespy's StringToEndPoint method and convert our address to an EndPoint
            System.Net.IPEndPoint ipaddress = GameSpy.StringToEndPoint(ServerBrowserList.Items[ServerBrowserList.SelectedIndices[0]].Text);

            // Send Data
            udp.Send(sendBytes4, sendBytes4.Length, ipaddress);

            // Recieve Data
            byte[] receiveBytes = udp.Receive(ref ipaddress);

            // Make it readable
            string returnData = Encoding.ASCII.GetString(receiveBytes, 0, receiveBytes.Length);

            // Data must have \0 byte, for parsing, and must be a public server (openplaying)
            if (returnData.IndexOf('\0') != -1 && returnData.Split('\0')[16] == "openplaying")
            {
                // Since there is no real "Update" method for Lists - we must remove and add them manually
                Server temp = new Server(ipaddress.ToString(), returnData);
                Servers[ipaddress.ToString()] = temp;
                ServerBrowserList.RemoveObject(Servers[ipaddress.ToString()]);
                ServerBrowserList.AddObject(temp);
                ServerBrowserList.SelectObject(temp);

                // Update server details
                MapPicture.Image = MapPictures[temp.Map.ToLower()] as Bitmap;
                GametypePicture.Image = GTPictures[temp.Gametype.ToLower()] as Bitmap;
                NameTxt.Text = temp.Name;
                GamenameTxt.Text = temp.Gametype + " - " + temp.GameName;
                PlayersTxt.Text = temp.Players.Length + "/" + temp.MaxPlayers;
                VersionTxt.Text = temp.Version;
                midProcess = false; // we're not busy anymore, so turn that flag off
            }
            udp.Close();
        }
        #endregion

    }
}
