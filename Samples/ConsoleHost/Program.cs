using System;
using Piercer;

namespace ConsoleHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            foreach (var assembly in new Diagnostics().GetAssemblies())
            {
                Console.WriteLine(assembly);
            }
        }
    }
}