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

        int numberOfPorts;
        int totalErrors;
        int totalPackets;
        int totalCharacters;

        int dataRate;
        int errorRate;
        int packetRate;
		
        public void setPorts(List<Port> lp) {
            this.ports = lp;
        }

        public int getTotalErrors() { return totalErrors; }
        public int getTotalPackets() { return totalPackets; }
        public int getTotalCharacters() { return totalCharacters; }
        public int getNumberOfPorts() { return numberOfPorts; }

        public int getdataRate() { return dataRate; }
        public int getErrorRate() { return errorRate; }
        public int getPacketRate() { return packetRate; }


        public void calculateTotals()
        {
            numberOfPorts = ports.Count();

            int errTot =0;
            int packTot =0;
            int charTot =0;
            foreach (Port p in ports)
            {
                errTot += p.getTotalErrors();
                packTot += p.getTotalPackets();
                charTot += p.getTotalChars();
            }

            totalCharacters = charTot;
            totalErrors = errTot;
            totalPackets = packTot;
        }
    }
}


