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
        public int totalErrors { get; set; }
        public int totalPackets { get; set; }
        public int totalCharacters { get; set; }
        public int dataRate { get; set; }
        public int errorRate { get; set; }
        public int packetRate { get; set; }

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


