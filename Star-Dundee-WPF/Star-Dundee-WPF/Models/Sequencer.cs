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
            List<int> possibleIndex = getPossibleIndexList(theData, curr, prev);
            possibleIndex = parseForSequence(theData, curr, prev, possibleIndex, p);


            //If there is only one possible sequence index left, return it as the index
            if (possibleIndex.Count() == 1)
            {
                return possibleIndex[0];

            }
            else if (possibleIndex.Count() == 0)
            {
                //Return if there is less than one possibility
                return -1;
            }
            else
            {
                //Return
                return -2;
            }

        }

        public List<int> getPossibleIndexList(List<int[]> theData, int[] curr, int[] prev)
        {

            List<int> possibleIndex = new List<int>();

            //Compare only first two lines of data to find potential sequence number index
            for (int i = 0; i < theData[0].Count(); i++)
            {
                //Check if current is exactly 1 more than previous
                if (curr[i] == (prev[i] + 1))
                {
                    //If so add to list for possible index
                    possibleIndex.Add(i);
                }
            }

            return possibleIndex;

        }

        public List<int> parseForSequence(List<int[]> theData, int[] curr, int[] prev, List<int> possibleIndex, List<Packet> p)
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
                if (i == 512) {
                    Console.WriteLine("");
                }
                //For every possible index identified
                for (int x = 0; x < possibleIndex.Count(); x++)
                {
                    int currIndex = possibleIndex[x];
                    //Check next line for increment

                    //TODO - Matt
                    //
                    //Out of range exception breaks this here if error and shorter data string than expected is found
                    //File Test 6, link 5

                    //Sequence number issuse when error occurs on packet ff & next 00 compares to fe from 2 packets before

                    if (packetsSkipped > 0) {

                        prev = theData[i - (1 + packetsSkipped)];
                    }

                    try
                    {
                        if (curr[currIndex] == (prev[currIndex] + 1 + packetsSkipped))
                        {
                            //Still could be the index
                            // Console.WriteLine(" === " + currIndex + " === " + curr[currIndex]);
                            packetsSkipped = 0;
                            Console.WriteLine("1");
                        }
                        //allow for going from ff(255) back to 00 as valid   test 5/link1
                        else if (prev[currIndex] == 255 && curr[currIndex] == 00)
                        {
                            packetsSkipped = 0;
                            Console.WriteLine("2");
                        }

                        else if (packetsSkipped>0 && (prev[currIndex]+packetsSkipped) == 255 && ((curr[currIndex]-packetsSkipped) == 00 || curr[currIndex]==00)) {
                            packetsSkipped = 0;
                        }

                        else if (curr[currIndex] == (prev[currIndex]))
                        {
                            Console.WriteLine("3");

                            //Compare actual strings
                            if (curr.SequenceEqual(prev))
                            {
                                if (idiotCount == 0)
                                {
                                    Console.WriteLine("4");
                                    firstIdiotIndex = i - 1;

                                }
                                if (idiotCount >= 4)
                                {
                                    if (!idiotsSet)
                                    {
                                        for (int y = firstIdiotIndex; y < i; y++)
                                        {
                                            Console.WriteLine("5");
                                            p[y].setError(true, "babbling");
                                        }
                                    }
                                    p[i].setError(true, "babbling");

                                }


                                idiotCount++;
                            }
                            else {
                                Console.WriteLine("6");
                                possibleIndex.Remove(currIndex);
                                packetsSkipped = 0;

                            }

                        }
                        else
                        {
                            //Check if is 1 more than expected and then if next packet is error then sequence error happens here
                            if (curr[currIndex] == (prev[currIndex] + 2) && p[i + 1].getErrorStatus())
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
