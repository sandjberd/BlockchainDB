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
            ProcessDirectory(usersPath+"\\MultiChain");
        }
        public void ProcessDirectory(string targetDirectory)
        {
            try
            {
                // Process the list of files found in the directory.
                string[] fileEntries = Directory.GetFiles(targetDirectory);
                foreach (string fileName in fileEntries)
                    ProcessFile(fileName);

                // Recurse into subdirectories of this directory.
                string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
                foreach (string subdirectory in subdirectoryEntries)
                    ProcessDirectory(subdirectory);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Some ERROR {0}",ex.Message); //TODO: insert into Log (Authorization exception)
            }
        }

        // Insert logic for processing found files here.
        private void ProcessFile(string path)
        {
            //Console.WriteLine(path);
            if (path.Contains("MultiChain"))
            {                
                multichainPaths.Add(path);
            }
            foreach(string multichainFile in multichainPaths)
            {
                Console.WriteLine("{0}", multichainFile);
            }
            //Console.WriteLine("Processed file '{0}'.", path);
        }

        public List<string> GetAllMultichainPaths() //Todo: give blockchain names as paramters to check if the blockchain exists there 
        {
            return multichainPaths;
        }

    }
}
