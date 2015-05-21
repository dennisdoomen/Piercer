using Owin;

namespace Piercer.Middleware
{
    public static class AppBuilderExtensions
    {
        /// <summary>
        ///     Creates the OWIN mid-func needed to hook the query host Web API into an existing OWIN pipeline.
        /// </summary>
        public static IAppBuilder UsePiercer(this IAppBuilder appBuilder)
        {
            appBuilder.Use(Middleware.Create());

            return appBuilder;
        }
    }
}