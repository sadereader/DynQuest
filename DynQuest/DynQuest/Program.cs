using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynQuest
{
    class Program
    {
        public static bool Run = true;
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing DynQuest version {0}.","version.goes.here.");
            while (Run)
            {
                //magic happens here

            }


            Console.Write("DynQuest has reached exit state, press any key to close DynQuest");
            Console.ReadKey(true);
        }
    }
}
