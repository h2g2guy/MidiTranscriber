using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Engine;
using WindowsMidiEngine;

namespace DotNetConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<string> task = MidiEngine.GetInputDevices();

            foreach (string s in task)
            {
                Console.WriteLine(s);
            }
        }
    }
}
