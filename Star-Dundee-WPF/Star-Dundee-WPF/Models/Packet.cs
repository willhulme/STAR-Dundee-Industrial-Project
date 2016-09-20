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
        bool hasError;
        public Packet() { }

        public Packet(DateTime timeStamp, Data theData)
        {
            this.timestamp = timeStamp;
            this.theData = theData;
        }

        public Data getData()
        {
            return this.theData;

        }

        public DateTime getTimestamp() {
            return this.timestamp;
        }


        public bool getErrorStatus()
        {
            return hasError;

        }

        public ErrorType getErrorType() {

            return errors;
        }

        public void setError(bool err, string type)
        {
            hasError = err;

            //Distinguish error type, pass in and set enum value

            switch (type)
            {
                case "sequence":

                    this.errors = ErrorType.sequence;
                    break;

                case "disconnect":

                    this.errors = ErrorType.disconnect;
                    break;

                case "parity":

                    this.errors = ErrorType.parity;
                    break;

                case "noError":

                    this.errors = ErrorType.noError;
                    break;

                case "babbling":
                    this.errors = ErrorType.babblingIdiot;
                    break;
                case "":
                    Console.WriteLine("errors occured - maybe");
                    break;

            }

        }


    }


    enum ErrorType
    {
        noError,
        disconnect,
        parity,
        crc,
        eep,
        timeout,
        babblingIdiot,
        sequence
    };
}
