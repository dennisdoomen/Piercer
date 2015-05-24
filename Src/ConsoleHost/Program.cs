using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Owin;

namespace ConsoleHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string baseUril = "http://localhost:1234";

            using (WebApp.Start<Startup>(baseUril))
            {
                Console.WriteLine("Now running at " + baseUril);
                Console.ReadLine();
            }
        }
    }

    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use((owinContext, next) =>
            {
                IDictionary<string, object> environment = owinContext.Environment;

                foreach (string key in environment.Keys)
                {
                    Console.WriteLine("{0}: {1}", key, environment[key]);
                }

                return next();
            });

            app.UseWelcomePage();
        }
    }
}