using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Star_Dundee_WPF.Models;
using System.Collections.Generic;


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

        [TestMethod]
        public void TestRMAPWriteReplyLogicAddress()
        {
            string cargo = "3c 00 4c 00 00 2c";
            RMAP rmap = new RMAP();
            rmap.buildPacket(cargo);

            string command = rmap.command;
            byte[] transactionID = new byte[2] { 0x00, 0x00 };
            byte destinationLogicalAdd = 0x4c;
            uint status = 0x00;
            byte replyCRC = 0x2c;

            Assert.AreEqual("WRITE REPLY", command);
            CollectionAssert.AreEqual(transactionID, rmap.transactionID);
            Assert.AreEqual(destinationLogicalAdd, rmap.destinationlogicalAddress);
            Assert.AreEqual(status, rmap.status);
            Assert.AreEqual(replyCRC, rmap.replyCRC);

        }
		
        [TestMethod]
        public void TestDataSplit2()
        {
            CRC8 crc = new CRC8();
            string cargo = "fe 01 0d 00 fe 00 00 00 00 00 04 0e d9 4b d2 15 1d";
            List<Packet> list;
            Checkmate checkm8;
            initiateCheckmate(out list, out checkm8,cargo);
            checkm8.Check(list);
            Assert.AreEqual("fe 01 0d 00 fe 00 00 00 00 00 04 0e", checkm8.testHeader);
            Assert.AreEqual("d9 4b d2 15 1d", checkm8.testData);
            Assert.AreEqual(0,crc.Check(checkm8.testHeader));
            Assert.AreEqual(0, crc.Check(checkm8.testData));

        }

        [TestMethod]
        public void TestDataSplit()
        {
            CRC8 crc = new CRC8();
            string cargo = "4c 01 7c 20 4a 00 01 00 00 01 00 00 00 00 04 30 00 00 13 61 7b";
            List<Packet> list;
            Checkmate checkm8;
            initiateCheckmate(out list, out checkm8, cargo);
            checkm8.Check(list);
            Assert.AreEqual("4c 01 7c 20 4a 00 01 00 00 01 00 00 00 00 04 30", checkm8.testHeader);
            Assert.AreEqual("00 00 13 61 7b", checkm8.testData);
            Assert.AreEqual(0, crc.Check(checkm8.testHeader));
            Assert.AreEqual(0, crc.Check(checkm8.testData));
        }

        private static void initiateCheckmate(out List<Packet> list, out Checkmate checkm8, string cargo)
        {
            
            string[] dataPairs = cargo.Split(' ');
            DateTime dt = DateTime.ParseExact("08-09-2016 16:59:59.985", "dd-MM-yyyy HH:mm:ss.fff", null);
            Data data = new Data(dataPairs);
            Packet packet = new Packet(dt, data);
            list = new List<Packet>();
            list.Add(packet);
            checkm8 = new Checkmate();
        }

        [TestMethod]
        public void TestCRC()
        {
            CRC8 crcTest = new CRC8();
            Assert.AreEqual(0, crcTest.Check("fe 01 0d 00 fe 00 00 00 00 00 04 0e d9 4b d2 15 1d"));
        }
    }
}
