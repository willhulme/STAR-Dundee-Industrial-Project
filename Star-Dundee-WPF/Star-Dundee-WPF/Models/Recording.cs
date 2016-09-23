using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Recording
    {
        public List<Port> ports { get; set; }
        int totalErrors;
        int totalPackets;
        int totalCharacters;
        int dataRate;
        int errorRate;
        int packetRate;




        public void setPorts(List<Port> lp)
        {

            this.ports = lp;

        }
    }
}


