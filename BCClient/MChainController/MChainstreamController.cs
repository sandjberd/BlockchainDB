using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiChainLib;

namespace BCClient.MChainController
{
    class MChainstreamController
    {
        private Process cli = new Process();
        public string ChainName { get; private set; }
        public int Chainport { get; private set; }
        public string RpcUser { get; private set; }
        public string RpcPassword { get; private set; }
        public string RpcHost { get; private set; }

        private MultiChainClient client;

        public MChainstreamController(string blockchain)
        {
            ChainName = blockchain;
            RpcUser = "multichainrpc";
            RpcPassword = "topgeheimespasswort";
            RpcHost = "localhost";
            cli.StartInfo.FileName = "MultichainApps\\multichain-cli.exe";
            cli.StartInfo.Arguments = ChainName;
            cli.StartInfo.UseShellExecute = false;

            client = new MultiChainClient(RpcHost, 6718, false, RpcUser, RpcPassword, ChainName);

        }

        public List<string> GetConnectedNodes()
        {
            string result = "";
            List<string> nodes = new List<string>();
            cli.StartInfo.Arguments += " getpeerinfo";
            Console.WriteLine(cli.StartInfo.Arguments);
            cli.StartInfo.RedirectStandardOutput = true;
            cli.Start();

            result = cli.StandardOutput.ReadToEnd();
            Console.WriteLine(result);
            return nodes;
        }

        public async Task GetConnectedNodesLib()
        {
            var x = await client.GetPeerInfoAsync();
            x.AssertOk();
            
            for (int i = 0; i < x.Result.Count; i++)
            {
                Console.WriteLine(x.Result[i].Addr);
                Console.WriteLine(x.Result[i].Handshake);
            }
        }

        public async Task GetStreamInfo(string streamname)
        {
            var x = await client.GetListStreamItems(streamname);
            Console.WriteLine("Datensätze im Stream: "+x.Result.Count);
            foreach (var value in x.Result)
            {
                Console.WriteLine(value.Data+": "+value.Key);
            }
        }

        public async Task WriteIntoStream(string streamname, string data)
        {
            byte[] ba = Encoding.Default.GetBytes(data);
            var hexString = BitConverter.ToString(ba);
            hexString = hexString.Replace("-", "");

            var x = await client.PublishIntoStream(streamname, data, hexString);
            Console.WriteLine("TransactionID: "+x.Result);
        }
    }
}
