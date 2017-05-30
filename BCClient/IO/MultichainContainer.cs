using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BCClient.IO
{
    /*
     * This class returns if on the local node exists a blockchain 
     * 
     * 
     * */
    class MultichainContainer
    {
        private List<string> multichainPaths = new List<string>();
        public MultichainContainer() 
        {
            string usersPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            ProcessDirectory(usersPath+"\\MultiChain\\");
        }
        private void ProcessDirectory(string targetDirectory)
        {
            try
            {
                string[] fileEntries = Directory.GetDirectories(targetDirectory);
                foreach(string directory in fileEntries)
                {
                    multichainPaths.Add(directory);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Some ERROR {0}",ex.Message); //TODO: insert into Log (Authorization exception)
            }
        }


        public List<string> GetAllMultichainPaths() //Todo: give blockchain names as paramters to check if the blockchain exists there 
        {
            return multichainPaths;
        }

        public bool ExistMultichain(string multichainName)
        {
            foreach (string mChainName in multichainPaths)
            {
                if (mChainName.Contains(multichainName))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
