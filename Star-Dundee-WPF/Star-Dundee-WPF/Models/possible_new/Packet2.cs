using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models.possible_new
{
    class Packet2
    {
        public DateTime timeStamp { get; set;}
        public String errorType { get; set;}
        public string[] data { get; set;}
        public int dataLength { get; set; }
        public char packetType { get; set; }
        public string packetMarkerType { get; set; }
        public string protocol { get; set; }

        public Packet2()
        {

        }
    }
}
