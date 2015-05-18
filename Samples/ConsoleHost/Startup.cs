using Owin;
using Piercer.Middleware;

namespace ConsoleHost
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UsePiercer();
        }
    }
}