using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Net;
using Newtonsoft.Json.Converters;

namespace CommonTool
{
    public class DataUtility
    {
        public static string ToJson(Object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static object ToObject(string str, Type type)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(str, type);
        }

        /// <summary>
        /// Return true if IP is private.
        /// </summary>
        /// <returns></returns>
        public static bool IsPrivateIP(IPAddress IP)
        {
            byte[] b_IP = IP.GetAddressBytes();

            // 10.x.x.x
            if (Convert.ToInt32(b_IP[0]).Equals(10))
                return true;

            // 172.16.x.x
            if (Convert.ToInt32(b_IP[0]).Equals(172) && Convert.ToInt32(b_IP[1]).Equals(16))
                return true;

            // 192.168.x.x
            if (Convert.ToInt32(b_IP[0]).Equals(192) && Convert.ToInt32(b_IP[1]).Equals(168))
                return true;

            return false;
        }

        public static string NetworkAddress(string ip, string mask)
        {
            string[] ips = ip.Split('.');
            string[] masks = mask.Split('.');

            return string.Format("{0}.{1}.{2}.{3}",
                (Convert.ToByte(ips[0]) & Convert.ToByte(masks[0])).ToString(),
                (Convert.ToByte(ips[1]) & Convert.ToByte(masks[1])).ToString(),
                (Convert.ToByte(ips[2]) & Convert.ToByte(masks[2])).ToString(),
                (Convert.ToByte(ips[3]) & Convert.ToByte(masks[3])).ToString());
        }
    }
}