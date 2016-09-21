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
        bool hasSourceAdd;
        byte sourceAddLen;
        public string command { get; set; }
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
            for (int k = 14 + i; i < characterBytes.Length - 1; i++)
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
            bool[] sourceAddressBits = new bool[2];
            commandResponseBit = getBit(cmdByte, 7);
            writeReadBit = getBit(cmdByte, 6);
            sourceAddressBits[0] = getBit(cmdByte, 2);
            sourceAddressBits[1] = getBit(cmdByte, 1);
             if(writeReadBit == false)
             {
                 readModWrite = getBit(cmdByte, 5);
             }

            cmdBits = new bool[] { readModWrite, writeReadBit, commandResponseBit };
            BitArray bitArray = new BitArray(cmdBits);
            command = identifyCommand(bitArray);
            sourceAddLen = calcsourceAdd(sourceAddressBits);

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
