using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Packet
    {
        DateTime timestamp;
        ErrorType errors;
        public Data theData;
        int totalChars;

        public Packet(DateTime timeStamp, Data theData)
        {
            this.timestamp = timeStamp;
            this.theData = theData;
        }

        public Data getData() {
            return this.theData;

        }

    }


    enum ErrorType {
        noError,
        disconnect,
        parity,
        crc,
        eep,
        timeout,
        sequence
    };
}
