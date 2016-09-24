using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class GridColumn
    {
        public string time;
        public string[] ports;

        public GridColumn() {

            time = "";
            ports = new string[8];
            for(int i = 0; i < 8; i++)
            {
                ports[i] = "";
            }
        }

        public GridColumn(string time, string port1, string port2, string port3, string port4, string port5, string port6, string port7, string port8)
        {
            this.time = time;
            ports[0] = port1;
            ports[1] = port2;
            ports[2] = port3;
            ports[3] = port4;
            ports[4] = port5;
            ports[5] = port6;
            ports[6] = port7;
            ports[7] = port8;
        }

        public void setTime(string time)
        {
            this.time = time;
        }

        public string getTime()
        {
            return time;
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
