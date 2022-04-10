using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Lidgren.Network;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace CommonTool
{
    public static class CD2Extension
    {
        public static void AppendTextAutoClear(this RichTextBox richTxtBox, string msg)
        {
            richTxtBox.AppendText(msg);
            if (richTxtBox.Lines.Length >= 100)
            {
                int totalLen = richTxtBox.Lines.Length;
                int copyLen = totalLen / 2;
                string[] str_arr = new string[totalLen - copyLen];

                Array.Copy(richTxtBox.Lines, copyLen, str_arr, 0, str_arr.Length);
                richTxtBox.Clear();

                foreach (string s in str_arr)
                    if (s != string.Empty)
                        richTxtBox.AppendText(s + Environment.NewLine);
            }
            richTxtBox.ScrollToCaret();
        }

        public static IPEndPoint ToIPEndPoint(this string endPoint)
        {
            string[] ep = endPoint.Split(':');
            if (ep.Length < 2) throw new FormatException("Invalid endpoint format");
            IPAddress ip;
            if (ep.Length > 2)
            {
                if (!IPAddress.TryParse(string.Join(":", ep, 0, ep.Length - 1), out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }
            else
            {
                if (!IPAddress.TryParse(ep[0], out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }
            int port;
            if (!int.TryParse(ep[ep.Length - 1], System.Globalization.NumberStyles.None, System.Globalization.NumberFormatInfo.CurrentInfo, out port))
            {
                throw new FormatException("Invalid port");
            }
            return new IPEndPoint(ip, port);
        }

        public static long ToTimeStamp(this DateTime dateTime)
        {
            return dateTime.Subtract(new DateTime(1970, 1, 1)).Ticks;
        }

        public static DateTime ToDateTime(this long timestamp)
        {
            return new DateTime(1970, 1, 1).Add(TimeSpan.FromTicks(timestamp));
        }

        public static string ToDateString(this long timestamp)
        {
            return DateTime.FromBinary(timestamp).ToShortDateString();
        }

        public static bool TryGetProtocol(this NetIncomingMessage msg, out BaseProtocol bp)
        {
            try
            {
                bool isDataProtocol = msg.ReadBoolean();

                if (isDataProtocol)
                {
                    DataProtocol tmp = new DataProtocol();

                    /*
                     * Packet Format: 30 <= Packet_Length <= MTU
                     *  0------7-----15-----------31
                     *  |Action| Type |     TTL    |
                     *  +--------------------------+
                     *  |            TS            |
                     *  |                          |
                     *  +--------------------------+
                     *  |            Src           |
                     *  |                          |
                     *  +--------------------------+
                     *  |            Des           |
                     *  |                          |
                     *  +--------------------------+
                     *  |   Callback (Byte Array)  |
                     *  +--------------------------+
                     *  |    Content (Byte Array)  |
                     *  +--------------------------+
                     */
                    tmp.Action = (DPAction)msg.ReadByte();
                    tmp.Type = (DPType)msg.ReadByte();
                    tmp.TTL = msg.ReadInt16();
                    tmp.TS = msg.ReadInt64();
                    tmp.Src = msg.ReadInt64();
                    tmp.Des = msg.ReadInt64();
                    tmp.Callback = msg.ReadString();
                    tmp.Content = msg.ReadString();

                    bp = tmp;

                    return true;
                }
                else
                {
                    FileProtocol tmp = new FileProtocol();
                    int length;

                    tmp.Length = msg.ReadUInt64();
                    tmp.Type = (FPType)msg.ReadByte();
                    tmp.Src = msg.ReadInt64();
                    tmp.Des = msg.ReadInt64();
                    tmp.File_ID = msg.ReadInt64();
                    length = msg.ReadInt32();
                    tmp.Data = msg.ReadBytes(length);

                    bp = tmp;

                    return true;
                }
            }
            catch
            {
                msg.Position = 0;
                bp = null;

                return false;
            }
        }

        //public static bool TryGetDataProtocol(this NetIncomingMessage msg, out BaseProtocol dp)
        //{
        //    try
        //    {
        //        DataProtocol tmp = new DataProtocol();

        //        /*
        //         * Packet Format: 30 <= Packet_Length <= MTU
        //         *  0------7-----15-----------31
        //         *  |Action| Type |     TTL    |
        //         *  +--------------------------+
        //         *  |            TS            |
        //         *  |                          |
        //         *  +--------------------------+
        //         *  |            Src           |
        //         *  |                          |
        //         *  +--------------------------+
        //         *  |            Des           |
        //         *  |                          |
        //         *  +--------------------------+
        //         *  |   Callback (Byte Array)  |
        //         *  +--------------------------+
        //         *  |    Content (Byte Array)  |
        //         *  +--------------------------+
        //         */
        //        tmp.Action = (DPAction)msg.ReadByte();
        //        tmp.Type = (DPType)msg.ReadByte();
        //        tmp.TTL = msg.ReadInt16();
        //        tmp.TS = msg.ReadInt64();
        //        tmp.Src = msg.ReadInt64();
        //        tmp.Des = msg.ReadInt64();
        //        tmp.Callback = msg.ReadString();
        //        tmp.Content = msg.ReadString();
                
        //        dp = tmp;

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Position = 0;
        //        dp = null;

        //        return false;
        //    }
        //}

        //public static bool TryGetFileProtocol(this NetIncomingMessage msg, out BaseProtocol fp)
        //{
        //    try
        //    {
        //        FileProtocol tmp = new FileProtocol();
        //        int length;

        //        tmp.Length = msg.ReadUInt64();
        //        tmp.Type = (FPType)msg.ReadByte();
        //        tmp.Src = msg.ReadInt64();
        //        tmp.Des = msg.ReadInt64();
        //        tmp.File_ID = msg.ReadInt64();
        //        length = msg.ReadInt32();
        //        tmp.Data = msg.ReadBytes(length);

        //        fp = tmp;

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Position = 0;
        //        fp = null;

        //        return false;
        //    }
        //}

        public static string CreateMD5Hash(this string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
                sb.Append(hashBytes[i].ToString("X2"));
            return sb.ToString();
        }
    }
}
