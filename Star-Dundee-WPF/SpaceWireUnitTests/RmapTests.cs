using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Star_Dundee_WPF.Models;

namespace SpaceWireUnitTests
{
    [TestClass]
    public class RmapTests
    {
        [TestMethod]
        public void TestRMAPReadPathAddress()
        {
            string cargo = "4d 20 00 00 03 02 fe 00 01 00 00 00 01 00 00 00 04 f0";
            RMAP rmap = new RMAP();
            rmap.buildPacket(cargo);

            string command = rmap.command;

            Assert.AreEqual("READ", command);
        }
    }
}
