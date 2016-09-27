using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class RateCalculator2
    {
        int port;
        //List<Packet> packets;
        //List<Tuple<DateTime, decimal>> rate;
        public RateCalculator2()
        {

        }

        public RateCalculator2(List<Packet> packets)
        {
            // this.packets = packets;
        }

        public List<Tuple<DateTime, decimal>> CalculateDataRate(List<Packet> packets)
        {
            List<Tuple<DateTime, decimal>> rate = new List<Tuple<DateTime, decimal>>();
            if (packets.Count < 100)
            {
                //int overflow
                for (int i = 0; i < packets.Count - 1; i++)
                {
                    Tuple<DateTime, decimal> timeAndRate; //The time stamp and the decimal is the rate between it and the next packet in kilobytes
                    TimeSpan difference = (packets[i + 1].timestamp - packets[i].timestamp);
                    if (difference.TotalSeconds != 0)
                    {
                        decimal kiloBytesPerSecond = (decimal)(((double)packets[1].dataLength / difference.TotalSeconds) / 1000);
                        timeAndRate = new Tuple<DateTime, decimal>(packets[i].timestamp, kiloBytesPerSecond);
                        rate.Add(timeAndRate);
                    }
                }
            }
            else if (packets.Count < 1000)
            {
                for (int i = 0; i < packets.Count - 1; i += 50)
                {
                    int totalLength = 0;
                    int packetsAhead = 0;
                    for (int j = i; j < (i + 50) && j < packets.Count - 1; j++)
                    {
                        Console.WriteLine(j);
                        totalLength += packets[j].dataLength;
                        packetsAhead = j - i;
                    }
                    Tuple<DateTime, decimal> timeAndRate;
                    TimeSpan difference = (packets[i + packetsAhead].timestamp - packets[i].timestamp);
                    decimal kiloBytesPerSecond = (decimal)(((double)totalLength / difference.TotalSeconds) / 1000);
                    timeAndRate = new Tuple<DateTime, decimal>(packets[i].timestamp, kiloBytesPerSecond);
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
                    TimeSpan difference = (packets[i + packetsAhead].timestamp - packets[i].timestamp);
                    decimal kiloBytesPerSecond = (decimal)(((double)totalLength / difference.TotalSeconds) / 1000);
                    timeAndRate = new Tuple<DateTime, decimal>(packets[i].timestamp, kiloBytesPerSecond);
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
                    TimeSpan difference = (packets[i + packetsAhead].timestamp - packets[i].timestamp);
                    decimal kiloBytesPerSecond = (decimal)(((double)totalLength / difference.TotalSeconds) / 1000);
                    timeAndRate = new Tuple<DateTime, decimal>(packets[i].timestamp, kiloBytesPerSecond);
                    rate.Add(timeAndRate);
                }
            }
            return rate;
        }

        public decimal CalculatePacketRate(List<Packet> packets)
        {
            TimeSpan difference = (packets[packets.Count - 1].timestamp - packets[0].timestamp);
            decimal packetsPerSecond = (decimal)(((double)packets.Count / difference.TotalSeconds));
            return packetsPerSecond;
        }

        public decimal CalculateErrorRate(Port thePort)
        {
            //Calculate packet error rate for a port
            decimal errorRate;
            int totalErrors = thePort.totalErrors;
            int totalPackets = thePort.totalPackets;
            errorRate = (decimal)totalErrors / totalPackets;
            return errorRate;
        }



    }
}
