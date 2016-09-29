using LiveCharts;
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
        public Func<double, string> Formatter { get; set; }

        public Graph()
        {
            packetTotalCollection = new SeriesCollection{};

            LiveCharts.Wpf.Charts.Base.Chart.Colors = new List<System.Windows.Media.Color>
            {

                System.Windows.Media.Colors.Aqua
            };
            Labels = new[] { "Port 8", "Port 7", "Port 6", "Port 5", "Port 4", "Port 3", "Port 2", "Port 1" };
            Formatter = value => value + " Packets";

            charactersTotalCollection = new SeriesCollection { };

            LiveCharts.Wpf.Charts.Base.Chart.Colors = new List<System.Windows.Media.Color>
            {
                System.Windows.Media.Colors.Coral


            };
            Labels = new[] { "Port 8", "Port 7", "Port 6", "Port 5", "Port 4", "Port 3", "Port 2", "Port 1" };
            Formatter = value => value + " Characters";

            errorsTotalCollection = new SeriesCollection { };

            LiveCharts.Wpf.Charts.Base.Chart.Colors = new List<System.Windows.Media.Color>
            {
                System.Windows.Media.Colors.DarkGreen
            };
            Labels = new[] { "Port 8", "Port 7", "Port 6", "Port 5", "Port 4", "Port 3", "Port 2", "Port 1" };
            Formatter = value => value + " Errors";


            dataRateCollection = new SeriesCollection { };

            LiveCharts.Wpf.Charts.Base.Chart.Colors = new List<System.Windows.Media.Color>
            {
                System.Windows.Media.Colors.DarkBlue
            };
            Labels = new[] { "Port 8", "Port 7", "Port 6", "Port 5", "Port 4", "Port 3", "Port 2", "Port 1" };
            Formatter = value => value + " Data Rate";

            errorRateCollection = new SeriesCollection { };


            LiveCharts.Wpf.Charts.Base.Chart.Colors = new List<System.Windows.Media.Color>
            {
                System.Windows.Media.Colors.DarkOrange
            };
            Labels = new[] { "Port 8", "Port 7", "Port 6", "Port 5", "Port 4", "Port 3", "Port 2", "Port 1" };
            Formatter = value => value + " Error Rate";

            packetRateCollection = new SeriesCollection { };

            LiveCharts.Wpf.Charts.Base.Chart.Colors = new List<System.Windows.Media.Color>
            {
                System.Windows.Media.Colors.Khaki
            };
            Labels = new[] { "Port 8", "Port 7", "Port 6", "Port 5", "Port 4", "Port 3", "Port 2", "Port 1" };
            Formatter = value => value + " packet Rate";

            dataRateTimeCollection = new SeriesCollection { };

            LiveCharts.Wpf.Charts.Base.Chart.Colors = new List<System.Windows.Media.Color>
            {
                System.Windows.Media.Colors.SkyBlue
            };
            Labels = new[] { "Port 8", "Port 7", "Port 6", "Port 5", "Port 4", "Port 3", "Port 2", "Port 1" };
            Formatter = value => value + " Packets";

        }
    }
}
        

