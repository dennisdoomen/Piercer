using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Owin;

namespace Piercer.Middleware
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UsePiercer(this IAppBuilder appBuilder)
        {
            HttpConfiguration configuration = BuildHttpConfiguration();

            appBuilder.Map("/api", a => a.UseWebApi(configuration));

            return appBuilder;
        }

        private static HttpConfiguration BuildHttpConfiguration()
        {
            var configuration = new HttpConfiguration();

            configuration.Services.Replace(typeof (IAssembliesResolver), new WebApiAssembliesResolver());
            configuration.MapHttpAttributeRoutes();
            configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            return configuration;
        }

        // This is needed to ensure only the controller and routes in this assembly are discovered
        /// <summary>
        /// </summary>
        private class WebApiAssembliesResolver : IAssembliesResolver
        {
            public ICollection<Assembly> GetAssemblies()
            {
                return new[] {GetType().Assembly};
            }
        }
    }
}