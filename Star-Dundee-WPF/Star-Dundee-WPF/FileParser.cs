using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Dundee_WPF
{
    class FileParser
    {

        public void readFile()
        {
            string fileName = "../../DataFiles/test4_link1.rec";

            string[] lineInFile = System.IO.File.ReadAllLines(fileName);

            Console.WriteLine("Reading......");

            foreach (string line in lineInFile)
            {
                Console.WriteLine("\t" + line);
            }

            Console.WriteLine("Reading Complete");
        }

    }
}
