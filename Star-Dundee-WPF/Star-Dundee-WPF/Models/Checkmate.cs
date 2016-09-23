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

        RMAP RMAP = new RMAP();
        public List<Packet> Check(List<Packet> Packet)
        {
            foreach (Packet item in Packet)
            {
                string[] cargo = item.theData.getTheData();
                string[] SplitCargo = RMAP.GetHeader(cargo);
                string CargoHead = SplitCargo[0]; //HEADER
                string CargoData = SplitCargo[1]; //DATA
                int HeadResult = CRC.Check(CargoHead);
                int DataResult = CRC.Check(CargoData);
                if (DataResult != 0)
                {
                    if (DataResult == -2)
                    {
                        item.setError(true, "length");
                    }
                    else
                    {
                        item.setError(true, "datacrc");
                    }
                }
                if (HeadResult != 0)
                {
                    if (HeadResult == -2)
                    {
                        item.setError(true, "length");
                    }
                    else
                    {
                        item.setError(true, "headercrc");
                    }
                    //if (!item.getErrorStatus())
                    //{
                    //    string[] cargo = item.theData.getTheData();
                    //    dataHeaderSplittest = testRmapPlzRemovePlz.GetHeader(cargo);
                    //    if (dataHeaderSplittest.Length > 1)
                    //    {
                    //        testHeader = dataHeaderSplittest[0]; //HEADER
                    //        testData = dataHeaderSplittest[1]; //DATA
                    //    }
                    //    else
                    //    {
                    //        testHeader = dataHeaderSplittest[0];

                    //    }
                }
            }
            return Packet;
        }
    }
}


