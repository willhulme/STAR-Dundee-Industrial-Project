using Star_Dundee_WPF.Models.possible_new;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class RateCalculator
    {
        int port;
        List<Packet2> packets;
        List<Tuple<DateTime, decimal>> rate;
        public RateCalculator ()
        {

        }

        public RateCalculator(List<Packet2> packets)
        {
            this.packets = packets;
        }

        //public Tuple<DateTime,float> CalculateDataRate(List<Packet2> packets)
        public void CalculateDataRate(List<Packet2> packets)
        {
            rate = new List<Tuple<DateTime, decimal>>();
            if(packets.Count < 100)
            {
                for(int i = 0; i < packets.Count-1; i++)
                {
                    Tuple<DateTime,decimal> timeAndRate; //The time stamp and the decimal is the rate between it and the next packet in kilobytes
                    TimeSpan difference = (packets[i+1].timeStamp - packets[i].timeStamp);
                    decimal kiloBytesPerSecond = (decimal)(((double)packets[1].dataLength / difference.TotalSeconds)/1000);
                    timeAndRate = new Tuple<DateTime,decimal>(packets[i].timeStamp,kiloBytesPerSecond);
                    rate.Add(timeAndRate);
                }
            }
        }
    }
}
