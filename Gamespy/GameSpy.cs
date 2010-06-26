using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Windows.Forms;

namespace HAC2Beta2
{
    /// <summary>
    /// Adapted from code by Luigi Auriemma: http://aluigi.altervista.org/
    /// Validation code converted from C to PHP by Lithium.
    /// EncType2_Decoder converted from C to VB by Tres: http://www.nfbsp.com/
    /// Both of the above converted from PHP and VB to C# by FordGT90Concept.
    /// 
    /// EncType1 not supported.
    /// </summary>
    public class GameSpy
    {
        // Since we aren't supporting enctype1, this is not needed...
        //private static readonly byte[] EncType1_Data = { 1, 186, 250, 178, 81, 0, 84, 128, 117, 22, 142, 142, 2, 8, 54, 165, 45, 5, 13, 22, 82, 7, 180, 34, 140, 233, 9, 214, 185, 38, 0, 4, 6, 5, 0, 19, 24, 196, 30, 91, 29, 118, 116, 252, 80, 81, 6, 22, 0, 81, 40, 0, 4, 10, 41, 120, 81, 0, 1, 17, 82, 22, 6, 74, 32, 132, 1, 162, 30, 22, 71, 22, 50, 81, 154, 196, 3, 42, 115, 225, 45, 79, 24, 75, 147, 76, 15, 57, 10, 0, 4, 192, 18, 12, 154, 94, 2, 179, 24, 184, 7, 12, 205, 33, 5, 192, 169, 65, 67, 4, 60, 82, 117, 236, 152, 128, 29, 8, 2, 29, 88, 132, 1, 78, 59, 106, 83, 122, 85, 86, 87, 30, 127, 236, 184, 173, 0, 112, 31, 130, 216, 252, 151, 139, 240, 131, 254, 14, 118, 3, 190, 57, 41, 119, 48, 224, 43, 255, 183, 158, 1, 4, 248, 1, 14, 232, 83, 255, 148, 12, 178, 69, 158, 10, 199, 6, 24, 1, 100, 176, 3, 152, 1, 235, 2, 176, 1, 180, 18, 73, 7, 31, 95, 94, 93, 160, 79, 91, 160, 90, 89, 88, 207, 82, 84, 208, 184, 52, 2, 252, 14, 66, 41, 184, 218, 0, 186, 177, 240, 18, 253, 35, 174, 182, 69, 169, 187, 6, 184, 136, 20, 36, 169, 0, 20, 203, 36, 18, 174, 204, 87, 86, 238, 253, 8, 48, 217, 253, 139, 62, 10, 132, 70, 250, 119, 184 };

        #region " The Good Stuff "
        // Converted from Tres's VB code with new FordGT90Concept optimizations...
        /// <summary>
        /// Takes the gamename, handoff, enctype, and handoff and returns a list of endpoints.
        /// </summary>
        /// <param name="gamename">The name of the game.  A relatively complete list of these can be found in C:\Program Files\GameSpy Arcade\Services\detection.cfg</param>
        /// <param name="handoff">The game's handoff.  This can be found via http://motd.gamespy.com/software/services/index.aspx?mode=full&services=GAMENAME</param>
        /// <param name="type">Recommended to use EncType.Advanced2--EncType.Basic is obsolete.</param>
        /// <param name="filter">A filter to apply.  It is the same as what you would enter into GameSpy Arcade filters.</param>
        /// <returns>An array of IPEndPoints.  Each IPEndPoint represents a hosting server.</returns>
        public static IPEndPoint[] GetMasterServerList(string gamename, string handoff, EncType type, string filter)
        {
            TcpClient tcp = new TcpClient("master.gamespy.com", 28900);
            StringBuilder sb = new StringBuilder();
            NetworkStream ns = tcp.GetStream();

            while (tcp.Available == 0)
                Thread.Sleep(10);

            while (tcp.Available > 0)
                sb.Append(Convert.ToChar(ns.ReadByte()));

            byte[] tosend = Encoding.ASCII.GetBytes("\\basic\\gamename\\" + gamename + "\\enctype\\" + (int)type + "\\location\\0\\where\\" + filter + "\\validate\\" + MakeValidate(GetSecureKey(sb.ToString().TrimEnd('\0')), handoff, type) + "\\final\\list\\\\gamename\\" + gamename + "\\final");
            ns.Write(tosend, 0, tosend.Length);

            List<byte> received = new List<byte>();

            while (tcp.Available == 0)
                Thread.Sleep(1500);

            while (tcp.Available > 0)
                received.Add(Convert.ToByte(ns.ReadByte()));

            ns.Close();
            tcp.Close();

            IPEndPoint[] addrs;

            switch (type)
            {
                case EncType.Advanced2:
                    string[] parts = DecodeAdvanced2(received.ToArray(), Encoding.ASCII.GetBytes(GetHandoff(handoff))).Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    IPEndPoint[] temp = new IPEndPoint[parts.Length / 2];
                    for (int i = 1; i < parts.Length; i += 2)
                    {
                        temp[i / 2] = StringToEndPoint(parts[i]);
                    }
                    addrs = temp;
                    break;
                default:
                    // Not supported
                    addrs = null;
                    break;
            }

            return addrs;
        }
        /// <summary>
        /// Takes an ip:port and returns an IPEndPoint.
        /// </summary>
        /// <param name="ipport">An ip:port to convert.</param>
        /// <returns>An IPEndPoint representing the given ip:port.</returns>
        public static IPEndPoint StringToEndPoint(string ipport)
        {
            // split ip:port
            string[] addrparts = ipport.Split(':');
            if (addrparts.Length < 2) { return new IPEndPoint(IPAddress.Parse("127.0.0.1"),2302); }
            // return an IPAddress
            return new IPEndPoint(IPAddress.Parse(addrparts[0]), Convert.ToInt32(addrparts[1]));
        }

