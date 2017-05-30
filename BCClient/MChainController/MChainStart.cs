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

namespace BCClient.MChainController
{
    class MChainStart
    {
        private Process daemon = new Process();
        private Process util = new Process();
        public MChainStart()
        {
            daemon.StartInfo.FileName = "MultichainApps\\multichaind.exe";
            daemon.StartInfo.Arguments = "sandjChain -shortoutput=1";
            daemon.StartInfo.UseShellExecute = false;
            daemon.StartInfo.RedirectStandardOutput = false;
        }

        public async Task<bool> StartDaemonAsync()
        {
            if (Process.GetProcessesByName("multichaind").Length == 0)
            {
                daemon.Start();

                string response = await daemon.StandardOutput.ReadToEndAsync();
                if(response.Contains("started"))
                {
                    Console.WriteLine("Started");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Node is running already");
                return false;
            }
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
                if(result == "sandjChain@10.0.0.10:6719")
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

        public string InitBlockChain(string node)
        {
            string address;
            if (Process.GetProcessesByName("multichaind").Length == 0)
            {

                daemon.StartInfo.Arguments = "sandjChain@" + node + " -shortoutput=1";
                daemon.StartInfo.RedirectStandardOutput = true;
                daemon.Start();

                address = daemon.StandardOutput.ReadToEnd();
                Console.WriteLine(address);
                ConfigureRPC();
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

    }
}
