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
        //List<Tuple<DateTime, decimal>> rate;
        public RateCalculator ()
        {

        }

        public RateCalculator(List<Packet2> packets)
        {
            this.packets = packets;
        }

        public List<Tuple<DateTime,decimal>> CalculateDataRate(List<Packet2> packets)
        {
            List<Tuple<DateTime, decimal>> rate = new List<Tuple<DateTime, decimal>>();
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
            else if(packets.Count < 1000)
            {
                for (int i = 0; i < packets.Count - 1; i+= 50)
                {
                    int totalLength = 0;
                    int packetsAhead = 0;
                    for(int j=i; j < (i+50) && j < packets.Count-1; j++)
                    {
                        Console.WriteLine(j);
                        totalLength += packets[j].dataLength;
                        packetsAhead = j-i;
                    }
                    Tuple<DateTime, decimal> timeAndRate; 
                    TimeSpan difference = (packets[i + packetsAhead].timeStamp - packets[i].timeStamp);
                    decimal kiloBytesPerSecond = (decimal)(((double)totalLength/ difference.TotalSeconds) / 1000);
                    timeAndRate = new Tuple<DateTime, decimal>(packets[i].timeStamp, kiloBytesPerSecond);
                    rate.Add(timeAndRate);
                }
            }
            else if (packets.Count < 5000)
            {
                for (int i = 0; i < packets.Count - 1; i += 100)
                {
                    int totalLength = 0;
                    int packetsAhead = 0;
                    for (int j = i; j < (i + 100) && j < packets.Count - 1; j++)
                    {
                        Console.WriteLine(j);
                        totalLength += packets[j].dataLength;
                        packetsAhead = j - i;
                    }
                    Tuple<DateTime, decimal> timeAndRate; 
                    TimeSpan difference = (packets[i + packetsAhead].timeStamp - packets[i].timeStamp);
                    decimal kiloBytesPerSecond = (decimal)(((double)totalLength / difference.TotalSeconds) / 1000);
                    timeAndRate = new Tuple<DateTime, decimal>(packets[i].timeStamp, kiloBytesPerSecond);
                    rate.Add(timeAndRate);
                }
            }
            else
            {
                for (int i = 0; i < packets.Count - 1; i += 1000)
                {
                    int totalLength = 0;
                    int packetsAhead = 0;
                    for (int j = i; j < (i + 1000) && j < packets.Count - 1; j++)
                    {
                        Console.WriteLine(j);
                        totalLength += packets[j].dataLength;
                        packetsAhead = j - i;
                    }
                    Tuple<DateTime, decimal> timeAndRate; 
                    TimeSpan difference = (packets[i + packetsAhead].timeStamp - packets[i].timeStamp);
                    decimal kiloBytesPerSecond = (decimal)(((double)totalLength / difference.TotalSeconds) / 1000);
                    timeAndRate = new Tuple<DateTime, decimal>(packets[i].timeStamp, kiloBytesPerSecond);
                    rate.Add(timeAndRate);
                }
            }
            return rate;
        }
    }
}