        /// <summary>
        /// The supported enctypes.
        /// </summary>
        public enum EncType : byte
        {
            Basic = 0,
            //Advanced1 = 1,
            Advanced2 = 2
        }
        #endregion

        #region " Validation "
        // Converted from Lithium's PHP code...
        /// <summary>
        /// Creates an 8 byte validation code from the 6 byte secure key and 6 byte handoff.
        /// </summary>
        /// <param name="securekey">The 6 byte string received from the master server.</param>
        /// <param name="handoff">A 6 or more byte long game handoff.</param>
        /// <param name="type">The encryption type to process.</param>
        /// <returns>The 8 byte long validate string.</returns>
        private static string MakeValidate(string securekey, string handoff, EncType type)
        {
            byte[] table = new byte[256];                                   // Buffer
            byte[] secure = Encoding.ASCII.GetBytes(securekey);             // Secure Key
            byte[] hand = Encoding.ASCII.GetBytes(GetHandoff(handoff));
            int[] temp = new int[4];                                        // Some Temporary variables

            #region " Buffer with incremental data "
            for (short i = 0; i < 256; i++)
                table[i] = Convert.ToByte(i);
            #endregion

            #region " Scramble with key "
            for (short i = 0; i < 256; i++)
            {
                //Scramble the Table with our Handoff
                temp[0] = temp[0] + table[i] + hand[i % hand.Length] & 255;
                temp[2] = table[temp[0]];

                //Update the buffer
                table[temp[0]] = table[i];
                table[i] = Convert.ToByte(temp[2]);
            }
            #endregion

            #region " Scramble securekey with buffer "
            temp[0] = 0;
            byte[] key = new byte[6];
            for (byte i = 0; i < securekey.Length; i++)
            {
                //Add the next char to the array
                key[i] = secure[i];

                temp[0] = (temp[0] + key[i] + 1) & 255;
                temp[2] = table[temp[0]];

                temp[1] = (temp[1] + temp[2]) & 255;
                temp[3] = table[temp[1]];

                table[temp[1]] = Convert.ToByte(temp[2]);
                table[temp[0]] = Convert.ToByte(temp[3]);

                //XOR the Buffer
                key[i] = Convert.ToByte(key[i] ^ table[(temp[2] + temp[3]) & 255]);
            }
            #endregion

            #region " EncType management "
            switch (type)
            {
                /* NOT SUPPORTING
            case EncType.Inefficient:
                for (byte i = 0; i < securekey.Length; i++)
                    key[i] = EncType1_Data[key[i]];
                break;
                 */
                case EncType.Advanced2:
                    for (byte i = 0; i < securekey.Length; i++)
                        key[i] ^= hand[i % handoff.Length];
                    break;
            }
            #endregion

            #region " Create 8 byte long validate key "
            int length = Convert.ToInt32(secure.Length / 3);
            StringBuilder sb = new StringBuilder();
            byte j = 0;
            while (length >= 1)
            {
                length--;

                temp[2] = key[j];
                temp[3] = key[j + 1];

                sb.Append(AddChar(temp[2] >> 2));
                sb.Append(AddChar(((temp[2] & 3) << 4) | (temp[3] >> 4)));

                temp[2] = key[j + 2];

                sb.Append(AddChar(((temp[3] & 15) << 2) | (temp[2] >> 6)));
                sb.Append(AddChar(temp[2] & 63));

                j = Convert.ToByte(j + 3);
            }
            #endregion

            return sb.ToString();
        }
        /// <summary>
        /// Turns an integer into an ASCII char to be added to the validate code.
        /// </summary>
        /// <param name="value">A number to convert.</param>
        /// <returns>The character that represents 'value.'</returns>
        private static char AddChar(int value)
        {
            if (value < 26) return Convert.ToChar(value + 65);
            if (value < 52) return Convert.ToChar(value + 71);
            if (value < 62) return Convert.ToChar(value - 4);
            if (value == 62) return '+';
            if (value == 63) return '/';
            return Convert.ToChar(0);
        }
        /// <summary>
        /// Makes the handoff 6 bytes long.  If it is already 6 bytes long, it will return it.
        /// </summary>
        /// <param name="handoff">The handoff which is 6 bytes or longer.</param>
        /// <returns>The 6 byte version of the handoff.</returns>
        private static string GetHandoff(string handoff)
        {
            if (handoff.Length == 6)
                return handoff;
            else
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 2; i <= 13; i = i + 2)
                    sb.Append(handoff[i]);
                return sb.ToString();
            }
        }
        /// <summary>
        /// Extracts the secure key from the initial response from the master server.
        /// </summary>
        /// <param name="message">Appears something like "\basic\\secure\SECURE"</param>
        /// <returns>The 6 byte long secure key.</returns>
        private static string GetSecureKey(string message)
        {
            return message.Substring(message.Length - 6);
        }
        #endregion

        #region " Decode Advanced2 "
        // Converted from Tres's VB code...
        private static int p_ind;
        private static uint SumOverflow(uint a, uint b)
        {
            double res = a + b;
            return Convert.ToUInt32(res % 4294967296);
        }
        private static byte[] Shared1(uint[] tbuff, byte[] datap, int len)
        {
            p_ind = 309;
            int s_ind = 309;

            int datap_ind = 0;
            int lalind = 309;

            int bytepart = 4;
            byte[] ByteArray = new byte[4];
            while (len > 0)
            {
                if (datap_ind % 63 == 0)
                {
                    p_ind = s_ind;
                    lalind = 309;
                    bytepart = 4;
                    tbuff = Shared2(tbuff, 16);
                }

                if (bytepart > 3)
                {
                    uint t = tbuff[lalind];
                    ByteArray = BitConverter.GetBytes(t);
                    bytepart = 0;
                    lalind++;
                }
                datap[datap_ind] ^= Convert.ToByte(ByteArray[bytepart] % 256);
                datap_ind++;
                p_ind++;
                bytepart++;
                len--;
            }

            return datap;
        }
        private static uint[] Shared2(uint[] tbuff, int len)
        {
            uint t1, t2, t3, t4, t5;

            int old_p_ind = p_ind;
            t2 = tbuff[304];
            t1 = tbuff[305];
            t3 = tbuff[306];
            t5 = tbuff[307];
            int cnt = 0;
            for (int i = 0; i < len; i++)
            {
                p_ind = Convert.ToInt32(t2 + 272);
                while (t5 < 65536)
                {
                    t1 = SumOverflow(t1, t5);
                    p_ind++;
                    t3 = SumOverflow(t3, t1);
                    t1 = SumOverflow(t1, t3);

                    tbuff[p_ind - 17] = t1;
                    tbuff[p_ind - 1] = t3;
                    t4 = (t3 << 24) | (t3 >> 8);
                    tbuff[p_ind + 15] = t5;

                    t5 <<= 1;

                    t2++;

                    t1 ^= tbuff[t1 & 255];
                    t4 ^= tbuff[t4 & 255];

                    t3 = (t4 << 24) | (t4 >> 8);

                    t4 = (t1 >> 24) | (t1 << 8);
                    t4 ^= tbuff[t4 & 255];
                    t3 ^= tbuff[t3 & 255];

                    t1 = (t4 >> 24) | (t4 << 8);
                }
                t3 ^= t1;
                tbuff[old_p_ind + i] = t3;
                t2--;

                t1 = tbuff[t2 + 256];
                t5 = tbuff[t2 + 272];
                t1 = ~t1;

                t3 = (t1 << 24) | (t1 >> 8);

                t3 ^= tbuff[t3 & 255];
                t5 ^= tbuff[t5 & 255];
                t1 = (t3 << 24) | (t3 >> 8);

                t4 = (t5 >> 24) | (t5 << 8);

                t1 ^= tbuff[t1 & 255];
                t4 ^= tbuff[t4 & 255];

                t3 = (t4 >> 24) | (t4 << 8);

                t5 = (tbuff[t2 + 288] << 1) + 1;
                cnt++;
            }
            tbuff[304] = t2;
            tbuff[305] = t1;
            tbuff[306] = t3;
            tbuff[307] = t5;
            return tbuff;
        }
        private static uint[] Shared3(uint[] data, int n1, int n2)
        {
            uint t1, t2, t3, t4;
            int i;
            t2 = Convert.ToUInt32(n1);
            t1 = 0;
            t4 = 1;
            data[304] = 0;
            i = 32768;

            while (i != 0)
            {
                t2 = SumOverflow(t2, t4);
                t1 = SumOverflow(t1, t2);
                t2 = SumOverflow(t2, t1);
                if ((n2 & i) != 0)
                {
                    t2 = ~t2;
                    t4 = (t4 << 1) + 1;
                    t3 = (t2 << 24) | (t2 >> 8);
                    t3 ^= data[t3 & 255];
                    t1 ^= data[t1 & 255];
                    t2 = (t3 << 24) | (t3 >> 8);
                    t3 = (t1 >> 24) | (t1 << 8);
                    t2 ^= data[t2 & 255];
                    t3 ^= data[t3 & 255];
                    t1 = (t3 >> 24) | (t3 << 8);
                }
                else
                {
                    data[data[304] + 256] = t2;
                    data[data[304] + 272] = t1;
                    data[data[304] + 288] = t4;
                    data[304]++;
                    t3 = (t1 << 24) | (t1 >> 8);
                    t2 ^= data[t2 & 255];
                    t3 ^= data[t3 & 255];
                    t1 = (t3 << 24) | (t3 >> 8);
                    t3 = (t2 >> 24) | (t2 << 8);
                    t3 ^= data[t3 & 255];
                    t1 ^= data[t1 & 255];
                    t2 = (t3 >> 24) | (t3 << 8);
                    t4 <<= 1;
                }

                i >>= 1;
            }
            data[305] = t2;
            data[306] = t1;
            data[307] = t4;
            data[308] = Convert.ToUInt32(n1);
            return data;
        }
        private static uint[] Shared4(byte[] data, uint[] dest)
        {
            byte[] src = new byte[data.Length - 1];
            for (int i = 0; i < data.Length - 1; i++)
                src[i] = data[i + 1];
            byte pos, x, y;
            int size = data[0];

            for (short i = 0; i <= 255; i++)
                dest[i] = 0;
            for (y = 0; y <= 3; y++)
            {
                for (short i = 0; i <= 255; i++)
                    dest[i] = Convert.ToUInt32(((dest[i] << 8) + i) & 4294967295);

                pos = y;
                for (x = 0; x <= 1; x++)
                {
                    for (short i = 0; i <= 255; i++)
                    {
                        uint tmp = dest[i];
                        pos += Convert.ToByte((tmp + src[i % size]) & 255);

                        dest[i] = dest[pos];
                        dest[pos] = tmp;
                    }
                }
            }
            for (short i = 0; i <= 255; i++)
                dest[i] ^= Convert.ToUInt32(i);

            dest = Shared3(dest, 0, 0);

            return dest;
        }
        /// <summary>
        /// Decodes the information received from the master server.
        /// </summary>
        /// <param name="data">The information received from the master server.</param>
        /// <param name="handoff">The 6 byte long array represeting the handoff.</param>
        /// <returns>The decoded data as a string.</returns>
        private static string DecodeAdvanced2(byte[] data, byte[] handoff)
        {
            uint[] dest = new uint[326];
            for (int i = 256; i < dest.Length; i++)
                dest[i] = 0;
            data[0] ^= 236;
            for (int i = 0; i < handoff.Length; i++)
                data[i + 1] ^= handoff[i];
            dest = Shared4(data, dest);

            byte[] datap = new byte[data.Length - 1 - data[0]];

            for (int i = 0; i < data.Length - data[0] - 1; i++)
                datap[i] = data[data[0] + i + 1];
            if (datap.Length < 6)
                return Encoding.ASCII.GetString(data);

            datap = Shared1(dest, datap, datap.Length);

            return Encoding.ASCII.GetString(datap);
        }
        #endregion
    }
}
