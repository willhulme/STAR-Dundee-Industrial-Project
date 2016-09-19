using Star_Dundee_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF
{
    class FileParser
    {
        Port thePort;

        string theFilepath = "../../DataFiles/test3/link1.rec";
        bool fileRead;

        public void parse() {

            
            List<string> packetData;
            string[] fileData =  readFile();
            List<Packet> packets = new List<Packet>();


            if (fileRead)//fileData != null)
            {
                packetData = parseFile(fileData);
                packets = splitData(packetData);


                //Call to find sequnce index of the data
                Sequencer s = new Sequencer();
                int seqIndex = s.findSequence(packets);

                //If a non-error value is returned
                if (seqIndex >= 0)
                {
                    // For each packet
                    foreach (Packet p in packets)
                    {
                        //Set the index to the actual data objects
                        p.theData.setSeqIndex(seqIndex);
                    }
                    applySequenceNumbers(packets);
                }
                else if (seqIndex == -1 || seqIndex == -2)
                {
                    //No sequence number Identifiable
                    Console.WriteLine("NO SEQUENCE NUMBER IDENTIFIABLE");
                }
                else {
                    //Error
                    Console.WriteLine("Broke");
                }
            }

            Console.WriteLine("     ");

            printRecordData(packets);


            thePort.setPackets(packets);



        }



        public void printRecordData( List<Packet> packets) {

            int packetcount = 1;


            Console.WriteLine("PRINTING DATA\n");
            Console.WriteLine("Port Number : " + thePort.getPortNumber());
            Console.WriteLine("Starting Timestamp : " + thePort.getStart());
            Console.WriteLine("Ending Timestamp : " + thePort.getEnd());

            Console.WriteLine("\n\n");

            foreach (Packet p in packets)
            {

                Console.WriteLine("TimeStamp : " + p.getTimestamp());

                string[] stringData = p.theData.getTheData();
                Console.Write("DataString : ");

                foreach (string s in stringData)
                {

                    Console.Write(s + " ");

                }

                Console.Write("\n");
                Console.WriteLine("Sequence Number : " + p.theData.getSeqNumber());
                Console.WriteLine("Sequence Index : " + p.theData.getSeqIndex());


                Console.WriteLine("Has Errors? : " + p.getErrorStatus());
                Console.WriteLine("Error Type : " + p.getErrorType());

                Console.WriteLine("Packet Count : " + packetcount);

                packetcount++;
                Console.WriteLine(" ");
            }

            Console.WriteLine(" ");


        }

        public string[] readFile()
        {
            //set path for file to be read
            string fileName = theFilepath;
            fileRead = false;

            //check file exists
            if (System.IO.File.Exists(fileName))
            {
                Console.WriteLine("Reading......");
                //read file into string array
                string[] lineInFile = System.IO.File.ReadAllLines(fileName);
                Console.WriteLine("Reading Complete");
                fileRead = true;
                return lineInFile;
                
            }
            else
            {
                Console.WriteLine("Error reading file");
                fileRead = false;
                return null;
            }

            //return System.IO.File.Exists(fileName);
        }

        public List<string> parseFile(string[] lineInFile)
        {
            //Isolate start and end timestamps for recording as well as port number
            string startTimeStamp = lineInFile[0];
            string endTimeStamp = lineInFile[lineInFile.Length - 1];
            int portNumber = Convert.ToInt32(lineInFile[1]);

            DateTime start = new DateTime();
            start = DateTime.Parse(startTimeStamp);


            DateTime end = new DateTime();
            end = DateTime.Parse(endTimeStamp);

            thePort = new Port(portNumber,  start, end);


            List<string> currentPackets = new List<string>();
            string currentPacket = "";

            //Loop through remainder of the file data
            for (int i = 2; i < lineInFile.Length - 1; i++)
            {
                //Check for an empty line
                if (lineInFile[i].Equals(""))
                {
                    //Check if concatenated string is not empty
                    if (!currentPacket.Equals(""))
                    {
                        //Add data to list of packet data strings and blank placeholder string
                        currentPackets.Add(currentPacket);
                        currentPacket = "";
                    }
                    
                }else{
                    //Add line to placeholder followed by a delimiter
                    currentPacket += lineInFile[i] + "*";
                }
            }
            return currentPackets;
            
        }

        public List<Packet> splitData(List<string> currentPackets)
        {
            List<Packet> packets = new List<Packet>();
          

            int packetCount = 0;

            //Loop through all packet data stored in strings 
            foreach (string packetString in currentPackets)
            {
                //Split the string at the * delimiter character
                string[] packetData = packetString.Split('*');

                //If the data is a packet
                if (packetData[1].Equals("P"))
                {
                    //Store timestamp
                    DateTime packetTimeStamp = DateTime.Parse(packetData[0]);

                    //Split data string into an array of strings, each string representing a 1 byte hex value
                    string[] dataPairs = packetData[2].Split(' ');

                    //Create new data objects for the extracted data
                    Data newData = new Data(dataPairs);
                    Packet newPacket = new Packet(packetTimeStamp, newData);

                    newPacket.setError(false, "noError");

                    //Add to list of packets
                    packets.Add(newPacket);

                    //increment packet count
                    packetCount++;
                }
                else if (packetData[1].Equals("E")) {

                    //Disconnect or parity
                    string errorType = packetData[2].ToLower();

                    packets[packetCount - 1].setError(true,errorType);

                }
            }

            return packets;
            
            
        }

        public void applySequenceNumbers(List<Packet> packets)
        {
            foreach (Packet p in packets)
            {
                //For each packet, add the sequence number to the objects, based on its index 
                int index = p.theData.getSeqIndex();
                string seqNum = p.theData.getTheData()[index];
                p.theData.setSeqNumber(seqNum);
            }
            Console.WriteLine("\"\"");
        }
    }
}
