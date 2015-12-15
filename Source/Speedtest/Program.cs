using System;
using System.Diagnostics;
using System.IO;
using Cyral.BrainF.Interpreter;
namespace Speedtest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var source = File.ReadAllText("../../../../Files/mandelbrot.b");
            var interpreter = new Interpreter(30000, Console.Write, () => (char)Console.Read());
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 1; i++)
            interpreter.Run(source);
            watch.Stop();
            Console.WriteLine("Completed in ms: " + watch.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}