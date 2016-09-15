using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Data
    {
        string address;
        string sequenceNum;
        string[] data;

        public Data(string[] data)
        {
            this.data = data;
        }
    }
}
