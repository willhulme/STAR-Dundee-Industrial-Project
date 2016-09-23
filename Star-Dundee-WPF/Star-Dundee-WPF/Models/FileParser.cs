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
        Recording theRecord;
        List<Port> thePorts;

        public List<OverviewTest> overviewList { get; set; }

        Port thePort;
        bool fileRead;
        Checkmate crc_check = new Checkmate();
        int currentPort;

        public void parse(string[] filePaths)
        {
            theRecord = new Recording();
            thePorts = new List<Port>();

            List<Packet> packets = new List<Packet>();

            //Check timestamps match and files actually exist
            if (checkTimeStamps(filePaths))
            {
                //For every file selected
                foreach (string file in filePaths)
                {
                    List<string> packetData;

                    //Read file into string array, parse & split
                    string[] fileData = readFile(file);
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
                    else
                    {
                        //Error
                        Console.WriteLine("Broke");
                    }
                    //Set and attach all data to current port object
                    thePort.setPackets(packets);
                    thePort.setTotals();
                    thePorts.Add(thePort);
                }

                //Attach all formatted data to the recording object
                theRecord.setPorts(thePorts);
                theRecord.calculateTotals();

                buildOverview();


                //print data for testing purposes
               printRecordData(thePorts);

            }
            else
            {
                Console.WriteLine("Error reading file(s) - please try again");
            }
        }

        private void buildOverview()
        {
            //To help with the overview  
            string timeFormat = "dd-MM-yyyy HH:mm:ss.fff";
            OverviewTest currentOverview = new OverviewTest();
            DateTime currentTime = theRecord.ports[0].startTime;
            Console.WriteLine("Port Start Time: " + currentTime.ToString(timeFormat));
            int currentPort;


            //Get how many overview segments we need           
            double overviewSegments = (theRecord.ports[0].stopTime - theRecord.ports[0].startTime).TotalMilliseconds;
            overviewSegments++;

            //Start to build the overview
            overviewList = new List<OverviewTest>();
            currentOverview.Time = currentTime.ToString(timeFormat);

            for (int s = 0; s < overviewSegments; s++)
            {
                currentOverview.Time = currentTime.ToString(timeFormat);

                //Console.WriteLine("current overview time: " + currentOverview.Time);
                overviewList.Add(new OverviewTest());
                overviewList[s].Time = currentTime.ToString(timeFormat);
                currentTime = currentTime.AddMilliseconds(1);
                //Console.WriteLine("current time: " + currentTime.ToString(timeFormat));
            }

            Console.WriteLine("Overview Segments: " + overviewSegments);
            Console.WriteLine("Stoptime: " + theRecord.ports[0].stopTime.ToString(timeFormat));
            Console.WriteLine("start time: " + theRecord.ports[0].startTime.ToString(timeFormat));

            //Reset current time
            currentTime = theRecord.ports[0].startTime;
            //For every packet in every port, update the overview                       
            for (int j = 0; j < theRecord.ports.Count; j++)
            {
                Port portToCheck = theRecord.ports[j];

                Console.WriteLine("I'm in port " + (j + 1));


                int timeStampCounter = 0;

                for (int k = 0; k < portToCheck.packets.Count; k++)
                {
                    Packet packetToCheck = portToCheck.packets[k];

                    bool found = false;

                    while (found == false && timeStampCounter < overviewList.Count)
                    {
                        found = (overviewList[timeStampCounter].Time.Equals(packetToCheck.timestamp.ToString(timeFormat), StringComparison.Ordinal));
                        timeStampCounter++;
                    }

                    //int value1 = overviewList.FindIndex(
                    //    delegate (OverviewTest ovTest)
                    //    {
                    //        return ovTest.Time.Equals(packetToCheck.timestamp.ToString(timeFormat), StringComparison.Ordinal);
                    //    }
                    //    );

                    if (!found)
                    {
                        timeStampCounter = 0;

                        while (found == false && timeStampCounter < overviewList.Count)
                        {
                            found = (overviewList[timeStampCounter].Time.Equals(packetToCheck.timestamp.ToString(timeFormat), StringComparison.Ordinal));
                            timeStampCounter++;
                        }

                        timeStampCounter--;

                        Console.WriteLine("This fucked up at port " + (j + 1) + ", packet " + k + ", timeStampCounter " + timeStampCounter + ", and timestamp " + packetToCheck.timestamp.ToString(timeFormat));
                    }

                    //Get the port number and the error type and add it to the overview segment
                    currentPort = theRecord.ports[j].portNumber;

                    switch (currentPort)
                    {
                        case 1:
                            overviewList[timeStampCounter].Port1 = packetToCheck.errors.ToString();
                            break;

                        case 2:
                            overviewList[timeStampCounter].Port2 = packetToCheck.errors.ToString();
                            break;

                        case 3:
                            overviewList[timeStampCounter].Port3 = packetToCheck.errors.ToString();
                            break;

                        case 4:
                            overviewList[timeStampCounter].Port4 = packetToCheck.errors.ToString();
                            break;

                        case 5:
                            overviewList[timeStampCounter].Port5 = packetToCheck.errors.ToString();
                            break;

                        case 6:
                            overviewList[timeStampCounter].Port6 = packetToCheck.errors.ToString();
                            break;

                        case 7:
                            overviewList[timeStampCounter].Port7 = packetToCheck.errors.ToString();
                            break;

                        case 8:
                            overviewList[timeStampCounter].Port8 = packetToCheck.errors.ToString();
                            break;
                    }
                }


                Console.WriteLine("Overview Segment Time: " + currentTime.ToString(timeFormat));

            }
        }

        public bool checkTimeStamps(string[] filePaths)
        {
            string[] startTimes = new string[filePaths.Length];

            for (int i = 0; i < filePaths.Length; i++)
            {
                if (System.IO.File.Exists(filePaths[i]))
                {
                    System.IO.StreamReader currentFile = new System.IO.StreamReader(filePaths[i]);
                    startTimes[i] = currentFile.ReadLine();
                }
                else
                {
                    return false;
                }
            }

            bool matchingStamps = true;
            int counter = 0;

            while (matchingStamps && counter < startTimes.Length - 1)
            {
                matchingStamps = startTimes[counter].Equals(startTimes[counter + 1]);
                counter++;
            }

            return matchingStamps;
        }

        public void printRecordData(List<Port> ports)
        {
            string timeFormat= "dd-MM-yyyy HH:mm:ss.fff";

            int packetcount = 0;
            int currPort;

            Console.WriteLine("PRINTING DATA\n");
            Console.WriteLine("Recording Totals");
            Console.WriteLine("Number of ports used : " + theRecord.getNumberOfPorts());
            Console.WriteLine("Total Packets : " + theRecord.getTotalPackets());
            Console.WriteLine("Total Errors : " + theRecord.getTotalErrors());
            Console.WriteLine("Total Characters : " + theRecord.getTotalCharacters() + " Bytes");
            Console.WriteLine("\n\n");

            foreach (Port thePort in ports)
            {
                List<Packet> packets = thePort.getPackets();
                currPort = thePort.getPortNumber();
                packetcount = 0;

                Console.WriteLine("PRINTING DATA\n");

                Console.WriteLine("Port Number : " + currPort);
                Console.WriteLine("Starting Timestamp : " + thePort.getStart().ToString(timeFormat));

                Console.WriteLine("Ending Timestamp : " + thePort.getEnd().ToString(timeFormat));

				Console.WriteLine("Number of Packets : " + thePort.getTotalPackets());
                Console.WriteLine("Number of Errors : " + thePort.getTotalErrors());
                Console.WriteLine("Number of Characters : " + thePort.getTotalChars() + " Bytes");

                Console.WriteLine("\n\n");

                foreach (Packet p in packets)
                {
                    packetcount++;

                    Console.WriteLine("TimeStamp : " + p.getTimestamp().ToString(timeFormat));

                    string[] stringData = p.theData.getTheData();
                    Console.Write("DataString : ");

                    foreach (string s in stringData)
                    {
                        Console.Write(s + " ");
                    }

                    Console.Write("\n");

                    Console.WriteLine("Sequence Index : " + p.theData.getSeqIndex());

                    Console.Write("Sequence Number : ");
                    string[] sequenceData = p.theData.getSeqNumber();

                    foreach (string s in sequenceData)
                    {
                        Console.Write(s + " ");
                    }
                    Console.Write("\n");
                    Console.WriteLine("Protocol ID : " + p.theData.getProtocol());


                    Console.WriteLine("Packet Address : " + p.theData.getAddress());

                    Console.WriteLine("Has Errors? : " + p.getErrorStatus());
                    Console.WriteLine("Error Type : " + p.getErrorType());

                    Console.WriteLine("Characters : " + p.getTotalChars() + " Bytes");

                    Console.WriteLine("Packet Count : [Port " + currPort + "] " + packetcount);

                    Console.WriteLine(" ");
                }

                Console.WriteLine(" ");

            }
        }

        public string[] readFile(string dir)
        {

            Console.WriteLine("Reading......");

            //read file into string array
            string[] lineInFile = System.IO.File.ReadAllLines(dir);

            Console.WriteLine("Reading Complete");

            return lineInFile;

        }

        public List<string> parseFile(string[] lineInFile)
        {
            //Isolate start and end timestamps for recording as well as port number
            string startTimeStamp = lineInFile[0];
            string endTimeStamp = lineInFile[lineInFile.Length - 1];
            int portNumber = Convert.ToInt32(lineInFile[1]);
            currentPort = portNumber;

            //Store isolated data in necessary data types
            DateTime start = new DateTime();
            start = DateTime.Parse(startTimeStamp);
            DateTime end = new DateTime();
            end = DateTime.Parse(endTimeStamp);
            thePort = new Port(portNumber, start, end);

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
                }
                else
                {
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

                    if (packetData[3].Equals("EOP"))
                    {
                        newPacket.setError(false, "noError");
                    }
                    else if (packetData[3].Equals("EEP"))
                    {
                        newPacket.setError(true, "eep");
                    }
                    else if (packetData[3].Equals("None"))
                    {
                        Console.Write("");
                        //newPacket.setError(false, "eep");
                    }

                    newPacket.setTotalChars();
                    //Add to list of packets
                    packets.Add(newPacket);

                    //increment packet count
                    packetCount++;
                }
                else if (packetData[1].Equals("E"))
                {
                    //Disconnect or parity
                    string errorType = packetData[2].ToLower();

                    if (packets[packetCount - 1].getErrorType() == ErrorType.noError)
                    {
                        packets[packetCount - 1].setError(true, errorType);
                    }
                }
            }

           // packets = crc_check.Check(packets);

            return packets;
        }

        public void applySequenceNumbers(List<Packet> packets)
        {
            string [] prevSeq;
            Packet p;
            bool isRmap;
            string protocolID = packets[0].getData().getProtocol();

            if (protocolID.Equals("01"))
            {
                isRmap = true;
            }
            else {
                isRmap = false;
            }

            for(int i = 0;i<packets.Count();i++)
            {
                string[] sa;
                p = packets[i];
                //For each packet, add the sequence number to the objects, based on its index 
                //If packet has no error or sequence error
                if (!p.getErrorStatus() || (p.getErrorStatus() && (p.getErrorType() == ErrorType.sequence || p.getErrorType() == ErrorType.babblingIdiot)))
                {

                    
                    int index = p.theData.getSeqIndex();
                    string seqNum = p.theData.getTheData()[index];
                    string prevByte = p.theData.getTheData()[index-1];
                    if (isRmap)
                    {
                        string[] seqBytes = { prevByte, seqNum };
                        sa = seqBytes;
                    }
                    else
                    {
                        string[] seqBytes = { seqNum };
                        sa = seqBytes;
                    }
                        p.theData.setSeqNumber(sa);
                }
                else
                {
                    string prevByte = p.theData.getTheData()[(p.theData.getSeqIndex()) - 1];


                    if (isRmap)
                    {
                        string[] seqBytes = { prevByte, "err" };
                        sa = seqBytes;
                    }
                    else
                    {
                        string[] seqBytes = { "err" };
                        sa = seqBytes;
                    }
                    p.theData.setSeqNumber(sa);
                }
                prevSeq = p.getData().getSeqNumber();
            }
            Console.WriteLine("\"\"");
        }
    }
}
