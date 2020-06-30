using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePACKER
{
    public static class Log
    {
        public enum Code
        {
            Default = 1,
            Warning = 2,
            Error = 3,
            Success = 4
        }

        public static void Write(string str, Code level = Code.Default)
        {
            string Title = "INFO";

            switch (level)
            {
                case Code.Error:
                    Title = "ERROR";
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case Code.Success:
                    Title = "SUCCESS";
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case Code.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }

            Console.WriteLine("  [" + Title + "]: " + str);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
