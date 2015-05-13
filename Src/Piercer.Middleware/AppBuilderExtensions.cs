using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Owin;
using Swashbuckle.Application;

namespace Piercer.Middleware
{
    public static class AppBuilderExtensions
    {
        private const string RootPath = "/api";

        public static IAppBuilder UsePiercer(this IAppBuilder appBuilder, PiercerSettings settings)
        {
            HttpConfiguration configuration = BuildHttpConfiguration();

            EnableSwagger(configuration);

            appBuilder.Map(RootPath, a => a.UseWebApi(configuration));

            return appBuilder;
        }

        private static void EnableSwagger(HttpConfiguration configuration)
        {
            configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Piercer; easily diagnose run-time assemblies and threads");
                    c.RootUrl(req => SwaggerDocsConfig.DefaultRootUrlResolver(req) + "/api");
                    c.IncludeXmlComments(GetXmlCommentsPath());
                })
                .EnableSwaggerUi();
        }

        private static string GetXmlCommentsPath()
        {
            return Assembly.GetExecutingAssembly().CodeBase.ToLower().Replace(".dll", ".xml");
        }

        private static HttpConfiguration BuildHttpConfiguration()
        {
            var configuration = new HttpConfiguration();

            configuration.Services.Replace(typeof (IAssembliesResolver), new WebApiAssembliesResolver());
            configuration.MapHttpAttributeRoutes();
            configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;

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

    public class PiercerSettings
    {
    }
}