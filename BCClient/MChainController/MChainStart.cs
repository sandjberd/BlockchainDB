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
using BCClient.IO;

namespace BCClient.MChainController
{
    class MChainStart
    {
        private Process daemon = new Process();
        private Process util = new Process();
        private static NodeScanner scanner = new NodeScanner();
        private int port = ConfigReader.GetPort();
        private string chainName = ConfigReader.GetChainName();

        public MChainStart()
        {
            daemon.StartInfo.FileName = "MultichainApps\\multichaind.exe";
            daemon.StartInfo.Arguments = chainName + " -shortoutput=1";
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
                if (string.IsNullOrEmpty(result))
                {
                    InitBlockChain();
                    return true;
                }
                else
                {
                    if (result.Contains(chainName + "@")) //TODO: result NullReferenceException
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error: " + result);
                        return false;
                    }
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

            //string node = scanner.GetNodes(port, true)[0];
            string node = "10.0.0.11";
            if (Process.GetProcessesByName("multichaind").Length == 0)
            {

                daemon.StartInfo.Arguments = chainName + "@" + node + ":"+port+" -shortoutput=1";
                daemon.StartInfo.RedirectStandardOutput = true;
                daemon.Start();

                address = daemon.StandardOutput.ReadToEnd();
                Console.WriteLine("Address: "+address);
                ConfigureRPC();
                GrantPermissions(node, address, ConfigReader.GetRpcPort(), ConfigReader.GetRpcUser(), ConfigReader.GetRpcPassword()).GetAwaiter().GetResult();
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
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\MultiChain\\"+chainName+"\\multichain.conf";
            try
            {
                StreamWriter file =  new StreamWriter(path, false);
                Console.WriteLine(file.GetHashCode());
                file.Write(ConfigReader.GetRpcData());
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
                var client = new MultiChainClient(runningNode, port, false, username, password, chainName);
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
