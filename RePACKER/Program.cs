using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePACKER
{
    class Program
    {
        public string Game = "187 Ride Or Die";
        public string Format = "mfd";
        public string Version = "1.0";
        
        static void Main(string[] args)
        {
            Program Tool = new Program();

            Tool.Title();

            
            if (args.Length == 0)
            {
                Tool.Usage();
            }
            else
            {
                new Archive(args[0], Tool.Format);
            }

            Console.ReadLine();
        }

        public void Title()
        {
            string ConsoleTitle = Game + " " + Format.ToUpper() + " Tool (" + Version + ")";

            Console.Title = ConsoleTitle;

            Console.WriteLine();
            Console.WriteLine("  " + ConsoleTitle);
            Console.WriteLine("  Written by Meth0d - https://meth0d.org");
            Console.WriteLine("  This software is provided without any warranty.");
            Console.WriteLine();

        }

        public void Usage()
        {
            Console.WriteLine("  [USAGE]: ");
            Console.WriteLine("   - Unpack: drag and drop a file into the tool executable.");
            Console.WriteLine("   - Repack: drag and drop a folder into the tool executable.");
            Console.WriteLine();
            Console.WriteLine("   Enter to close !");
        }
    }
}
