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
        int totalErrors;
        int totalPackets;
        int totalChars;



        int dataRate;
        int errorRate;
        int packetRate;



        public Port(int prt, DateTime start, DateTime end) {

            this.portNumber = prt;
            this.startTime = start;
            this.stopTime = end;


            packets = new List<Packet>();

        }
		
        public int getTotalErrors() { return totalErrors; }
        public int getTotalPackets(){ return totalPackets; }
        public int getTotalChars()  { return totalChars; }
        public DateTime getStart()  { return startTime; }
        public List<Packet> getPackets() { return this.packets; }
        public int getPortNumber() { return this.portNumber; }
        public DateTime getEnd() { return this.stopTime; }

        public void setTotals() {
            setPacketTotal();
            setErrorTotal();
            setTotalCharacters();
        }

        public void setTotalCharacters()
        {
            int total = 0;
            foreach (Packet p in packets)
            {
                total += p.getTotalChars();
            }
            totalChars = total;

        }

        public void setErrorTotal() {
            int errorCount = 0;
            foreach (Packet p in packets)
            {
                if (p.getErrorStatus())
                {
                    errorCount++;
                }
            }
            totalErrors = errorCount;
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


    }



}
