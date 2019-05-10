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
            Task<List<string>> task = MidiEngine.GetInputDevices();
            task.Wait();
            List<string> devices = task.Result;

            foreach (string s in devices)
            {
                Console.WriteLine(s);
            }
        }
    }
}
