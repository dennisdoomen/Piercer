using System;
using Microsoft.Owin.Hosting;

namespace ConsoleHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string rootUrl = "http://localhost:12345";

            using (WebApp.Start<Startup>(rootUrl))
            {
                Console.WriteLine("OWIN host running at {0}. Press ENTER to shutdown", rootUrl);
                Console.ReadLine();
            }
        }
    }
}