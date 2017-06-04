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
        private static MChainstreamController mchainCli = new MChainstreamController("sandjChain");
        static void Main(string[] args)
        {
            
            Console.WriteLine("BCClient started");

            if (container.ExistMultichain("sandjChain"))
            {
                Userprocedure();
            }
            else
            {
                mchainStarter.InitBlockChain();
                Userprocedure();
            }
        }

        private static void Userprocedure()
        {
            string key;
            Console.WriteLine("1.) Start Blockchain");
            Console.WriteLine("2.) Show Connected Nodes");
            Console.WriteLine("3.) Show Datastreams");
            Console.WriteLine("4.) Write into Datastream");
            Console.WriteLine("5.) ");
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
                    StartMultichain();
                    break;
                case 2:
                    ShowConnectedNodes();
                    break;
                case 3:
                    ReadStream();
                    break;
                case 4:
                    WriteIntoStream();
                    break;
                case 5:
                    Console.WriteLine("option");
                    break;
            }
        }

        private static void StartMultichain()
        {
            Console.WriteLine("Starting multichain ...");
            if (mchainStarter.StartDaemon())
            {
                Console.WriteLine("Node started");
            }
            else
            {
                Console.WriteLine("Node not started");
            }
        }
        
        private static void ShowConnectedNodes()
        {
            Console.WriteLine("Show connected nodes");
            mchainCli.GetConnectedNodesLib().GetAwaiter().GetResult(); // Todo: Exception if Multichain is not started
        }

        private static void ReadStream()
        {
            Console.WriteLine("Reading datastream");
            Console.WriteLine("Enter a streamname [root]");
            string streamname = Console.ReadLine();
            mchainCli.GetStreamInfo(streamname).GetAwaiter().GetResult();
        }

        private static void WriteIntoStream()
        {
            Console.WriteLine("Write something into stream");
            Console.WriteLine("Which stream? ");
            string streamname = Console.ReadLine();
            Console.WriteLine("Enter your data:");
            string data = Console.ReadLine();
            mchainCli.WriteIntoStream(streamname, data).GetAwaiter().GetResult();
        }


    }
}
