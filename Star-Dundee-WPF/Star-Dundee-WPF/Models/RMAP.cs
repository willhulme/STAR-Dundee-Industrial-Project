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
        public byte sourcelogicalAddress { get; set; }
        public byte destinationlogicalAddress { get; set; }
        byte ptCmdSp { get; set; }
        //bool hasSourceAdd;
        public byte sourceAddLen;
        public string command { get; set; }
        public byte destinationKey { get; set; }
        public byte sourceLAdd { get; set; }
        //ushort transactionID { get; set; }
        public byte[] transactionID = new byte[2];
        public byte extWriteAdd { get; set; }
        public byte extReadAdd { get; set; }
        public uint writeAddress { get; set; }
        public uint readAddress { get; set; }
        public byte[] dataLength = new byte[3];
        public uint dataLengthInt { get; set; }
        public byte[] data { get; set; }
        public byte headerCRC { get; set; }
        public byte dataCRC { get; set; }
        public byte replyCRC { get; set; }
        public byte reserved { get; set; }
        public byte status { get; set; }

        public RMAP() { } //CONSTRUCTOR
 

        public void buildPacket(string packet)
        {
            string[] characters = packet.Split(' ');
            byte[] characterBytes = characters.Select(s => Convert.ToByte(s, 16)).ToArray();
            ptCmdSp = characterBytes[0];
            command = getCommandType(ptCmdSp);

            if (command.Contains("REPLY"))
            {
                getReplyPacket(characterBytes);
            }
            else
            {
                GetCommandPacket(characterBytes);
            }
            printPacketDetails(this);

        }

        private void GetCommandPacket(byte[] characterBytes)
        {
            destinationKey = characterBytes[1];
            int i = sourceAddLen * 4;
            sourcelogicalAddress = characterBytes[2 + i];
            transactionID[0] = characterBytes[3 + i];
            transactionID[1] = characterBytes[4 + i];

            if (command.Equals("WRITE"))
            {
                //i = GetWritePacket(characterBytes, i);
                GetWritePacket(characterBytes, i);
            }
            else if (command.Equals("READ"))
            {

                GetReadPacket(characterBytes, i);
            }
        }

        private void GetReadPacket(byte[] characterBytes, int i)
        {
            extReadAdd = characterBytes[5 + i];
            byte[] readBytes = new byte[4] { characterBytes[6 + i], characterBytes[7 + i], characterBytes[8 + i], characterBytes[9 + i] };
            Array.Reverse(readBytes);
            readAddress = arrayToInt(readBytes);
            dataLength[0] = characterBytes[10 + i];
            dataLength[1] = characterBytes[11 + i];
            dataLength[2] = characterBytes[12 + i];
            Array.Reverse(dataLength);
            dataLengthInt = arrayToInt(dataLength);
            headerCRC = characterBytes[13 + i];
        }

        private int GetWritePacket(byte[] characterBytes, int i)
        {
            extWriteAdd = characterBytes[5 + i];
            byte[] writeBytes = new byte[4] { characterBytes[6 + i], characterBytes[7 + i], characterBytes[8 + i], characterBytes[9 + i] };
            Array.Reverse(writeBytes);
            writeAddress = arrayToInt(writeBytes);
            dataLength[0] = characterBytes[10 + i];
            dataLength[1] = characterBytes[11 + i];
            dataLength[2] = characterBytes[12 + i];
            Array.Reverse(dataLength);
            dataLengthInt = arrayToInt(dataLength);
            headerCRC = characterBytes[13 + i];
            data = new byte[dataLengthInt];
            int j = 0;
            for (int k = 14 + i; k < characterBytes.Length - 1; k++)
            {
                data[j] = characterBytes[k];
                j++;
            }
            dataCRC = characterBytes[(characterBytes.Length - 1)];
            return i;
        }

        private void getReplyPacket(byte[] characterBytes)
        {
            status = characterBytes[1];
            destinationlogicalAddress = characterBytes[2];
            transactionID[0] = characterBytes[3];
            transactionID[1] = characterBytes[4];

            if (command.Equals("WRITE REPLY"))
            {
                replyCRC = characterBytes[5];
                //return;
            }
            else
            {
                dataLength[0] = characterBytes[6];
                dataLength[1] = characterBytes[7];
                dataLength[2] = characterBytes[8];
                Array.Reverse(dataLength);
                dataLengthInt = arrayToInt(dataLength);
                headerCRC = characterBytes[9];
                data = new byte[dataLengthInt];
                int j = 0;
                for (int i = 10; i < characterBytes.Length - 1; i++)
                {
                    data[j] = characterBytes[i];
                    j++;
                }
                dataCRC = characterBytes[(characterBytes.Length - 1)];
            }
        }

        private uint arrayToInt(byte[] dataLength)
        {
            //Array.Reverse(dataLength);
            int pos = 0;
            uint result = 0;
            foreach(byte by in dataLength)
            {
                result |= (uint)(by << pos);
                 pos += 8;
            }
               return result;
        }

        private byte calcsourceAdd(bool[] bits)
        {
            Array.Reverse(bits);
            BitArray sourceBitArray = new BitArray(bits);
            byte[] lNum = new byte[1];
            sourceBitArray.CopyTo(lNum, 0);

            return lNum[0];

        }



        private string getCommandType(byte cmdByte)
        {
            bool commandResponseBit;
            bool writeReadBit;
            bool readModWrite = false;
            bool[] cmdBits;
            string command = "";

            //bool[] sourceAddressBits = new bool[2];
            commandResponseBit = getBit(cmdByte, 7);
            writeReadBit = getBit(cmdByte, 6);
            
             if(writeReadBit == false)
             {
                 readModWrite = getBit(cmdByte, 5);
             }
            //sourceAddressBits[0] = getBit(cmdByte, 2);
            //sourceAddressBits[1] = getBit(cmdByte, 1);
            cmdBits = new bool[] { readModWrite, writeReadBit, commandResponseBit };
            BitArray bitArray = new BitArray(cmdBits);
            command = identifyCommand(bitArray);

            //sourceAddLen = calcsourceAdd(sourceAddressBits);
            sourceAddLen = (byte)GetSourcePathLength(cmdByte);

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

        public string[] GetHeader(string[] packetCharacters)
        {
            int i = 0;
            //int index = 0;
            string command;
            byte[] characterBytes = packetCharacters.Select(s => Convert.ToByte(s, 16)).ToArray();
            
            while(characterBytes[i] < 32)
            {
                i++;
            }

            command = getCommandType(characterBytes[i + 2]);
            //return command;
            if(command.Equals("WRITE REPLY") || command.Equals("READ"))
            {
                string[] headerOnly = new string[1];
                headerOnly[0] = String.Join(" ", packetCharacters);
                return headerOnly;
            }
            else
            {
                string[] headerAndData = new string[2];
                if (command.Equals("WRITE") || command.Equals("READ-MODIFY-WRITE"))
                {
                    int j = (GetSourcePathLength(characterBytes[i + 2])*4);
                    i += (16 + j);
                    string [] headerCharacters = new string[i];
                    string [] dataCharacters = new string[characterBytes.Length - i];

                    Array.Copy(packetCharacters, 0, headerCharacters, 0, i);
                    Array.Copy(packetCharacters, i, dataCharacters, 0, dataCharacters.Length);

                    headerAndData[0] = String.Join(" ", headerCharacters);
                    headerAndData[1] = String.Join(" ", dataCharacters);
                }
                else if (command.Equals("READ REPLY") || command.Equals("READ-MODIFY-WRITE REPLY"))
                {
                    i += 12;
                    string[] headerCharacters = new string[i];
                    string[] dataCharacters = new string[characterBytes.Length - i];

                    Array.Copy(packetCharacters, 0, headerCharacters, 0, i);
                    Array.Copy(packetCharacters, i, dataCharacters, 0, dataCharacters.Length);

                    headerAndData[0] = String.Join(" ", headerCharacters);
                    headerAndData[1] = String.Join(" ", dataCharacters);
                }

                return headerAndData;
            }

        }

        private int GetSourcePathLength(byte cmdByte)
        {
            bool[] sourceAddressBits = new bool[2];
            sourceAddressBits[0] = getBit(cmdByte, 2);
            sourceAddressBits[1] = getBit(cmdByte, 1);
            return calcsourceAdd(sourceAddressBits);
        }

    }

    


    

}
