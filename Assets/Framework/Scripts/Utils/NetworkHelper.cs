using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Framework.Scripts.Utils
{
    public class NetworkHelper
    {
        public static List<String> GetLocalIpAddresses()
        {
            var ipAddresses = new List<String>();

            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddresses.Add(ip.ToString());
                }
            }

            return ipAddresses;
        }
    }
}