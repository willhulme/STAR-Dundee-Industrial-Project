using Star_Dundee_WPF.Models;
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
        int currentPortNumber;
        public List<GridColumn> listOfColumns { get; set; }
        private Packet previousPacket = null;
        private List<DateTime> columns { get; set; }
        public bool startParsing(string[] filePaths)
        {
            if (filesExistAndMatch(filePaths))
            {
                mainRecording = new Recording();

                readFile(filePaths);


                //fillDataGrid();
                fillDataGridSimply();

               
                return true;

            }
            else
            {
                Console.WriteLine("Error reading file(s) - please try again");
                return false;
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
            RateCalculator2 RC2 = new RateCalculator2();
            columns = new List<DateTime>();
            List<Tuple<DateTime,int>> portusedTime = new List<Tuple<DateTime,int>>();
            foreach (string fileName in filePaths)
            {
                Port currentPort = new Port();
                previousPacket = null;
                StreamReader streamReader = new StreamReader(fileName);

                currentPort.setStartTime(DateTime.ParseExact(streamReader.ReadLine(), "dd-MM-yyyy HH:mm:ss.fff", null));
                currentPort.setPortNumber(int.Parse(streamReader.ReadLine()));
                //use this to add to the packet when getting built
                currentPortNumber = currentPort.portNumber;
                streamReader.ReadLine();

                while (streamReader.Peek() != -1)
                {
                    Packet currentPacket = new Packet();
                    //set the port number in the packet
                    currentPacket.portIndex = currentPortNumber;

                    string timeStamp = streamReader.ReadLine();
                    //Console.WriteLine("time: " + timeStamp);

                    currentPacket.setTimeStamp(DateTime.ParseExact(timeStamp, "dd-MM-yyyy HH:mm:ss.fff", null));
                    //Console.WriteLine("\t" + currentPacket.getTimestamp());
                    Tuple<DateTime, int> timecheck = new Tuple<DateTime, int>(currentPacket.timestamp, currentPort.portNumber);
                    columns.Add(currentPacket.timestamp);
                    
                    string packetType = streamReader.ReadLine();
                    if (packetType == null)
                    {
                        break;
                    }
                    //Console.WriteLine(packetType);
                    currentPacket.setPacketType(char.Parse(packetType));
                    if (streamReader.Peek() == -1 || currentPacket.getPacketType().Equals("E"))
                    {
                        //Console.WriteLine("peek " + streamReader.Peek());
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
                        currentPacket.setErrorType("BabblingIdiot");
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
                            else if (previousPacket != null && currentPacket.transactionID < previousPacket.transactionID)
                            {
                                if (currentPacket.transactionID == 0 && previousPacket.transactionID == 65535)
                                {
                                    //wombo combo
                                }
                                else
                                {
                                    currentPacket.setErrorType("Sequence");
                                }

                            }
                        }
                        else if (currentPacket.protocol.Equals("CUSTOM"))
                        {
                            byte newData = getCustomProtocolTransactionID(cargo);
                            currentPacket.transactionID = newData;
                            if (previousPacket != null && currentPacket.transactionID < previousPacket.transactionID)
                            {
                                if (currentPacket.transactionID == 0 && previousPacket.transactionID == 65535)
                                {
                                    //wombo combo
                                }
                                else
                                {
                                    currentPort.packets[(currentPort.packets.Count - 1)].errorType = "Sequence";
                                    currentPacket.setErrorType("Sequence");
                                }
                            }
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
                currentPort.dataRateTime = RC2.CalculateDataRate(currentPort.packets);

                currentPort.packetRate = Math.Round(RC2.CalculatePacketRate(currentPort.packets), 4);
                currentPort.errorRate = Math.Round(RC2.CalculateErrorRate(currentPort), 4);

                mainRecording.addPort(currentPort);
            }
            columns.Sort((a, b) => a.CompareTo(b));
            mainRecording.calculateTotals();
        }

        private byte getCustomProtocolTransactionID(string cargo)
        {
            cargo = trimPathAddress(cargo);
            string[] characters = cargo.Split(' ');
            byte[] characterBytes = characters.Select(s => Convert.ToByte(s, 16)).ToArray();
            return characterBytes[2];
        }

        //I don't understand this method at all so couldn't rewrite with the same naming conventions
        public static string trimPathAddress(string cargo)
        {
            string[] characters = cargo.Split(' ');
            //Console.WriteLine(cargo);
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

        public void fillDataGridSimply()
        {
            //fill our overview with the needed information
            string dateTimeFormat = "dd-MM-yyyy HH:mm:ss.fff";
            listOfColumns = new List<GridColumn>();
            
            int gridIndex = 0;

            //Search all ports
            for (int portCounter = 0; portCounter < mainRecording.ports.Count; portCounter++)
            {
                //for every packet
                for (int packetCounter = 0; packetCounter < mainRecording.ports[portCounter].packets.Count; packetCounter++)
                {
                    GridColumn currentGridColumn = new GridColumn();
                    currentGridColumn.time = mainRecording.ports[portCounter].packets[packetCounter].timestamp.ToString(dateTimeFormat);

                    bool exists = false;
                    int columnIndex = 0;
                    //check if column already exists and get its index
                    for (int columnCounter = 0; columnCounter < listOfColumns.Count; columnCounter++)
                    {
                        if (currentGridColumn.time == listOfColumns[columnCounter].time)
                        {
                            exists = true;
                            columnIndex = columnCounter;
                            break;
                        }
                    }

                    //Build up current column
                    currentGridColumn.index = gridIndex.ToString();
                    

                    

                    //check what port the error is on 
                    int packetErrorPort = mainRecording.ports[portCounter].packets[packetCounter].portIndex;

                    //Get the errorMessage
                    string toDisplay = "";

                    if (mainRecording.ports[portCounter].packets[packetCounter].getErrorType() == null)
                    {
                        toDisplay = "Packet";
                    }
                    else
                    {
                        toDisplay = mainRecording.ports[portCounter].packets[packetCounter].getErrorType();
                    }

                    //Add the error to the correct port on the grid
                    switch (packetErrorPort)
                    {
                        case 1:
                            currentGridColumn.port1 = toDisplay;
                            break;
                        case 2:
                            currentGridColumn.port2 = toDisplay;
                            break;
                        case 3:
                            currentGridColumn.port3 = toDisplay;
                            break;
                        case 4:
                            currentGridColumn.port4 = toDisplay;
                            break;
                        case 5:
                            currentGridColumn.port5 = toDisplay;
                            break;
                        case 6:
                            currentGridColumn.port6 = toDisplay;
                            break;
                        case 7:
                            currentGridColumn.port7 = toDisplay;
                            break;
                        case 8:
                            currentGridColumn.port8 = toDisplay;
                            break;
                    }

                   

                    if (!exists)
                    {
                        //add column to the grid                            
                        GridColumn newGridColumn = new GridColumn();
                        newGridColumn = currentGridColumn;
                        listOfColumns.Add(newGridColumn);
                    }
                    else
                    {
                        //write to column instead
                        switch (packetErrorPort)
                        {
                            case 1:
                                listOfColumns[columnIndex].port1 = currentGridColumn.port1;
                                break;
                            case 2:
                                listOfColumns[columnIndex].port2 = currentGridColumn.port2;
                                break;
                            case 3:
                                listOfColumns[columnIndex].port3 = currentGridColumn.port3;
                                break;
                            case 4:
                                listOfColumns[columnIndex].port4 = currentGridColumn.port4;
                                break;
                            case 5:
                                listOfColumns[columnIndex].port5 = currentGridColumn.port5;
                                break;
                            case 6:
                                listOfColumns[columnIndex].port6 = currentGridColumn.port6;
                                break;
                            case 7:
                                listOfColumns[columnIndex].port7 = currentGridColumn.port7;
                                break;
                            case 8:
                                listOfColumns[columnIndex].port8 = currentGridColumn.port8;
                                break;
                        }

                       
                    }
                    gridIndex++;

                }
            }
        }
        public void fillDataGrid()
        {
            string dateTimeFormat = "dd-MM-yyyy HH:mm:ss.fff";
            DateTime startTime = mainRecording.getPort(0).getStartTime();
            DateTime timeOfLastPacket = mainRecording.getPort(0).getPacket(mainRecording.getPort(0).getTotalPackets() - 1).getTimestamp();

            DateTime currentLastPacket;
            DateTime currentFirstPacket;
            DateTime earliestPacket = mainRecording.getPorts()[0].packets[0].timestamp;

            foreach (Port currentPort in mainRecording.getPorts())
            {
                currentLastPacket = currentPort.getPacket(currentPort.getPackets().Count - 1).getTimestamp();
                currentFirstPacket = currentPort.packets[0].timestamp;

                //Console.WriteLine(currentLastPacket + "\t" + timeOfLastPacket);

                if (DateTime.Compare(currentLastPacket, timeOfLastPacket) > 0)
                {
                    timeOfLastPacket = currentLastPacket;
                }
                if (DateTime.Compare(currentFirstPacket, earliestPacket) < 0)
                {
                    earliestPacket = currentFirstPacket;
                }
            }

            startTime = earliestPacket;

            //double numberOfColumns = (timeOfLastPacket - startTime).TotalMilliseconds;
            double numberOfColumns = columns.Count;
            //numberOfColumns += 2;

            listOfColumns = new List<GridColumn>();
            //DateTime currentTime = startTime;
            DateTime currentTime = columns[0];

            //Console.WriteLine("Number of Columns: " + numberOfColumns);

            for (int i = 0; i < numberOfColumns; i++)
            {
                string currentTimeStamp = currentTime.ToString(dateTimeFormat);

                GridColumn currentGridColumn = new GridColumn();

                currentGridColumn.setTime(currentTimeStamp);
                currentGridColumn.index = i.ToString();
                listOfColumns.Add(currentGridColumn);
                //currentTime = currentTime.AddMilliseconds(1);
                currentTime = columns[i];
            }

            Console.WriteLine("Number of Columns: " + numberOfColumns);
            Console.WriteLine("StartTime: " + startTime);
            Console.WriteLine("End Time: " + timeOfLastPacket);

            for (int portCounter = 0; portCounter < mainRecording.getPorts().Count; portCounter++)
            {
                Port portToCheck = mainRecording.getPort(portCounter);

                int currentPortNumber = portToCheck.portNumber;

                //Console.WriteLine("I'm in port " + (portCounter + 1));

                int timeStampCounter = 0;

                for (int packetCounter = 0; packetCounter < portToCheck.getPackets().Count; packetCounter++)
                {
                    Packet packetToCheck = portToCheck.getPacket(packetCounter);
                    

                    int indexInGrid = 0;
                    bool found = false;

                    while (found == false && timeStampCounter < listOfColumns.Count)
                    {

                        if (packetToCheck.timestamp.ToString(dateTimeFormat) == listOfColumns[timeStampCounter].time)
                        {
                            //indexInGrid++;
                            #region switchcase
                            //check if a packet is already at this time and if so add a new column to the grid at the correct bit
                            switch (currentPortNumber)
                            {
                                case 1:
                                    if (listOfColumns[timeStampCounter].port1 != null)
                                    {
                                        GridColumn newColumn = new GridColumn();
                                        newColumn.setTime(packetToCheck.timestamp.ToString(dateTimeFormat));
                                        if (listOfColumns[timeStampCounter].time == listOfColumns[timeStampCounter + 1].time && listOfColumns[timeStampCounter + 1].port1 != null)
                                        {
                                            timeStampCounter++;
                                            newColumn.index = timeStampCounter.ToString();
                                            listOfColumns.Insert((timeStampCounter), newColumn);
                                        }
                                        else
                                        {
                                            timeStampCounter++;
                                            break;
                                        }
                                    }
                                    break;

                                case 2:
                                    if (listOfColumns[timeStampCounter].port2 != null)
                                    {
                                        GridColumn newColumn = new GridColumn();
                                        newColumn.setTime(packetToCheck.timestamp.ToString(dateTimeFormat));
                                        if (listOfColumns[timeStampCounter].time == listOfColumns[timeStampCounter + 1].time && listOfColumns[timeStampCounter + 1].port2 != null)
                                        {
                                            timeStampCounter++;
                                            newColumn.index = timeStampCounter.ToString();
                                            listOfColumns.Insert((timeStampCounter), newColumn);
                                        }
                                        else
                                        {
                                            timeStampCounter++;
                                            break;
                                        }
                                        
                                    }

                                    break;
                                case 3:
                                    if (listOfColumns[timeStampCounter].port3 != null)
                                    {
                                        GridColumn newColumn = new GridColumn();
                                        newColumn.setTime(packetToCheck.timestamp.ToString(dateTimeFormat));
                                        if (listOfColumns[timeStampCounter].time == listOfColumns[timeStampCounter + 1].time && listOfColumns[timeStampCounter + 1].port3 != null)
                                        {
                                            timeStampCounter++;
                                            newColumn.index = timeStampCounter.ToString();
                                            listOfColumns.Insert((timeStampCounter), newColumn);
                                        }
                                        else
                                        {
                                            timeStampCounter++;
                                            break;
                                        }
                                    }
                                    break;

                                case 4:
                                    if (listOfColumns[timeStampCounter].port4 != null)
                                    {
                                        GridColumn newColumn = new GridColumn();
                                        newColumn.setTime(packetToCheck.timestamp.ToString(dateTimeFormat));
                                        if (listOfColumns[timeStampCounter].time == listOfColumns[timeStampCounter + 1].time && listOfColumns[timeStampCounter + 1].port4 != null)
                                        {
                                            timeStampCounter++;
                                            newColumn.index = timeStampCounter.ToString();
                                            listOfColumns.Insert((timeStampCounter), newColumn);
                                        }
                                        else
                                        {
                                            timeStampCounter++;
                                            break;
                                        }
                                    }
                                    break;

                                case 5:
                                    if (listOfColumns[timeStampCounter].port5 != null)
                                    {
                                        GridColumn newColumn = new GridColumn();
                                        newColumn.setTime(packetToCheck.timestamp.ToString(dateTimeFormat));
                                        if (listOfColumns[timeStampCounter].time == listOfColumns[timeStampCounter + 1].time && listOfColumns[timeStampCounter + 1].port5 != null)
                                        {
                                            timeStampCounter++;
                                            newColumn.index = timeStampCounter.ToString();
                                            listOfColumns.Insert((timeStampCounter), newColumn);
                                        }
                                        else
                                        {
                                            timeStampCounter++;
                                            break;
                                        }
                                    }
                                    break;

                                case 6:
                                    if (listOfColumns[timeStampCounter].port6 != null)
                                    {
                                        GridColumn newColumn = new GridColumn();
                                        newColumn.setTime(packetToCheck.timestamp.ToString(dateTimeFormat));
                                        if (listOfColumns[timeStampCounter].time == listOfColumns[timeStampCounter + 1].time && listOfColumns[timeStampCounter + 1].port6 != null)
                                        {
                                            timeStampCounter++;
                                            newColumn.index = timeStampCounter.ToString();
                                            listOfColumns.Insert((timeStampCounter), newColumn);
                                        }
                                        else
                                        {
                                            timeStampCounter++;
                                            break;
                                        }
                                    }
                                    break;

                                case 7:
                                    if (listOfColumns[timeStampCounter].port7 != null)
                                    {
                                        GridColumn newColumn = new GridColumn();
                                        newColumn.setTime(packetToCheck.timestamp.ToString(dateTimeFormat));
                                        if (listOfColumns[timeStampCounter].time == listOfColumns[timeStampCounter + 1].time && listOfColumns[timeStampCounter + 1].port7 != null)
                                        {
                                            timeStampCounter++;
                                            newColumn.index = timeStampCounter.ToString();
                                            listOfColumns.Insert((timeStampCounter), newColumn);
                                        }
                                        else
                                        {
                                            timeStampCounter++;
                                            break;
                                        }
                                    }
                                    break;

                                case 8:
                                    if (listOfColumns[timeStampCounter].port8 != null)
                                    {
                                        GridColumn newColumn = new GridColumn();
                                        newColumn.setTime(packetToCheck.timestamp.ToString(dateTimeFormat));
                                        if (listOfColumns[timeStampCounter].time == listOfColumns[timeStampCounter + 1].time && listOfColumns[timeStampCounter + 1].port8 != null)
                                        {
                                            timeStampCounter++;
                                            newColumn.index = timeStampCounter.ToString();
                                            listOfColumns.Insert((timeStampCounter), newColumn);
                                        }
                                        else
                                        {
                                            timeStampCounter++;
                                            break;
                                        }

                                    }
                                    break;

                            }
                            #endregion switchcase


                        }




                        found = (listOfColumns[timeStampCounter].getTime().Equals(packetToCheck.getTimestamp().ToString(dateTimeFormat), StringComparison.Ordinal));
                        indexInGrid = timeStampCounter;
                        timeStampCounter++;
                        //if the current packet time is at the current list index
                        
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

                            //Console.WriteLine("This fucked up at port " + (portCounter + 1) + ", packet " + packetCounter + ", timeStampCounter " + timeStampCounter + ", and timestamp " + packetToCheck.getTimestamp().ToString(dateTimeFormat));
                    }

                        //Console.WriteLine("******* \t" + listOfColumns[timeStampCounter].getTime() + packetToCheck.getErrorType() + portCounter);

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
