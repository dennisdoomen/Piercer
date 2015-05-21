using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string baseUril = "http://localhost:1234";

            using (WebApp.Start<Startup>(baseUril))
            {
                Console.WriteLine("No running at " + baseUril);
                Console.ReadLine();
            }
        }
    }

    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
#if DEBUG
            app.UseErrorPage();
#endif
            app.UseWelcomePage("/");
        }
    }
}