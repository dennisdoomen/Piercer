using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
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
