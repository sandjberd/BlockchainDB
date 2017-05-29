using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiChainLib;
using System.Threading;
using BCClient.Nodefinder;
using BCClient.IO;
using BCClient.MChainController;

namespace BCClient
{
    class Program
    {
        private static MultichainContainer container = new MultichainContainer();
        private static MChainStart mchainStarter = new MChainStart();
        private static string runningNode = "";
        static void Main(string[] args)
        {
            
            Console.WriteLine("BCClient started");

            if (ExistMultichain("sandjChain"))
            {
                string key;

                while ((key = Console.ReadKey().KeyChar.ToString()) != "6")
                {
                    int keyValue;
                    int.TryParse(key, out keyValue);

                    ProcessInput(keyValue);
                }
            }
            else
            {
                //Todo: need new Nodefinder
                runningNode = "10.0.0.11:6719";
                string address = mchainStarter.InitBlockChain(runningNode);
                Console.WriteLine("Yeah here is the address"+address);
            }


        }

        private static void ProcessInput(int keyValue)
        {
            switch (keyValue)
            {
                case 1:
                    Console.WriteLine("Starting Multichain");

                    if (mchainStarter.StartDaemon())
                    {
                        Console.WriteLine("Jippie");
                    }
                    else
                    {
                        Console.WriteLine("Node not started");
                    }
                    
                    /*
                    var task = Task.Run(async () => { return await mchainStarter.StartDaemonAsync(); });
                    if (task.Result == true)
                    {
                        Console.WriteLine("Node Started");
                    }
                    else if (task.Result == false)
                    {
                        Console.WriteLine("Nope");
                    }
                    */
                    break;
                case 2:
                    Console.WriteLine("Starting Multichain");
                    break;
                case 3:
                    Console.WriteLine("Starting Multichain");
                    break;
                case 4:
                    Console.WriteLine("Starting Multichain");
                    break;
                case 5:
                    Console.WriteLine("Starting Multichain");
                    break;
            }
        }

        public static async Task RunAsync()
        {
            IEnumerable<string> address = new string[] { "1Tp4tJSnhtJefoXJKJUm5dFinCce7zZz9H13JK" };

            var client = new MultiChainClient("192.168.1.107", 6718, false, "multichainrpc", "topgeheimespasswort", "sandjChain");
            var x = await client.GetListStreamItems("root");
            //var x = await client.GrantAsync(address, BlockchainPermissions.Connect,0,"0","0",0,0);
            
            

            Console.WriteLine("RunAsync");
            //x.AssertOk();
            //Console.WriteLine(x.Result.ChainName);
            for (int i = 0;i < x.Result.Count;i++)
            {
                Console.WriteLine(x.Result[i].Key);
            }
        }

        public static bool ExistMultichain(string multichainName)
        {
            List<string> multichainpaths = container.GetAllMultichainPaths();
            
            foreach(string mChainName in multichainpaths)
            {
                if (mChainName.Contains(multichainName)) return true;
                else return false;
            }
            return false;
        }
    }
}
