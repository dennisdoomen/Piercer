using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IEnumerable<string> query =
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                select assembly.FullName;

            foreach (string assembly in query)
            {
                Console.WriteLine(assembly);
            }
        }
    }
}