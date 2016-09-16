using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF.Models
{
    class Checkmate
    {
        CRC8 CRC = new CRC8();
        private bool lenCheck(string cargo)
        {
            CRC.Check(cargo);
            return true;
        }
    }
}
