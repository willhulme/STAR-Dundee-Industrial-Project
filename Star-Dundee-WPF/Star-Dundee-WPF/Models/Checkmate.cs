using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Star_Dundee_WPF.Models
{
    class Checkmate
    {
        Crc32 crc32 = new Crc32();
        public string checkmate(string cargo)
        {
            string hash = string.Empty;
            byte[] byteArray = Encoding.UTF8.GetBytes(cargo);
            MemoryStream stream = new MemoryStream(byteArray);
            foreach (byte b in crc32.ComputeHash(stream)) hash += b.ToString("x2").ToLower();
            return hash;
        }
    }
}
