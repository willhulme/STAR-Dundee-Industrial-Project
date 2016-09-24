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

        public List<Packet2> parseFile(string[] filepaths)
        {
            CRC8 crc_check = new CRC8();

            StreamReader streamReader = new StreamReader("C:/Users/ryanrobinson/Downloads/team_project_example_files_19-09-16/test1/link1.rec"); //set up reader
==
            //packet.timeStamp = DateTime.ParseExact(line, "dd-MM-yyyy HH:mm:ss.fff", null);
            recordingTime = DateTime.ParseExact(streamReader.ReadLine(), "dd-MM-yyyy HH:mm:ss.fff", null); //get initial recording date
            port = int.Parse(streamReader.ReadLine()); //get port number
            streamReader.ReadLine(); //blank line

            while ((line = streamReader.ReadLine()) != null) //start of packets
            {
                Packet2 packet = new Packet2(); //create a packet   
                packet.timeStamp = DateTime.ParseExact(line, "dd-MM-yyyy HH:mm:ss.fff", null); //parse packet timestamp
                Console.WriteLine(packet.timeStamp);
                line = streamReader.ReadLine(); //next line
                if (streamReader.Peek() == -1) //if this is -1 then it means it has reached the end of the file
                {
                    break; //if reached the end of the file discard the timestamp we got previsouly 
                }
                packet.packetType = char.Parse(line); //this should be either 'P' OR 'E'

                line = streamReader.ReadLine(); //next line should be the data or if it says disconnect or parity etc
                string cargo = line;
                packet.data = line.Split(' '); //split it into an array
                packet.dataLength = packet.data.Length; //get length
                line = streamReader.ReadLine(); //next line should be packet marker
                if(line != "") //if it is blank this means that the packet marker was 'E' so skip it
                { 
                    packet.packetMarkerType = line; //should be eop eep none etc
                    line = streamReader.ReadLine();
                }
                //MAYBE HERE DO DETECTION OF ERROR
                if(packet.packetType.Equals('E'))
                {
                    packet.errorType = packet.data[0]; //if E then set the error type to be what is in data ie. disconnect or parity so on so on
                }
                else if(packet.packetMarkerType.Equals("None")) //self explanitary
                {
                    packet.errorType = "None";
                }
                else if (packet.packetMarkerType.Equals("EEP"))
                {
                    packet.errorType = "EEP";
                }
                else //if the packet seems to be fine. lets do a crc check on it
                {
                    cargo = trimPathAddress(cargo); //trim the path address off it at the start
                    packet.protocol = GetProtocol(cargo); //get the protocol of the packet
                    if (packet.protocol.Equals("RMAP")) //if it is rmap
                    {
                        int i = crc_check.Check(cargo); //then crc (i should really make this static)
                        if (i != 0) //if the crc doesnt match the calculated crc then it is an error
                        {
                            packet.errorType = "CRC"; 
                        }
                    }

                }
                packetList.Add(packet); //add to packet list
            }
            streamReader.Close();
            return packetList;
        } 

        private static string trimPathAddress(string cargo)
        {
            string[] characters = cargo.Split(' ');
            byte[] characterBytes = characters.Select(s => Convert.ToByte(s, 16)).ToArray();
            List<string> charBytes = new List<string>(characters);
            int index = 0;
            while(characterBytes[index] < 32)
            {
                charBytes.RemoveAt(0);
                index++;
            }
            return String.Join(" ", charBytes.ToArray());
        }

        private static string GetProtocol(string cargo)
        {
            cargo = trimPathAddress(cargo);
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
