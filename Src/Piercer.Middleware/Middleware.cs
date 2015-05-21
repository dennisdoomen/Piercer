using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Microsoft.Owin.Builder;
using Owin;
using Swashbuckle.Application;

namespace Piercer.Middleware
{
    using MidFunc = Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>;

    internal static class Middleware
    {
        public static MidFunc Create()
        {
            return next =>
            {
                var appBuilder = new AppBuilder();

                HttpConfiguration configuration = BuildHttpConfiguration();

                EnableSwagger(configuration);

                appBuilder.Map("/api", a => a.UseWebApi(configuration));
                appBuilder.Run(ctx => next(ctx.Environment));

                return appBuilder.Build();
            };
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