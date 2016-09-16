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
        int sequenceIndex;
        string[] data;

        public Data(string[] data)
        {
            this.data = data;
        }


        public void setSeqIndex(int index)
        {
            this.sequenceIndex = index;
        }


        public int getSeqIndex()
        {
            return this.sequenceIndex;
        }


        public void setSeqNumber(string number)
        {
            this.sequenceNum = number;
        }


        public string[] getTheData() {

            return this.data;
        }

    }
}
