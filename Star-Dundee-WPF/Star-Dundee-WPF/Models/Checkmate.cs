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
                if (!item.getErrorStatus())
                {
                    if (SplitCargo.Length > 1)
                    {
                        string CargoHead = SplitCargo[0]; //HEADER
                        string CargoData = SplitCargo[1]; //DATA
                        int HeadResult = CRC.Check(CargoHead);
                        int DataResult = CRC.Check(CargoData);
                    }
                }
                else
                {
                    string CargoHead = SplitCargo[0]; //HEADER
                    int HeadResult = CRC.Check(CargoHead);
                }
            }
            return Packet;
        }
    }
}


