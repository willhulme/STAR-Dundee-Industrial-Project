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
        public SeriesCollection packetTotalCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public Graph()
        {
            packetTotalCollection = new SeriesCollection
            {
                

            };

            LiveCharts.Wpf.Charts.Base.Chart.Colors = new List<System.Windows.Media.Color>
            {
                System.Windows.Media.Colors.Chartreuse
            };



            //adding series updates and animates the chart
            //SeriesCollection.Add(new StackedColumnSeries
            //{
            //    Values = new ChartValues<double> { 4, 8 },
            //    StackMode = StackMode.Values
            //});

            //adding values also updates and animates
            //SeriesCollection[2].Values.Add(8d);

            Labels = new[] { "Port 8", "Port 7", "Port 6", "Port 5", "Port 4", "Port 3", "Port 2", "Port 1" };
            Formatter = value => value + " Packets";
        }



    }
}
        

