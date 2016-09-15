using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Port
    {
        string name;
        DateTime startTime;
        DateTime stopTime;
        List<Packet> packets;
        int totalErrors;
        int totalPackets;
        int totalChars;
        int dataRate;
        int errorRate;
        int packetRate;
    
    }
}
