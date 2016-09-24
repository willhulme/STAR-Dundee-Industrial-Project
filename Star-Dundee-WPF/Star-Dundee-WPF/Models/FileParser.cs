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

        private Port currentPort;

        public void startParsing(string[] filePaths)
        {
            if (filesExistAndMatch(filePaths))
            {
                foreach(string currentFile in filePaths)
                {
                    Console.WriteLine("Reading File............");
                    string[] fileInformation = System.IO.File.ReadAllLines(currentFile);
                    Console.WriteLine("File Reading Complete");

                    string[] packetsData = splitFileIntoPackets(fileInformation);
                }
            }
        }

        public bool filesExistAndMatch(string[] filePaths)
        {
            string[] startTimes = new string[filePaths.Length];

            for(int i = 0; i < filePaths.Length; i++)
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

            while(matchingStamps && counter < startTimes.Length)
            {
                matchingStamps = startTimes[counter].Equals(startTimes[counter + 1]);
                counter++;
            }

            return matchingStamps;
        }

        public string[] splitFileIntoPackets(string[] linesInFile)
        {
            DateTime startTimeStamp = new DateTime();
            DateTime endTimeStamp = new DateTime();

            startTimeStamp = DateTime.Parse(linesInFile[0]);
            int portNumber = Convert.ToInt32(linesInFile[1]);
            endTimeStamp = DateTime.Parse(linesInFile[linesInFile.Length - 1]);

            currentPort = new Port(portNumber, startTimeStamp, endTimeStamp);

            List<string> packetsInFile = new List<string>();
            string currentPacket = "";

            for(int i = 2; i < linesInFile.Length -1; i++)
            {
                if(linesInFile[[i].equals)
            }

        }

    }
}
