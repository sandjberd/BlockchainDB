using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiChainLib;
using System.Threading;
using BCScanner;
using BCClient.IO;

namespace BCClient
{
    class Program
    {
        private static MultichainContainer container;
        static void Main(string[] args)
        {
            
            Console.WriteLine("Multichain starting");
            //container = new MultichainContainer();

            RunAsync().GetAwaiter().GetResult();
            //Console.WriteLine(finder.getBlockchainNodes()[0].ToString());
        }

        public static async Task RunAsync()
        {
            IEnumerable<string> address = new string[] { "1Tp4tJSnhtJefoXJKJUm5dFinCce7zZz9H13JK" };

            var client = new MultiChainClient("192.168.1.107", 6718, false, "multichainrpc", "topgeheimespasswort", "sandjChain");
            //var x = await client.GetInfoAsync();
            var x = await client.GrantAsync(address, BlockchainPermissions.Connect,0,"0","0",0,0);
            
            Console.WriteLine("RunAsync");
            //x.AssertOk();
            //Console.WriteLine(x.Result.ChainName);
            Console.WriteLine(x.Result.ToString());

        }

        public bool ExistMultichain(string multichainName)
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
