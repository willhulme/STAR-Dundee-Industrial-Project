using Star_Dundee_WPF.Models.possible_new;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Parse
    {
        public List<Packet2> packetList = new List<Packet2>();
        private string line;
        private int port;
        private DateTime recordingTime;

        public void parseFile(string[] filepaths)
        {
            CRC8 crc_check = new CRC8();
            StreamReader r = new StreamReader("C:/Users/ryanrobinson/Downloads/team_project_example_files_19-09-16/test6/link8.rec"); //set up reader
            //packet.timeStamp = DateTime.ParseExact(line, "dd-MM-yyyy HH:mm:ss.fff", null);
            recordingTime = DateTime.ParseExact(r.ReadLine(), "dd-MM-yyyy HH:mm:ss.fff", null); //get initial recording date
            port = int.Parse(r.ReadLine()); //get port number
            r.ReadLine(); //blank line

            while ((line = r.ReadLine()) != null) //start of packets
            {
                Packet2 packet = new Packet2();
                packet.timeStamp = DateTime.ParseExact(line, "dd-MM-yyyy HH:mm:ss.fff", null);
                Console.WriteLine(packet.timeStamp);
                line = r.ReadLine();
                if (r.Peek() == -1)
                {
                    //Console.WriteLine("hello");
                    break;
                }
                packet.packetType = char.Parse(line);

                line = r.ReadLine();
                string cargo = line;
                packet.data = line.Split(' ');
                line = r.ReadLine();
                if(line != "")
                { 
                    packet.packetMarkerType = line;
                    line = r.ReadLine();
                }
                //MAYBE HERE DO DETECTION OF ERROR
                if(packet.packetType.Equals('E'))
                {
                    packet.errorType = packet.data[0];
                }
                else if(packet.packetMarkerType.Equals("None"))
                {
                    packet.errorType = "None";
                }
                else
                {
                    cargo = trimPathAddress(cargo);
                    packet.protocol = GetProtocol(cargo);
                    if (packet.protocol.Equals("RMAP"))
                    {
                        int i = crc_check.Check(cargo);
                        if (i != 0)
                        {
                            packet.errorType = "CRC";
                        }
                    }

                }
                packetList.Add(packet);
            }

        }

        private static string trimPathAddress(string cargo)
        {
            string[] characters = cargo.Split(' ');
            byte[] characterBytes = characters.Select(s => Convert.ToByte(s, 16)).ToArray();
            List<string> charBytes = new List<string>(characters);
            int index = 0;
            while(characterBytes[index] < 32)
            {
                charBytes.RemoveAt(index);
                index++;
            }
            return String.Join(" ", charBytes.ToArray());
        }

        private static string GetProtocol(string cargo)
        {
            trimPathAddress(cargo);
            string protocol;
            int protocolNumber;
            string[] characters = cargo.Split(' ');
            byte[] characterBytes = characters.Select(s => Convert.ToByte(s, 16)).ToArray();
            protocolNumber = characterBytes[1];

            switch(protocolNumber)
            {
                case 1:
                    protocol = "RMAP";
                    break;

                case 250:
                    protocol = "CUSTOM";
                    break;

                default:
                    protocol = "UNACCOUNTED";
                    break;
            }

            return protocol;
        }
    }
}
