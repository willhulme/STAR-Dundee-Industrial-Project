using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Recording : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Port> ports { get; set; }
        public Graph graphs { get; set; }
        public int totalErrors { get; set; }
        public int totalPackets { get; set; }
        public int totalCharacters { get; set; }
        public decimal dataRate { get; set; }
        public decimal errorRate { get; set; }
        public decimal packetRate { get; set; }
        public TimeSpan recordingTime { get; set; }

        private string[] portSum;
        public string[] portSummary {
            get { return portSum; }
            set
            {
                portSum = value;
               
                OnPropertyChanged("portSummary");
            }
        }
        private string[] packetSum;
        public string[] packetSummary
        {
            get { return packetSum; }
            set
            {
                packetSum = value;

                OnPropertyChanged("packetSummary");
            }
        }


        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
        public Recording()
        {
            ports = new List<Port>();
            graphs = new Graph();
        }


        public void setPorts(List<Port> lp) {
            this.ports = lp;
        }

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

            calcTotalErrorRate();
            calcRecordingTime();
            calcDataRate();
            calcPacketRate();

        }

    public void calcDataRate() {
            dataRate = (decimal)(((double)totalCharacters / recordingTime.TotalSeconds) / 1000);
            dataRate = Math.Round(dataRate, 4);
        }

        public void calcPacketRate()
        {
            packetRate = (decimal)(((double)totalPackets/ recordingTime.TotalSeconds));
            packetRate = Math.Round(packetRate, 4);
        }

        public TimeSpan calcRecordingTime()
        {
            DateTime start = ports[0].startTime;
            DateTime end;
            int x = ports[0].packets.Count();
            end = ports[0].packets[x - 1].timestamp;

            foreach (Port p in ports)
            {
                DateTime currEnd = p.packets[p.packets.Count() - 1].timestamp;

                if (end.CompareTo(currEnd) > 0)
                {
                    end = currEnd;
                }

            }

            //double recordingTime =
            TimeSpan thetimeSpan = (end - start);
            recordingTime = thetimeSpan;

            return recordingTime;
        }


        public void calcTotalErrorRate()
        {
            errorRate = Math.Round((decimal)totalErrors / totalPackets, 4);
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


