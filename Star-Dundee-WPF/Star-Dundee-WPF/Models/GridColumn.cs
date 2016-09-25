using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class GridColumn
    {
        private string time;
        private string port1;
        private string port2;
        private string port3;
        private string port4;
        private string port5;
        private string port6;
        private string port7;
        private string port8;

        public GridColumn() {
            time = "";
        }

        public GridColumn(string time, string port1, string port2, string port3, string port4, string port5, string port6, string port7, string port8)
        {
            this.time = time;
            this.port1 = port1;
            this.port2 = port2;
            this.port3 = port3;
            this.port4 = port4;
            this.port5 = port5;
            this.port6 = port6;
            this.port7 = port7;
            this.port8 = port8;
        }

        public void setTime(string time)
        {
            this.time = time;
        }

        public string getTime()
        {
            return time;
        }

        public void setPort1(string port)
        {
            port1 = port;
        }

        public void setPort2(string port)
        {
            port2 = port;
        }

        public void setPort3(string port)
        {
            port3 = port;
        }

        public void setPort4(string port)
        {
            port4 = port;
        }

        public void setPort5(string port)
        {
            port5 = port;
        }

        public void setPort6(string port)
        {
            port6 = port;
        }

        public void setPort7(string port)
        {
            port7 = port;
        }

        public void setPort8(string port)
        {
            port8 = port;
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
