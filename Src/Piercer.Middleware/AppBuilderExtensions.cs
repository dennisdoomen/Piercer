using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Autofac;
using Autofac.Features.ResolveAnything;
using Autofac.Integration.WebApi;
using Owin;
using Swashbuckle.Application;

namespace Piercer.Middleware
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UsePiercer(this IAppBuilder appBuilder, PiercerSettings settings)
        {
            HttpConfiguration configuration = BuildHttpConfiguration(settings);

            EnableSwagger(configuration);

            appBuilder.Map(settings.Route, a => a.UseWebApi(configuration));

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

        private static HttpConfiguration BuildHttpConfiguration(PiercerSettings settings)
        {
            var configuration = new HttpConfiguration();

            configuration.Services.Replace(typeof (IAssembliesResolver), new WebApiAssembliesResolver());
            configuration.MapHttpAttributeRoutes();
            configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(settings);
            containerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(containerBuilder.Build());

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