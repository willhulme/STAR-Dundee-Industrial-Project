﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Packet
    {
        public  DateTime timestamp { get; set; }
        public char packetType { get; set; }
        public string[] dataArray { get; set; }
        public string packetMarkerType { get; set; }
        public string errorType { get; set; }
        public string protocol { get; set; }
        public string packetIndex { get; set; }
        public int portIndex { get; set; }
        public ErrorType error { get; set; }
        public Data theData { get; set; }
        public int dataLength { get; set; }
        public bool hasError { get; set; }
        public uint transactionID { get; set; }

        public Packet()
        {
        }

        public Packet(DateTime timeStamp, Data theData)
        {
            this.timestamp = timeStamp;
            this.theData = theData;
        }

        //public Data getData()
       // {
       //     return this.theData;
       // }

        public DateTime getTimestamp() {
            return this.timestamp;
        }


        public void calcDataLength() {
            dataLength = dataArray.Count();
        }

        public int getTotalChars()
        {
            return dataLength;
        }

        public bool getErrorStatus()
        {
            return hasError;
        }

        public string getErrorType() {
            return errorType;
        }

        public void setError(bool err, string type)
        {
            hasError = err;

            //Distinguish error type, pass in and set enum value
            switch (type)
            {
                case "sequence":
                    this.error = ErrorType.Sequence;
                    break;

                case "None":
                    this.error = ErrorType.None;
                    break;

                case "disconnect":
                    this.error = ErrorType.Disconnect;
                    break;

                case "parity":

                    this.error = ErrorType.Parity;
                    break;

                case "noError":

                    this.error = ErrorType.NoError;
                    break;
                case "eep":

                    this.error = ErrorType.EEP;
                    break;

                case "babbling":
                    this.error = ErrorType.BabblingIdiot;
                    break;

                case "headercrc":
                    this.error = ErrorType.CRCHeader;
                    break;

                case "datacrc":
                    this.error = ErrorType.CRCData;
                    break;

                case "":
                    Console.WriteLine("errors occured - maybe");
                    break;

            }
        }

        public void setTimeStamp(DateTime timeStamp)
        {
            this.timestamp = timeStamp;
        }

        public void setPacketType(char packetType)
        {
            this.packetType = packetType;
        }

        public void setDataArray(string[] dataArray)
        {
            this.dataArray = dataArray;
        }

        public void setPacketMarkerType(string packetMarkerType)
        {
            this.packetMarkerType = packetMarkerType;
        }

        public char getPacketType()
        {
            return packetType;
        }

        public string getPacketMarkerType()
        {
            return packetMarkerType;
        }

        public void setErrorType(string errorType)
        {
            this.errorType = errorType;
            hasError = true;
        }

        public string[] getDataArray()
        {
            return dataArray;
        }

        public void setProtocol(string protocol)
        {
            this.protocol = protocol;
        }

        public string getProtocol()
        {
            return protocol;
        }
    }


    enum ErrorType
    {
        None,
        NoError,
        Disconnect,
        Parity,
        CRCHeader,
        CRCData,
        EEP,
        Timeout,
        BabblingIdiot,
        Sequence
    };
}
