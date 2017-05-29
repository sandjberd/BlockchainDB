using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BCClient.MChainController
{
    class MChainStart
    {
        private Process daemon = new Process();
        private Process cli = new Process();
        private Process util = new Process();
        public MChainStart()
        {
            daemon.StartInfo.FileName = "MultichainApps\\multichaind.exe";
            daemon.StartInfo.Arguments = "sandjChain shortoutput=1";
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
                daemon.Start();
                while (Console.ReadLine() == "Node started")
                {
                    Task.Delay(2000);
                }
                return true;
            }
            else
            {
                Console.WriteLine("Node is running already");
                return false;
            }
        }

        public string InitBlockChain(string node)
        {
            if (Process.GetProcessesByName("multichaind").Length == 0)
            {
                daemon.StartInfo.Arguments = "sandjChain@" + node + " shortoutput=1";
                daemon.Start();
                while (Console.ReadLine().Length > 5)
                {
                    Task.Delay(2000);
                }
                return "Console.ReadLine()";
            }
            else
            {
                Console.WriteLine("Node is running already");
                return "Error"; 
            }
        }

    }
}
