using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynQuest
{
    class Program
    {
        public static bool Run = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Initializing DynQuest version {0}.","version.goes.here.");
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
           //Initialize directX window here
            while (Run)
            {
                RenderFrame();
                Console.Write(".");
                Thread.Sleep(1000);
            }


            Console.Write("DynQuest has reached exit state, press any key to close DynQuest");
            Console.ReadKey(true);
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Run = false;
        }
        public static void RenderFrame()
        {

        }
    }
}
