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
            string cargo = "4d 20 00 00 03 02 fe 00 00 00 00 00 01 00 00 00 04 dc";
            RMAP rmap = new RMAP();
            rmap.buildPacket(cargo);

            string command = rmap.command;
            byte[] transactionID = new byte[2] {0x00, 0x00};
            uint datalength = 4;
            uint readAddress = 0x00000100;
            byte extReadAddress = 0x00;

            Assert.AreEqual("READ", command);
            Assert.AreEqual(0x20, rmap.destinationKey);
            Assert.AreEqual(0Xfe, rmap.sourcelogicalAddress);
            CollectionAssert.AreEqual(transactionID, rmap.transactionID);
            Assert.AreEqual(datalength, rmap.dataLengthInt);
            Assert.AreEqual(readAddress, rmap.readAddress);
            Assert.AreEqual(extReadAddress, rmap.extReadAdd);
            
        }

        [TestMethod]
        public void TestRMAPWriteLogicAddress()
        {
            string cargo = "7c 20 4a 00 02 00 00 01 00 00 00 00 04 44 00 00 31 38 78";
            RMAP rmap = new RMAP();
            rmap.buildPacket(cargo);

            string command = rmap.command;
            byte[] transactionID = new byte[2] { 0x00, 0x02 };
            uint datalength = 4;
            uint writeAddress = 0x00010000;
            byte extWriteAddress = 0x00;

            Assert.AreEqual("WRITE", command);
            Assert.AreEqual(0x20, rmap.destinationKey);
            Assert.AreEqual(0X4a, rmap.sourcelogicalAddress);
            CollectionAssert.AreEqual(transactionID, rmap.transactionID);
            Assert.AreEqual(datalength, rmap.dataLengthInt);
            Assert.AreEqual(writeAddress, rmap.writeAddress);
            Assert.AreEqual(extWriteAddress, rmap.extWriteAdd);

        }

        [TestMethod]
        public void TestRMAPReadLogicAddress()
        {
            string cargo = "4c 20 2d ff fa 00 00 02 00 00 00 00 08 12";
            RMAP rmap = new RMAP();
            rmap.buildPacket(cargo);

            string command = rmap.command;
            byte[] transactionID = new byte[2] { 0xff, 0xfa };
            uint datalength = 8;
            uint readAddress = 0x00020000;
            byte extReadAddress = 0x00;
            byte headerCRC = 0x12;

            Assert.AreEqual("READ", command);
            Assert.AreEqual(0x20, rmap.destinationKey);
            Assert.AreEqual(0X2d, rmap.sourcelogicalAddress);
            CollectionAssert.AreEqual(transactionID, rmap.transactionID);
            Assert.AreEqual(datalength, rmap.dataLengthInt);
            Assert.AreEqual(readAddress, rmap.readAddress);
            Assert.AreEqual(extReadAddress, rmap.extReadAdd);
            Assert.AreEqual(headerCRC, rmap.headerCRC);

        }

        [TestMethod]
        public void TestRMAPReadReplyPathAddress()
        {
            string cargo = "0d 00 fe 00 00 00 00 00 04 0e d9 4b d2 15 1d";
            RMAP rmap = new RMAP();
            rmap.buildPacket(cargo);

            string command = rmap.command;
            byte[] transactionID = new byte[2] { 0x00, 0x00 };
            uint datalength = 4;
            byte[] data = new byte[4] {0xd9, 0x4b, 0xd2, 0x15};
            byte headerCRC = 0x0e;
            byte dataCRC = 0x1d;

            Assert.AreEqual("READ REPLY", command);
            Assert.AreEqual(0x00, rmap.status);
            Assert.AreEqual(0Xfe, rmap.destinationlogicalAddress);
            CollectionAssert.AreEqual(transactionID, rmap.transactionID);
            Assert.AreEqual(datalength, rmap.dataLengthInt);
            Assert.AreEqual(headerCRC, rmap.headerCRC);
            Assert.AreEqual(dataCRC, rmap.dataCRC);

        }
    }
}
