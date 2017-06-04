using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Resources;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using MultiChainLib;
using BCClient.Nodefinder;

namespace BCClient.MChainController
{
    class MChainStart
    {
        private Process daemon = new Process();
        private Process util = new Process();
        private static NodeScanner scanner = new NodeScanner();
        private int port = 0;

        public MChainStart()
        {
            daemon.StartInfo.FileName = "MultichainApps\\multichaind.exe";
            daemon.StartInfo.Arguments = "sandjChain -shortoutput=1";
            daemon.StartInfo.UseShellExecute = false;
            daemon.StartInfo.RedirectStandardOutput = false;
        }

        public bool StartDaemon()
        {
            if (Process.GetProcessesByName("multichaind").Length == 0)
            {
                daemon.StartInfo.RedirectStandardOutput = true;
                daemon.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                daemon.Start();

                string result = daemon.StandardOutput.ReadLineAsync().GetAwaiter().GetResult();
              
                daemon.StandardOutput.Close();
                if(result.Contains("sandjChain@"))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error: "+result);
                    return false;
                }                
            }
            else
            {
                Console.WriteLine("Node is running already");
                return false;
            }
        }

        public string InitBlockChain()
        {
            string address;
            port = 6719;

            //string node = scanner.GetNodes(port, true)[0];
            string node = "10.0.0.11";
            if (Process.GetProcessesByName("multichaind").Length == 0)
            {

                daemon.StartInfo.Arguments = "sandjChain@" + node + ":"+port+" -shortoutput=1";
                daemon.StartInfo.RedirectStandardOutput = true;
                daemon.Start();

                address = daemon.StandardOutput.ReadToEnd();
                Console.WriteLine(address);
                ConfigureRPC();
                GrantPermissions(node, address, port-1, "multichainrpc", "topgeheimespasswort").GetAwaiter().GetResult();
                StartDaemon();
                return address;
            }
            else
            {
                Console.WriteLine("Node is running already");
                return "Error"; 
            }
        }

        private bool ConfigureRPC()
        {
            //TODO: seperate the data
            StringBuilder rpcConf = new StringBuilder();
            rpcConf.AppendLine("rpcuser=multichainrpc");
            rpcConf.AppendLine("rpcpassword=topgeheimespasswort");
            rpcConf.AppendLine("rpcport=6718");
            rpcConf.AppendLine("rpcallowip=0.0.0.0/0");
            
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\MultiChain\\sandjChain\\multichain.conf";
            try
            {
                StreamWriter file =  new StreamWriter(path, false);
                Console.WriteLine(file.GetHashCode());
                file.Write(rpcConf);
                file.Close();
                Console.WriteLine(path.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }

        public async Task GrantPermissions(string runningNode, string newAddress, int port, string username, string password)
        {
            IEnumerable<string> address = new string[] { newAddress };
            try
            {
                var client = new MultiChainClient(runningNode, port, false, username, password, "sandjChain");
                var connect = await client.GrantAsync(address, BlockchainPermissions.Connect, 0, "0", "0", 0, 0);
                var admin = await client.GrantAsync(address, BlockchainPermissions.Admin, 0, "0", "0", 0, 0);
                var issue = await client.GrantAsync(address, BlockchainPermissions.Issue, 0, "0", "0", 0, 0);
                var send = await client.GrantAsync(address, BlockchainPermissions.Send, 0, "0", "0", 0, 0);
                var receive = await client.GrantAsync(address, BlockchainPermissions.Receive, 0, "0", "0", 0, 0);

                Console.WriteLine("Permissions granted!");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}
