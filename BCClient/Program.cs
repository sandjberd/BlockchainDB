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
        private NodeFinder finder;
        private static MultichainContainer container;
        static void Main(string[] args)
        {
            
            Console.WriteLine("Multichain starting");
            container = new MultichainContainer();

            //RunAsync().GetAwaiter().GetResult();
        }

        public static async Task RunAsync()
        {
            var client = new MultiChainClient("localhost", 7358, false, "multichainrpc", "7Dvu86aBMUxNiuFEChRJH6GJ2taMPKSKtiuawrwvzaa9", "publicchain");
            var x = await client.GetInfoAsync();
            
            
            Console.WriteLine("RunAsync");
            //x.AssertOk();
            Console.WriteLine(x.Result.ChainName);
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
