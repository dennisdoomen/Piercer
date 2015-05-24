using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Owin;

namespace ConsoleHost
{
    using MidFunc = Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>;

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
            app.Use((MidFunc)(next => environment =>
            {
                foreach (string key in environment.Keys)
                {
                    Console.WriteLine("{0}: {1}", key, environment[key]);
                }

                return next(environment);
            }));

            app.UseWelcomePage();
        }
    }
}