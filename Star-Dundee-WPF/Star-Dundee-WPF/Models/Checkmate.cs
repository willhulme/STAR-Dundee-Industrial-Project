using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Checkmate
    {
        CRC8 CRC = new CRC8();
        Packet Packet = new Packet();
        RMAP testRmapPlzRemovePlz = new RMAP();
        public string[] dataHeaderSplittest;
        public string testHeader;
        public string testData;
        public List<Packet> Check(List<Packet> Packet)
        {
            foreach (Packet item in Packet)
            {
                string[] cargo = item.theData.getTheData();
                dataHeaderSplittest = testRmapPlzRemovePlz.GetHeader(cargo);
                testHeader = dataHeaderSplittest[0]; //HEADER
                testData = dataHeaderSplittest[1]; //DATA
            }
            return Packet;
        }
    }
} 
