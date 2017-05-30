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

            if (container.ExistMultichain("sandjChain"))
            {
                Userprocedure();
            }
            else
            {
                //Todo: need new Nodefinder
                runningNode = "10.0.0.11:6719";
                string address = mchainStarter.InitBlockChain(runningNode);
                RegisterNode("10.0.0.11",address,6718, "multichainrpc","topgeheimespasswort").GetAwaiter().GetResult();
                Console.WriteLine("Permissionsgranted");
                Userprocedure();
            }
        }

        private static void Userprocedure()
        {
            string key;
            Console.WriteLine("1.) Start Blockchain");
            Console.WriteLine("2.) Show Connected Nodes");
            Console.WriteLine("3.) Show Datastreams");
            Console.WriteLine("4.) Create Datastream");
            Console.WriteLine("5.) Write into Datastream");
            while ((key = Console.ReadKey().KeyChar.ToString()) != "6")
            {
                int keyValue;
                int.TryParse(key, out keyValue);

                ProcessInput(keyValue);
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
                        Console.WriteLine("Node started");
                    }
                    else
                    {
                        Console.WriteLine("Node not started");
                    }
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

        public static async Task RegisterNode(string runningNode, string newAddress, int port, string username, string password)
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
