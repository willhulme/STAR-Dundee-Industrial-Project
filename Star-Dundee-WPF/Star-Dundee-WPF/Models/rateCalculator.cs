using Star_Dundee_WPF.Models.possible_new;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class RateCalculator
    {
        int port;
        List<Packet2> packets; 

        public RateCalculator ()
        {

        }

        public RateCalculator(List<Packet2> packets)
        {
            this.packets = packets;
        }

        /*public Tuple<DateTime,float> CalculateDataRate(List<Packet2> packets)
        {

        }*/
    }
}
