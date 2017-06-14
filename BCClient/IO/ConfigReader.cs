using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using IniParser;
using IniParser.Model;

namespace BCClient.IO
{
    static class ConfigReader
    {
        private static FileIniDataParser parser = new FileIniDataParser();
        private static IniData data = parser.ReadFile("config.ini");

        public static string GetChainName()
        {
            return data["Blockchain"]["name"];
        }

        public static int GetPort()
        {
            try
            {
                return Convert.ToInt32(data["Blockchain"]["port"]);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        public static StringBuilder GetRpcData()
        {
            StringBuilder rpcConf = new StringBuilder();
            rpcConf.AppendLine("rpcuser=multichainrpc");
            rpcConf.AppendLine("rpcpassword=topgeheimespasswort");
            rpcConf.AppendLine("rpcport=6718");
            rpcConf.AppendLine("rpcallowip=0.0.0.0/0");

            return rpcConf;
        }

        public static string GetRpcPassword()
        {
            return data["Rpc"]["rpcpassword"];
        }

        public static string GetRpcUser()
        {
            return data["Rpc"]["rpcuser"];
        }

        public static int GetRpcPort()
        {
            try
            {
                return Convert.ToInt32(data["Rpc"]["rpcport"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }
    }
}
