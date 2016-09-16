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

        public bool readFile()
        {
            //set path for file to be read
            string fileName = "../../DataFiles/test2_link1.rec";

            //check file exists
            if (System.IO.File.Exists(fileName))
            {
                Console.WriteLine("Reading......");
                //read file into string array
                string[] lineInFile = System.IO.File.ReadAllLines(fileName);
                Console.WriteLine("Reading Complete");

                parseFile(lineInFile);
            }
            else
            {
                Console.WriteLine("Error reading file");
            }

            return System.IO.File.Exists(fileName);
        }

        public void parseFile(string[] lineInFile)
        {
            //Isolate start and end timestamps for recording as well as port number
            string startTimeStamp = lineInFile[0];
            string endTimeStamp = lineInFile[lineInFile.Length - 1];
            int portNumber = Convert.ToInt32(lineInFile[1]);

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

            splitData(currentPackets);
        }

        public void splitData(List<string> currentPackets)
        {
            List<Packet> packets = new List<Packet>();
            Sequencer s = new Sequencer();

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

                    //Add to list of packets
                    packets.Add(newPacket);
                }
                //TODO - Add an else if to check packetData[1] is "E"
                // If so, associate the error with the last packet


            }
            
            //Call to find sequnce index of the data
            int seqIndex = s.findSequence(packets);
            //If a non-error value is returned
            if (seqIndex != -1)
            {
                // For each packet
                foreach (Packet p in packets)
                {
                    //Set the index to the actual data objects
                    p.theData.setSeqIndex(seqIndex);
                }
                applySequenceNumbers(packets);
            }
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
        }
    }
}
