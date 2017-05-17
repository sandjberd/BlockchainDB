using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using BCScanner.Scanner;

namespace BCScanner
{
    public class NodeFinder
    {
        private static int port = 7358; //TODO: put it in a config file
        private static List<string> blockChainNodes = new List<string>();

        public List<string> getBlockchainNodes()
        {
            string hostName = Dns.GetHostName();
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString(); //TODO: deprecated

            var ipAddresses = IpScanner.ScanIPs(myIP);
            foreach (string ip in ipAddresses)
            {
                //Scan ports
                var ports = PortScanner.Scan(ip, port, port);
                foreach (var port in ports)
                {
                    blockChainNodes.Add(ip + ":" + port);
                    Console.WriteLine(string.Format("Found open port: {0}", port.ToString())); //TODO: write into log
                }

            }

            return blockChainNodes;
        }

    }
}
