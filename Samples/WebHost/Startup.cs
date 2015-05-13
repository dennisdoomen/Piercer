using Microsoft.Owin;
using Owin;
using Piercer.Middleware;

[assembly: OwinStartupAttribute(typeof(WebHost.Startup))]
namespace WebHost
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UsePiercer(new PiercerSettings());
            ConfigureAuth(app);
        }
    }
}
