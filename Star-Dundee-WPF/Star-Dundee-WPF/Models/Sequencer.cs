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
            int result = getTheSequenceIndex(convertedData);
            return result;

        }


        public int getTheSequenceIndex(List<int[]> theData)
        {
            //Create list to store possible index values
            List<int> possibleIndex = new List<int>();

            //Set initial current and previous values for data comparison
            int[] prev = theData[0];
            int[] curr = theData[1];

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


            //Loop through the remaining lines of data
            for (int i = 2; i < theData.Count(); i++)
            {
                //Set current and previous lines for comparing
                curr = theData[i];
                prev = theData[i - 1];

                //For every possible index identified
                for (int x = 0; x < possibleIndex.Count(); x++)
                {
                    int currIndex = possibleIndex[x];
                    //Check next line for increment
                    if (curr[currIndex] == (prev[currIndex] + 1))
                    {
                        //Still could be the index
                        Console.WriteLine(" === " + currIndex + " === " + curr[currIndex]);
                    }
                    else
                    {
                        //Incremented value not found so remove from list of possibilities
                        possibleIndex.Remove(currIndex);
                    }
                }
            }

            //If there is only one possible sequence index left, return it as the index
            if (possibleIndex.Count() == 1)
            {
                return possibleIndex[0];

            }
            else {
                //Return if there is less or more than one possibility
                return -1;
            }

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
