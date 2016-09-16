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

            foreach (Packet current in p)
            {
                dataSets.Add(current.getData().getTheData());
            }

            dataSetLength = dataSets[0].Count();




            List<int[]> convertedData = getDecValues(dataSets, dataSetLength);


            //for (int i = 0; i < convertedData.Count(); i++)
           // {
             //   for (int j = 0; j < dataSetLength; j++)
             //   {
              //      Console.Write(" " + convertedData[i][j]);
              //  }
             //   Console.WriteLine("");
          //  }


            int result = getTheSequenceIndex(convertedData);
            Console.WriteLine("");
            Console.WriteLine(result);
            Console.WriteLine("");
            return result;

        }


        public int getTheSequenceIndex(List<int[]> theData)
        {


            List<int> possibleIndex = new List<int>();
            int sequenceIndex;


            int[] prev = theData[0];
            int[] curr = theData[1];

            //Compare first two lines of data to find potential sequence number index
            for (int i = 0; i < theData[0].Count(); i++)
            {

                if (curr[i] == (prev[i] + 1))
                {
                    possibleIndex.Add(i);
                }

            }



            for (int i = 2; i < theData.Count(); i++)
            {
                curr = theData[i];
                prev = theData[i - 1];

                for (int x = 0; x < possibleIndex.Count(); x++)
                {
                    int currIndex = possibleIndex[x];
                    if (curr[currIndex] == (prev[currIndex] + 1))
                    {
                        //Still could be the index
                        Console.WriteLine(" === " + currIndex + " === " + curr[currIndex]);
                    }
                    else
                    {
                        possibleIndex.Remove(currIndex);
                    }

                }

            }

            if (possibleIndex.Count() == 1)
            {
                return possibleIndex[0];

            }
            else {
                return -1;
            }

        }


        public List<int[]> getDecValues(List<string[]> dataSets, int dataSetLength)
        {

            List<int[]> converted = new List<int[]>();

            foreach (string[] currSet in dataSets)
            {
                List<int> intList = new List<int>();
                foreach (string currString in currSet)
                {
                    int x = Convert.ToInt32(currString, 16);
                    //Console.WriteLine(" - " + currString + " - " + x + " - ");
                    intList.Add(x);
                }
                converted.Add(intList.ToArray());
            }
            Console.WriteLine("");

            //for (int i = 0; i < converted.Count(); i++)
            //{
            //    for (int j = 0; j < dataSetLength; j++)
            //    {
                    //Console.Write(" " + converted[i][j]);
            //    }
            //    Console.WriteLine("");
           // }
            Console.WriteLine("\n DONE \n");
            return converted;
        }
    }


}
