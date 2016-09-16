using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Sequencer
    {
        public void findSequence(List<Packet> p)
        {

            List<string[]> dataSets = new List<string[]>();
            int dataSetLength;

            foreach (Packet current in p)
            {
                dataSets.Add(current.getData().getTheData());
            }

            dataSetLength = dataSets[0].Count();



            int sequenceIndex;
            List<int[]> convertedData = getDecValues(dataSets, dataSetLength);


            for (int i = 0; i < convertedData.Count(); i++)
            {
                for (int j = 0; j < dataSetLength; j++)
                {
                    Console.Write(" " + convertedData[i][j]);
                }
                Console.WriteLine("");
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
                    Console.WriteLine(" - " + currString + " - " + x + " - ");
                    intList.Add(x);
                }
                converted.Add(intList.ToArray());
            }
            Console.WriteLine("");

            for (int i = 0; i < converted.Count(); i++)
            {
                for (int j = 0; j < dataSetLength; j++)
                {
                    Console.Write(" " + converted[i][j]);
                }
                Console.WriteLine("");
            }
            Console.WriteLine("\n DONE \n");
            return converted;
        }

    }
}
