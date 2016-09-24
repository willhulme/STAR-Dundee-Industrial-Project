using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Port
    {
        private int portNumber;
        private DateTime startTime;
        private DateTime stopTime;
        private List<Packet> packets;
        private int totalErrors;
        private int totalPackets;
        private int totalCharacters;

        private int dataRate;
        private int errorRate;
        private int packetRate;

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

        public void setTotals() {
            setPacketTotal();
            setErrorTotal();
            setTotalCharacters();
        }

        public void setTotalCharacters()
        {
            foreach (Packet p in packets)
            {
                totalCharacters += p.getTotalChars();
            }
        }

        public void setErrorTotal() {
            foreach (Packet p in packets)
            {
                if (p.getErrorStatus())
                {
                    totalErrors++;
                }
            }
        }

        public void setPacketTotal() {
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
