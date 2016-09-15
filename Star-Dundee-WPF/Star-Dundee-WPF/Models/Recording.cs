using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Recording
    {
        List<Port> ports;
        int totalErrors;
        int totalPackets;
        int totalCharacters;
        int dataRate;
        int errorRate;
        int packetRate;
    }
}
