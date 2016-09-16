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
        ushort transactionID { get; set; }
        byte extWriteAdd { get; set; }
        uint writeAddress { get; set; }
        uint dataLength { get; set; }
        byte[] data { get; set; }
        byte headerCRC { get; set; }
        byte dataCRC { get; set; }
        byte reserved { get; set; }
        byte status { get; set; }

        public RMAP() { } //CONSTRUCTOR
 
        public void buildPacket(byte[] packet)
        {
            ptCmdSp = packet[2];
            command = getCommandType(ptCmdSp);         
        }



        public string getCommandType(byte cmdByte)
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

        public bool getBit(byte cmdByte, int index)
        {
            bool bit = (cmdByte & (1 << index-1)) != 0;
            return bit;
        }

    }

    

}
