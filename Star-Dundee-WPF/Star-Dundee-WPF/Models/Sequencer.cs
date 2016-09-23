using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Sequencer
    {
        public int findSequence(List<Packet> p)
        {
            List<string[]> dataSets = new List<string[]>();
            int dataSetLength;

            //Loop for each packet in list
            foreach (Packet current in p)
            {
                //Add the data section to list of data sets
                dataSets.Add(current.getData().getTheData());
            }

            //Get length of data string
            dataSetLength = dataSets[0].Count();

            //Call to function to get hex values converted to decimal
            List<int[]> convertedData = getDecValues(dataSets, dataSetLength);

            //Function call to get the sequence index
            int result = getTheSequenceIndex(convertedData, p);
            parseForAddress(convertedData, p);
            return result;
        }

        public int getTheSequenceIndex(List<int[]> theData, List<Packet> p)
        {
            //Set initial current and previous values for data comparison
            int[] prev = theData[0];
            int[] curr = theData[1];
            int[] next = theData[2];
            int repeatCount = 0;

            //Create list to store possible index values
            List<int> possibleIndex = getPossibleIndexList(theData, curr, prev, 1);
            if (possibleIndex.Count() > 0)
            {
                possibleIndex = parseForSequence(theData, curr, prev, possibleIndex, p, 1);
            }
            List<int> possibleIndexTwo = getPossibleIndexList(theData, curr, prev, 2);
            if (possibleIndexTwo.Count() > 0)
            {
                possibleIndexTwo = parseForSequence(theData, curr, prev, possibleIndexTwo, p, 2);
            }

            //If there is only one possible sequence index left, return it as the index
            if (possibleIndex.Count() == 1 && possibleIndexTwo.Count() == 0)
            {
                return possibleIndex[0];

            }
            else if (possibleIndex.Count() == 0 && possibleIndexTwo.Count() == 1)
            {
                //Return if there is less than one possibility
                return possibleIndexTwo[0];
            }
            else if (possibleIndex.Count() == 1 && possibleIndexTwo.Count() == 1)
            {
                //Return if there is less than one possibility
                return possibleIndex[0];
            }
            else
            {
                //Return
                return -1;
            }

        }
        public void parseForAddress(List<int[]> theData, List<Packet> p)
        {
            for (int i = 0;i<theData.Count();i++)

            {
                int[] curr = theData[i];
                string[] currDataSet = p[i].getData().getTheData();
                if (curr[0] < 32)
                {
                    //path addressing
                    string addString = "";
                    for (int j = 0; j < curr.Length; j++)
                    {
                        if (curr[j] != 254)
                        {
                            addString += (currDataSet[j] + " ");
                        }
                        else
                        {
                            addString += currDataSet[j];
                            p[i].getData().setAddress(addString);
                            p[i].getData().setProtocol(currDataSet[j + 1]);
                            break;
                        }
                    }
                }
                else if (curr[0] >= 32 && curr[0] <= 255)
                {
                    p[i].getData().setAddress(currDataSet[0]);
                    p[i].getData().setProtocol(currDataSet[1]);
                }
            }
        }

        public List<int> getPossibleIndexList(List<int[]> theData, int[] curr, int[] prev, int increment)
        {
            List<int> possibleIndex = new List<int>();

            //Compare only first two lines of data to find potential sequence number index
            for (int i = 0; i < theData[0].Count(); i++)
            {
                //Check if current is exactly 1 more than previous
                if (curr[i] == (prev[i] + increment))
                {
                    //If so add to list for possible index
                    possibleIndex.Add(i);
                }
            }
            return possibleIndex;
        }

        public List<int> getPossibleIndexListTwo(List<int[]> theData, int[] curr, int[] prev)
        {
            List<int> possibleIndex = new List<int>();

            //Compare only first two lines of data to find potential sequence number index
            for (int i = 0; i < theData[0].Count(); i++)
            {
                //Check if current is exactly 1 more than previous
                if (curr[i] == (prev[i] + 2))
                {
                    //If so add to list for possible index
                    possibleIndex.Add(i);
                }
            }
            return possibleIndex;
        }

        public List<int> parseForSequence(List<int[]> theData, int[] curr, int[] prev, List<int> possibleIndex, List<Packet> p, int incrementSize)
        {
            int packetsSkipped = 0;
            int idiotCount = 0;
            int firstIdiotIndex = 0;
            bool idiotsSet = false;

            //Loop through the remaining lines of data
            for (int i = 2; i < theData.Count(); i++)
            {
                //Set current and previous lines for comparing
                curr = theData[i];
                prev = theData[i - 1];
                //next = theData[i + 1];

                //For every possible index identified
                for (int x = 0; x < possibleIndex.Count(); x++)
                {
                    int currIndex = possibleIndex[x];
                    //Check next line for increment
                    if (packetsSkipped > 0)
                    {
                        prev = theData[i - (incrementSize + packetsSkipped)];
                    }
					
                    //Sequence number issuse when error occurs on packet ff & next 00 compares to fe from 2 packets 
                    try
                    {
                        if (curr[currIndex] == (prev[currIndex] + incrementSize + packetsSkipped))
                        {
                            //Still could be the index
                            // Console.WriteLine(" === " + currIndex + " === " + curr[currIndex]);
                            packetsSkipped = 0;
                        }
                        //allow for going from ff(255) back to 00 as valid   test 5/link1
                        else if (prev[currIndex] == 255 && curr[currIndex] == 00 && incrementSize == 1)
                        {
                            packetsSkipped = 0;
                        }

                        else if (prev[currIndex] == 254 && curr[currIndex] == 00 && incrementSize == 2)
                        {
                            packetsSkipped = 0;
                        }

                        else if (prev[currIndex] == 255 && curr[currIndex] == 01 && incrementSize == 2)
                        {
                            packetsSkipped = 0;
                        }

                        else if (packetsSkipped > 0 && (prev[currIndex] + packetsSkipped) == 255 && ((curr[currIndex] - packetsSkipped) == 00 || curr[currIndex] == 00))
                        {
                            packetsSkipped = 0;
                        }

                        else if (curr[currIndex] == (prev[currIndex]))
                        {
                            //Compare actual strings
                            if (curr.SequenceEqual(prev))
                            {
                                if (idiotCount == 0)
                                {
                                    firstIdiotIndex = i - 1;
                                }
                                if (idiotCount >= 4)
                                {
                                    if (!idiotsSet)
                                    {
                                        for (int y = firstIdiotIndex; y < i; y++)
                                        {
                                            p[y].setError(true, "babbling");
                                        }
                                    }
                                    p[i].setError(true, "babbling");
                                }
                                idiotCount++;
                            }
                            else {
                                possibleIndex.Remove(currIndex);
                                packetsSkipped = 0;
                            }
                        }
                        else
                        {
                            //Check if is 1 more than expected and then if next packet is error then sequence error happens here
                            if (curr[currIndex] == (prev[currIndex] + 2 * incrementSize) && p[i + 1].getErrorStatus())
                            {
                                //Still could be the index
                                //Console.WriteLine(" ++++ " + currIndex + " === " + curr[currIndex]);
                                packetsSkipped = 0;
                                p[i + 1].setError(true, "sequence");
                                //sequence error here
                            }
                            else {
                                if (!p[i].getErrorStatus())
                                {
                                    //Incremented value not found so remove from list of possibilities
                                    possibleIndex.Remove(currIndex);
                                    packetsSkipped = 0;
                                }
                            }
                        }
                    }
                    catch (IndexOutOfRangeException RE)
                    {
                        Console.WriteLine(RE.Message + "|| AT INDEX " + currIndex + " || AT LINE " + i + " || SKIPPED : " + packetsSkipped);
                        packetsSkipped++;
                    }
                }
            }
            return possibleIndex;
        }

        public List<int[]> getDecValues(List<string[]> dataSets, int dataSetLength)
        {
            List<int[]> converted = new List<int[]>();

            //Loop through each line of data in the recording
            foreach (string[] currSet in dataSets)
            {
                List<int> intList = new List<int>();
                //Loop through each string in the current line
                foreach (string currString in currSet)
                {
                    //Convert hex string into int value and store in list
                    int x = Convert.ToInt32(currString, 16);
                    intList.Add(x);
                }
                //Add converted line of data to overall list
                converted.Add(intList.ToArray());
            }

            //Return the list of decimal values
            return converted;
        }
    }


}
