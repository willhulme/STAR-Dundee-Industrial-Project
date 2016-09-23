using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class GridColumn
    {
        public String Time { get; set; }
        public string[] ports;
        public String Port1 { get; set; }
        public String Port2 { get; set; }
        public String Port3 { get; set; }
        public String Port4 { get; set; }
        public String Port5 { get; set; }
        public String Port6 { get; set; }
        public String Port7 { get; set; }
        public String Port8 { get; set; }

        public GridColumn() {

            Time = "";
            ports = new string[8];
            for(int i = 0; i < 8; i++)
            {
                ports[i] = "";
            }
            Port1 = "";
            Port2 = "";
            Port3 = "";
            Port4 = "";
            Port5 = "";
            Port6 = "";
            Port7 = "";
            Port8 = "";
           

        }

        public GridColumn(String time, String port1,
            String port2, String port3, String port4, String port5,
            String port6, String port7, String port8)
        {
            this.Time = time;
            this.Port1 = port1;
            this.Port2 = port2;
            this.Port3 = port3;
            this.Port4 = port4;
            this.Port5 = port5;
            this.Port6 = port6;
            this.Port7 = port7;
            this.Port8 = port8;
        }

        public static List<GridColumn> getSampleOverviewData()
        {
            return new List<GridColumn>(new GridColumn[16] {
            new GridColumn("14:27:54.245", "", "", "", "",
                "", "", "", ""),                
            new GridColumn("14:27:54.250",  "", "No Error", "", "",
                "", "", "", ""),
            new GridColumn("14:27:54.255", "", "", "Disconnect", "",
                "", "", "", ""),
            new GridColumn("14:27:54.260",  "", "", "", "Parity",
                "", "", "", ""),
            new GridColumn("14:27:54.265",  "", "", "", "",
                "CRC", "", "", ""),
            new GridColumn("14:27:54.270",  "", "", "", "",
                "", "EEP", "", ""),
            new GridColumn("14:27:54.275",  "", "", "", "",
                "", "", "Timeout", ""),
            new GridColumn("14:27:54.280",  "", "", "", "",
                "", "", "", "Babbling Idiot"),
             new GridColumn("14:27:54.285", "", "", "", "",
                "", "", "", ""),
            new GridColumn("14:27:54.290",  "", "No Error", "", "",
                "", "", "", ""),
            new GridColumn("14:27:54.295", "", "", "Disconnect", "",
                "", "", "", ""),
            new GridColumn("14:27:54.300",  "", "", "", "Parity",
                "", "", "", ""),
            new GridColumn("14:27:54.305",  "", "", "", "",
                "CRC", "", "", ""),
            new GridColumn("14:27:54.310",  "", "", "", "",
                "", "EEP", "", ""),
            new GridColumn("14:27:54.315",  "", "", "", "",
                "", "", "Timeout", ""),
            new GridColumn("14:27:54.320",  "", "", "", "",
                "", "", "", "Babbling Idiot")


        });
        }

    }
}
