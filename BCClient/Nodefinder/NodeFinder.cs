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
    public class NodeFinder
    {
        private List<string> blockChainNodes = new List<string>();

        public List<string> GetBlockchainNodes(int port)
        {
            try
            {
                string hostName = Dns.GetHostName();
                string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString(); //TODO: deprecated

                var ipAddresses = IpScanner.ScanIPs(myIP);
                foreach (string ip in ipAddresses)
                {
                    //Scan ports
                    var ports = PortScanner.Scan(ip, port, port);
                    foreach (var portLocla in ports)
                    {
                        blockChainNodes.Add(ip + ":" + port);
                        Console.WriteLine(string.Format("Found open port: {0} {1}", port.ToString(), ip)); //TODO: write into log
                        break;
                    }
                }
                return blockChainNodes;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}
