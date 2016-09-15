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
            string fileName = "../../DataFiles/test1_link1.rec";

            if (System.IO.File.Exists(fileName))
            {

                string[] lineInFile = System.IO.File.ReadAllLines(fileName);

                Console.WriteLine("Reading......");

                foreach (string line in lineInFile)
                {
                    Console.WriteLine("\t" + line);
                }

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
            string startTimeStamp = lineInFile[0];
            string endTimeStamp = lineInFile[lineInFile.Length - 1];

            int portNumber = Convert.ToInt32(lineInFile[1]);

            List<string> currentPackets = new List<string>();
            string currentPacket = "";


            for (int i = 2; i < lineInFile.Length - 1; i++)
            {
                if (lineInFile[i].Equals(""))
                {

                    if (!currentPacket.Equals(""))
                    {
                        currentPackets.Add(currentPacket);
                        currentPacket = "";
                    }
                    
                }else{
                    currentPacket += lineInFile[i] + "*";
                }
            }

            foreach(string packet in currentPackets){
                Console.WriteLine(packet);
            }

            splitData(currentPackets);
        }

        public void splitData(List<string> currentPackets)
        {
            List<Packet> packets = new List<Packet>();

            foreach (string packetString in currentPackets)
            {
                string[] packetData = packetString.Split('*');

                if (packetData[1].Equals("P"))
                {
                    DateTime packetTimeStamp = DateTime.Parse(packetData[0]);

                    string[] dataPairs = packetData[2].Split(' ');

                    Data newData = new Data(dataPairs);

                    Packet newPacket = new Packet(packetTimeStamp, newData);

                    Console.WriteLine("\t" + packetTimeStamp);

                    foreach (string data in dataPairs)
                    {
                        Console.WriteLine("\t \t" + data);
                    }
                }

                foreach (string data in packetData)
                {
                    Console.WriteLine(data);
                }
            }
        }

    }
}
