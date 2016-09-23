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
        string [] sequenceNum;
        string protocolID;
        int sequenceIndex;
        string[] data;

        public Data(string[] data)
        {
            this.data = data;
        }

        public string getProtocol() {
            return this.protocolID;
        }

        public void setProtocol(string pID) {
            this.protocolID = pID;
        }

        public int getChars() {
            //Returns the number of characters in bytes
            return data.Count();
        }

        public void setSeqIndex(int index)
        {
            this.sequenceIndex = index;
        }

        public void setAddress(string add)
        {
            this.address = add;
        }

        public string getAddress()
        {
            return this.address;
        }



        public int getSeqIndex()
        {
            return this.sequenceIndex;
        }


        public string[] getSeqNumber() {

            return this.sequenceNum;
        }


        public void setSeqNumber(string [] number)
        {
            this.sequenceNum = number;
        }


        public string[] getTheData() {

            return this.data;
        }

    }
}
