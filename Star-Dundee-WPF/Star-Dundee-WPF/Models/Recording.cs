using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Recording
    {
        private List<Port> ports;
        private int totalErrors;
        private int totalPackets;
        private int totalCharacters;
        private int dataRate;
        private int errorRate;
        private int packetRate;

        public Recording()
        {
            ports = new List<Port>();
        }


        public void setPorts(List<Port> lp) {
            this.ports = lp;
        }

        public int getTotalErrors() { return totalErrors; }
        public int getTotalPackets() { return totalPackets; }
        public int getTotalCharacters() { return totalCharacters; }
        public int getNumberOfPorts() { return ports.Count; }

        public int getdataRate() { return dataRate; }
        public int getErrorRate() { return errorRate; }
        public int getPacketRate() { return packetRate; }


        public void calculateTotals()
        {
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

        public void addPort(Port newPort)
        {
            ports.Add(newPort);
        }

        public Port getPort(int index)
        {
            return ports[index];
        }

        public List<Port> getPorts()
        {
            return ports;
        }
    }
}


