using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Packet
    {
        DateTime timestamp;
        ErrorType errors;
        Data theData;
        int totalChars;

    }


    enum ErrorType {
        noError,
        disconnect,
        parity,
        crc,
        eep,
        timeout,
        sequence
    };
}
