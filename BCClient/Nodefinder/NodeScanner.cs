using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using BCClient.Nodefinder.Scanner;

namespace BCClient.Nodefinder
{
    public class NodeScanner
    {
        private List<string> nodes = new List<string>();

        public List<string> GetNodes(int port, bool single)
        {
            List<string> clients = new List<string>();
            string gw = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .FirstOrDefault(a => a != null).ToString();


            string[] gwSplitted = gw.Split('.');
            StringBuilder ip = new StringBuilder();
            ip.Append(gwSplitted[0] + ".");
            ip.Append(gwSplitted[1] + ".");
            ip.Append(gwSplitted[2] + ".");
            Console.Write("Scanning clients");
            for (int i = 1; i < 255; i++)
            {
                Console.Write(" .");
                if (PingHost(ip + i.ToString(), port))
                {
                    string node = ip + i.ToString() + ":" + port;
                    nodes.Add(node);
                    Console.WriteLine("Found: " + node);
                    if (single) break;
                }

            }
            return nodes;
        }



        private bool PingHost(string host, int port)
        {
            try
            {
                TcpClient client = new TcpClient(host, port);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
