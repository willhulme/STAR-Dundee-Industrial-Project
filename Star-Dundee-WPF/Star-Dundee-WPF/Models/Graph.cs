using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Star_Dundee_WPF.Models
{
    class Graph
    {
        public SeriesCollection charactersTotalCollection { get; set; }

        public SeriesCollection packetTotalCollection { get; set; }
        public SeriesCollection errorsTotalCollection { get; set; }
        public SeriesCollection errorRateCollection { get; set; }
        public SeriesCollection dataRateCollection { get; set; }
        public SeriesCollection packetRateCollection { get; set; }
        public SeriesCollection dataRateTimeCollection { get; set; }
        public string[] Labels { get; set; }
        public string[] LabelsLine { get; set; }
        public Func<double, string> FormatterPackets { get; set; }
        public Func<double, string> FormatterCharacters { get; set; }
        public Func<double, string> FormatterErrors { get; set; }
        public Func<double, string> FormatterDataRate { get; set; }
        public Func<double, string> FormatterPacketRate { get; set; }
        public Func<double, string> FormatterErrorRate { get; set; }
        public Func<double, string> YFormatter { get; set; }


        public Graph()
        {
            packetTotalCollection = new SeriesCollection { };

           
            Labels = new[] { "Port 8", "Port 7", "Port 6", "Port 5", "Port 4", "Port 3", "Port 2", "Port 1" };
            FormatterPackets = value => value + " Packets";
            charactersTotalCollection = new SeriesCollection { };

            Labels = new[] { "Port 8", "Port 7", "Port 6", "Port 5", "Port 4", "Port 3", "Port 2", "Port 1" };
            FormatterCharacters = value => value + " Characters";

            errorsTotalCollection = new SeriesCollection { };

            Labels = new[] { "Port 8", "Port 7", "Port 6", "Port 5", "Port 4", "Port 3", "Port 2", "Port 1" };
            FormatterErrors = value => value + " Errors";


            dataRateCollection = new SeriesCollection { };

            Labels = new[] { "Port 8", "Port 7", "Port 6", "Port 5", "Port 4", "Port 3", "Port 2", "Port 1" };
            FormatterDataRate = value => value + " kB/s";

            errorRateCollection = new SeriesCollection { };

            Labels = new[] { "Port 8", "Port 7", "Port 6", "Port 5", "Port 4", "Port 3", "Port 2", "Port 1" };
            FormatterErrorRate = value => value + " Error Rate";

            packetRateCollection = new SeriesCollection { };

            Labels = new[] { "Port 8", "Port 7", "Port 6", "Port 5", "Port 4", "Port 3", "Port 2", "Port 1" };
            FormatterPacketRate = value => value + " packet Rate";

            dataRateTimeCollection = new SeriesCollection { };

            LabelsLine = new[] { "20", "40", "60", "80", "100", "120", "140", "160" };
            YFormatter = value => value.ToString("C");

        }
    }

}             


            

    

        

