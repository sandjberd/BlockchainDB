using System;
using BCScanner;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BCClientTest
{
    [TestClass]
    public class BCScannerTest
    {
        NodeFinder finder = new NodeFinder();
        [TestMethod]
        public void TestScanPort()
        {
            finder.getBlockchainNodes();
        }
    }
}
