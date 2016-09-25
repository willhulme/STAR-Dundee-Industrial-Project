using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Port
    {
        public int portNumber { get; set; }
        public DateTime startTime { get; set; }
        public DateTime stopTime { get; set; }
        public List<Packet> packets { get; set; }
        public int totalErrors { get; set; }
        public int totalPackets { get; set; }
        public int totalCharacters { get; set; }

        public int dataRate { get; set; }
        public int errorRate { get; set; }
        public int packetRate { get; set; }

        public Port(int prt, DateTime start, DateTime end) {
            this.portNumber = prt;
            this.startTime = start;
            this.stopTime = end;
            
            packets = new List<Packet>();
        }

        public Port()
        {
            packets = new List<Packet>();
        }

        public void calcTotalValues() {
            calcPacketTotal();
            calcErrorTotal();
            calcTotalCharacters();
        }

        public void calcTotalCharacters()
        {
            foreach (Packet p in packets)
            {
                p.calcTotalChars();
                totalCharacters += p.getTotalChars();
            }
        }

        public void calcErrorTotal() {
            foreach (Packet p in packets)
            {
                if (p.getErrorStatus())
                {
                    totalErrors++;
                }
            }
        }

        public void calcPacketTotal() {
            totalPackets = packets.Count();
        }

        public void setPackets(List<Packet> p) {
            this.packets = p;
        }

        public void addPacketToList(Packet p)
        {
            packets.Add(p);
        }

        public void setStartTime(DateTime startTime)
        {
            this.startTime = startTime;
        }

        public void setPortNumber(int portNumber)
        {
            this.portNumber = portNumber;
        }

        public DateTime getStartTime()
        {
            return startTime;
        }

        public List<Packet> getPackets()
        {
            return packets;
        }

        public Packet getPacket(int index)
        {
            return packets[index];
        }

        public int getTotalErrors()
        {
            return totalErrors;
        }

        public int getTotalPackets()
        {
            return totalPackets;
        }

        public int getTotalChars()
        {
            return totalCharacters;
        }
    }
}
