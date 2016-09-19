using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class RMAP
    {
        byte sourcelogicalAddress { get; set; }
        byte destinationlogicalAddress { get; set; }
        byte ptCmdSp { get; set; }
        string command { get; set; }
        byte destinationKey { get; set; }
        byte sourceLAdd { get; set; }
        //ushort transactionID { get; set; }
        byte[] transactionID = new byte[2];
        byte extWriteAdd { get; set; }
        byte extReadAdd { get; set; }
        uint writeAddress { get; set; }
        uint readAddress { get; set; }
        byte[] dataLength = new byte[3];
        uint dataLengthInt { get; set; }
        byte[] data { get; set; }
        byte headerCRC { get; set; }
        byte dataCRC { get; set; }
        byte replyCRC { get; set; }
        byte reserved { get; set; }
        byte status { get; set; }

        public RMAP() { } //CONSTRUCTOR
 
        public void buildPacket(byte[] packet)
        {
            ptCmdSp = packet[2];
            command = getCommandType(ptCmdSp);         
        }

        public void buildPacket(string packet)
        {
            string[] characters = packet.Split(' ');
            byte[] characterBytes = characters.Select(s => Convert.ToByte(s, 16)).ToArray();
            ptCmdSp = characterBytes[2];
            command = getCommandType(ptCmdSp);

            if (command.Contains("REPLY"))
            {
                status = characterBytes[3];
                destinationlogicalAddress = characterBytes[4];
                transactionID[0] = characterBytes[5];
                transactionID[1] = characterBytes[6];

                if(command.Equals("WRITE REPLY"))
                {
                    replyCRC = characterBytes[7];
                    return;
                }
                dataLength[0] = characterBytes[8];
                dataLength[1] = characterBytes[9];
                dataLength[2] = characterBytes[10];
                dataLengthInt = arrayToInt(dataLength);
                headerCRC = characterBytes[11];
                data = new byte[dataLengthInt];
                int j = 0;
                for(int i = 12; i < characterBytes.Length-1; i++)
                {
                    data[j] = characterBytes[i];
                }
                dataCRC = characterBytes[(characterBytes.Length - 1)];
            }
            else
            {
                destinationKey = characterBytes[3];
                sourcelogicalAddress = characterBytes[4];
                transactionID[0] = characterBytes[5];
                transactionID[1] = characterBytes[6];

                if (command.Equals("WRITE"))
                {
                    extWriteAdd = characterBytes[7];
                    byte[] writeBytes = new byte[4] { characterBytes[8], characterBytes[9], characterBytes[10], characterBytes[11] };
                    writeAddress = arrayToInt(writeBytes);
                    dataLength[0] = characterBytes[12];
                    dataLength[1] = characterBytes[13];
                    dataLength[2] = characterBytes[14];
                    dataLengthInt = arrayToInt(dataLength);
                    headerCRC = characterBytes[15];
                    data = new byte[dataLengthInt];
                    int j = 0;
                    for (int i = 16; i < characterBytes.Length - 1; i++)
                    {
                        data[j] = characterBytes[i];
                    }
                    dataCRC = characterBytes[(characterBytes.Length - 1)];
                }
                else if (command.Equals("READ"))
                {
                    extReadAdd = characterBytes[7];
                    byte[] readBytes = new byte[4] { characterBytes[8], characterBytes[9], characterBytes[10], characterBytes[11] };
                    readAddress = arrayToInt(readBytes);
                    dataLength[0] = characterBytes[12];
                    dataLength[1] = characterBytes[13];
                    dataLength[2] = characterBytes[14];
                    dataLengthInt = arrayToInt(dataLength);
                    headerCRC = characterBytes[15];
                }
            }
            printPacketDetails(this);

        }

        private uint arrayToInt(byte[] dataLength)
        {
            Array.Reverse(dataLength);
            int pos = 0;
            uint result = 0;
            foreach(byte by in dataLength)
            {
                result |= (uint)(by << pos);
                 pos += 8;
            }
               return result;
        }



        private string getCommandType(byte cmdByte)
        {
            bool commandResponseBit;
            bool writeReadBit;
            bool readModWrite = false;
            bool[] cmdBits;
            string command = "";
            commandResponseBit = getBit(cmdByte, 7);
            writeReadBit = getBit(cmdByte, 6);

             if(writeReadBit == false)
             {
                 readModWrite = getBit(cmdByte, 5);
             }

            cmdBits = new bool[] { readModWrite, writeReadBit, commandResponseBit };
            BitArray bitArray = new BitArray(cmdBits);
            command = identifyCommand(bitArray);

            return command;

        }

        private string identifyCommand(BitArray bitArray)
        {
            string command = "";
            byte[] cmdNum = new byte[1];
            bitArray.CopyTo(cmdNum, 0);
            
            switch (cmdNum[0])
            {
                case 6:
                    command = "WRITE";
                    break;
                                
                case 2:
                    command = "WRITE REPLY";
                    break;
                    
                case 4:
                    command = "READ";
                    break;

                case 0:
                    command = "READ REPLY";
                    break;

                case 5:
                    command = "READ-MODIFY-WRITE";
                    break;

                case 1:
                    command = "READ-MODIFY-WRITE REPLY";
                    break;

                default:
                    command = "COMMAND NOT RECOGNISED";
                    break;              
            }

            return command;
        }

        private bool getBit(byte cmdByte, int index)
        {
            bool bit = (cmdByte & (1 << index-1)) != 0;
            return bit;
        }

        void printPacketDetails(RMAP packet)
        {
            if(packet.command.Equals("WRITE"))
            {
                Console.Write("COMMAND: {0:x}/{1}\nDESTINATION KEY: {2:x}\nSOURCE LOGICAL ADDRESS: {3:x}\nTRANSACTION IDENTIFIER: {4:x}",packet.ptCmdSp,packet.command,packet.destinationKey,packet.sourceLAdd,packet.transactionID[1]);
            }
        }

    }

    


    

}
