﻿using Star_Dundee_WPF.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF
{
    class FileParser
    {

        public Recording mainRecording { get; set; }
        public List<GridColumn> listOfColumns { get; set; }
        private Packet previousPacket = null;
        public void startParsing(string[] filePaths)
        {
            if (filesExistAndMatch(filePaths))
            {
                mainRecording = new Recording();

                readFile(filePaths);

                fillDataGrid();
            }
            else
            {
                Console.WriteLine("Error reading file(s) - please try again");
            }
        }

        public bool filesExistAndMatch(string[] filePaths)
        {
            string[] startTimes = new string[filePaths.Length];

            for (int i = 0; i < filePaths.Length; i++)
            {
                Console.WriteLine("Checking file " + (i + 1));

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

            return timeStampsMatch(startTimes);
        }

        public bool timeStampsMatch(string[] startTimes)
        {
            bool matchingStamps = true;
            int counter = 0;

            while (matchingStamps && counter < (startTimes.Length - 1))
            {
                matchingStamps = startTimes[counter].Equals(startTimes[counter + 1]);
                counter++;
            }

            return matchingStamps;
        }

        public void readFile(string[] filePaths)
        {
            foreach (string fileName in filePaths)
            {
                Port currentPort = new Port();

                StreamReader streamReader = new StreamReader(fileName);

                currentPort.setStartTime(DateTime.ParseExact(streamReader.ReadLine(), "dd-MM-yyyy HH:mm:ss.fff", null));
                currentPort.setPortNumber(int.Parse(streamReader.ReadLine()));

                streamReader.ReadLine();

                while (streamReader.Peek() != -1)
                {
                    Packet currentPacket = new Packet();

                    string timeStamp = streamReader.ReadLine();
                    Console.WriteLine("time: " + timeStamp);

                    currentPacket.setTimeStamp(DateTime.ParseExact(timeStamp, "dd-MM-yyyy HH:mm:ss.fff", null));
                    Console.WriteLine("\t" + currentPacket.getTimestamp());

                    string packetType = streamReader.ReadLine();
                    if (packetType == null)
                    {
                        break;
                    }
                    Console.WriteLine(packetType);
                    currentPacket.setPacketType(char.Parse(packetType));
                    if (streamReader.Peek() == -1 || currentPacket.getPacketType().Equals("E"))
                    {
                        Console.WriteLine("peek " + streamReader.Peek());
                        break;
                    }
                    string cargo = streamReader.ReadLine();
                    currentPacket.setDataArray(cargo.Split(' '));

                    string packetMarkerType = streamReader.ReadLine();

                    if (packetMarkerType != "")
                    {
                        currentPacket.setPacketMarkerType(packetMarkerType);
                    }

                    if (currentPacket.getPacketType().Equals('E'))
                    {
                        currentPacket.setErrorType(currentPacket.getDataArray()[0]);
                    }
                    else if (currentPacket.getPacketMarkerType().Equals("None"))
                    {
                        currentPacket.setErrorType("None");
                    }
                    else if (currentPacket.getPacketMarkerType().Equals("EEP"))
                    {
                        currentPacket.setErrorType("EEP");
                    }
                    else if (previousPacket != null && Enumerable.SequenceEqual(currentPacket.dataArray, previousPacket.dataArray))
                    {
                        currentPacket.setErrorType("babbling");
                    }
                    else
                    {
                        cargo = trimPathAddress(cargo);
                        currentPacket.setProtocol(getProtocol(cargo));
                        if (currentPacket.getProtocol().Equals("RMAP"))
                        {
                            RMAP rmap = new RMAP();
                            currentPacket.transactionID = rmap.getTransactionID(cargo);
                            if (new CRC8().Check(cargo) != 0)
                            {
                                currentPacket.setErrorType("CRC");
                            }
                        }
                        else if(previousPacket != null && currentPacket.transactionID < previousPacket.transactionID)
                        {
                            currentPacket.setErrorType("OutOfSequence");
                        }
                    }
                    previousPacket = currentPacket;
                    currentPort.addPacketToList(currentPacket);

                    //quick fix for exceptions encountered on test 6 link 5, caused by a line read where the error "packets" have 1 less line
                    if (cargo.Equals("Parity") || cargo.Equals("Disconnect"))
                    {
                    }
                    else
                    {
                        streamReader.ReadLine();
                    }
                }
                streamReader.Close();

                currentPort.calcTotalValues();

                mainRecording.addPort(currentPort);
            }
            mainRecording.calculateTotals();
        }

        //I don't understand this method at all so couldn't rewrite with the same naming conventions
        public static string trimPathAddress(string cargo)
        {
            string[] characters = cargo.Split(' ');
            Console.WriteLine(cargo);
            byte[] characterBytes = characters.Select(s => Convert.ToByte(s, 16)).ToArray();
            List<string> charBytes = new List<string>(characters);
            int index = 0;
            while (characterBytes[index] < 32)
            {
                charBytes.RemoveAt(0);
                index++;
            }
            return String.Join(" ", charBytes.ToArray());
        }

        private static string getProtocol(string cargo)
        {
            cargo = trimPathAddress(cargo);
            string[] characters = cargo.Split(' ');
            byte[] characterBytes = characters.Select(s => Convert.ToByte(s, 16)).ToArray();
            int protocolNumber = characterBytes[1];

            switch (protocolNumber)
            {
                case 1:
                    return "RMAP";

                case 250:
                    return "CUSTOM";

                default:
                    return "UNACCOUNTED";
            }
        }

        public void fillDataGrid()
        {
            string dateTimeFormat = "dd-MM-yyyy HH:mm:ss.fff";
            DateTime startTime = mainRecording.getPort(0).getStartTime();
            DateTime timeOfLastPacket = mainRecording.getPort(0).getPacket(mainRecording.getPort(0).getTotalPackets() - 1).getTimestamp();


            foreach (Port currentPort in mainRecording.getPorts())
            {
                DateTime currentLastPacket = currentPort.getPacket(currentPort.getPackets().Count - 1).getTimestamp();
                DateTime currentFirstPacket = currentPort.packets[0].timestamp;

                Console.WriteLine(currentLastPacket + "\t" + timeOfLastPacket);

                if (DateTime.Compare(currentLastPacket, timeOfLastPacket) > 0)
                {
                    timeOfLastPacket = currentLastPacket;
                }
                if (DateTime.Compare(currentFirstPacket, startTime) > 0)
                {
                    startTime = currentFirstPacket;
                }
            }

            double numberOfColumns = (timeOfLastPacket - startTime).TotalMilliseconds;
            numberOfColumns += 2;

            listOfColumns = new List<GridColumn>();
            DateTime currentTime = startTime;


            Console.WriteLine("Number of Columns: " + numberOfColumns);

            for (int i = 0; i < numberOfColumns; i++)
            {
                string currentTimeStamp = currentTime.ToString(dateTimeFormat);

                GridColumn currentGridColumn = new GridColumn();

                currentGridColumn.setTime(currentTimeStamp);

                listOfColumns.Add(currentGridColumn);
                currentTime = currentTime.AddMilliseconds(1);
            }

            Console.WriteLine("Number of Columns: " + numberOfColumns);
            Console.WriteLine("StartTime: " + startTime);
            Console.WriteLine("End Time: " + timeOfLastPacket);

            for (int portCounter = 0; portCounter < mainRecording.getPorts().Count; portCounter++)
            {
                Port portToCheck = mainRecording.getPort(portCounter);

                int currentPortNumber = portToCheck.portNumber;

                Console.WriteLine("I'm in port " + (portCounter + 1));

                int timeStampCounter = 0;

                for (int packetCounter = 0; packetCounter < portToCheck.getPackets().Count; packetCounter++)
                {
                    Packet packetToCheck = portToCheck.getPacket(packetCounter);

                    int indexInGrid = 0;
                    bool found = false;

                    while (found == false && timeStampCounter < listOfColumns.Count)
                    {
                        found = (listOfColumns[timeStampCounter].getTime().Equals(packetToCheck.getTimestamp().ToString(dateTimeFormat), StringComparison.Ordinal));
                        indexInGrid = timeStampCounter;
                        timeStampCounter++;
                    }

                    if (!found)
                    {
                        timeStampCounter = 0;

                        while (found == false && timeStampCounter < listOfColumns.Count)
                        {
                            found = (listOfColumns[timeStampCounter].getTime().Equals(packetToCheck.getTimestamp().ToString(dateTimeFormat), StringComparison.Ordinal));
                            indexInGrid = timeStampCounter;
                            timeStampCounter++;
                        }

                        timeStampCounter--;

                        Console.WriteLine("This fucked up at port " + (portCounter + 1) + ", packet " + packetCounter + ", timeStampCounter " + timeStampCounter + ", and timestamp " + packetToCheck.getTimestamp().ToString(dateTimeFormat));
                    }

                    Console.WriteLine("******* \t" + listOfColumns[timeStampCounter].getTime() + packetToCheck.getErrorType() + portCounter);

                    string toDisplay = "";

                    if (packetToCheck.getErrorType() == null)
                    {
                        toDisplay = "Packet";
                    }
                    else
                    {
                        toDisplay = packetToCheck.getErrorType();
                    }

                    timeStampCounter = indexInGrid;

                    switch (currentPortNumber)
                    {
                        case 1:
                            listOfColumns[timeStampCounter].setPort1(toDisplay);
                            break;

                        case 2:
                            listOfColumns[timeStampCounter].setPort2(toDisplay);
                            break;

                        case 3:
                            listOfColumns[timeStampCounter].setPort3(toDisplay);
                            break;

                        case 4:
                            listOfColumns[timeStampCounter].setPort4(toDisplay);
                            break;

                        case 5:
                            listOfColumns[timeStampCounter].setPort5(toDisplay);
                            break;

                        case 6:
                            listOfColumns[timeStampCounter].setPort6(toDisplay);
                            break;

                        case 7:
                            listOfColumns[timeStampCounter].setPort7(toDisplay);
                            break;

                        case 8:
                            listOfColumns[timeStampCounter].setPort8(toDisplay);
                            break;
                    }
                }
            }
        }

        public List<GridColumn> getListOfColumns()
        {
            return listOfColumns;
        }
    }
}
