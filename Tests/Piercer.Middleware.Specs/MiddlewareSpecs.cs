using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Owin.Builder;
using Xunit;

namespace Piercer.Middleware.Specs
{
    public class MiddlewareSpecs
    {
        [Fact]
        public async Task When_using_the_default_settings_it_should_be_reachable_through_its_default_route()
        {
            // -------------------------------------------------------------------------------------------------------
            // Arrange
            // -------------------------------------------------------------------------------------------------------
            var appBuilder = new AppBuilder();

            appBuilder.UsePiercer(new PiercerSettings());

            // -------------------------------------------------------------------------------------------------------
            // Act
            // -------------------------------------------------------------------------------------------------------
            var httpClient = new HttpClient(new OwinHttpMessageHandler(appBuilder.Build()));

            var result = await httpClient.GetStringAsync("http://localhost/api/piercer/assemblies");

            // -------------------------------------------------------------------------------------------------------
            // Assert
            // -------------------------------------------------------------------------------------------------------
            result.Should().Contain("Piercer.Middleware");
        }

        [Fact]
        public async Task When_specifying_an_explicit_route_it_should_be_reachable_through_that_route()
        {
            // -------------------------------------------------------------------------------------------------------
            // Arrange
            // -------------------------------------------------------------------------------------------------------
            var appBuilder = new AppBuilder();

            appBuilder.UsePiercer(new PiercerSettings().AtRoute("/myroute"));

            var httpClient = new HttpClient(new OwinHttpMessageHandler(appBuilder.Build()));

            // -------------------------------------------------------------------------------------------------------
            // Act
            // -------------------------------------------------------------------------------------------------------

            var result = await httpClient.GetStringAsync("http://localhost/myroute/piercer/assemblies");

            // -------------------------------------------------------------------------------------------------------
            // Assert
            // -------------------------------------------------------------------------------------------------------
            result.Should().Contain("Piercer.Middleware");
        }

        [Fact]
        public async Task When_excluding_certain_assemblies_it_should_not_return_them_in_the_result()
        {
            // -------------------------------------------------------------------------------------------------------
            // Arrange
            // -------------------------------------------------------------------------------------------------------
            var appBuilder = new AppBuilder();

            appBuilder.UsePiercer(new PiercerSettings().Ignoring("Piercer.Middleware"));

            var httpClient = new HttpClient(new OwinHttpMessageHandler(appBuilder.Build()));

            // -------------------------------------------------------------------------------------------------------
            // Act
            // -------------------------------------------------------------------------------------------------------
            var result = await httpClient.GetStringAsync("http://localhost/api/piercer/assemblies");

            // -------------------------------------------------------------------------------------------------------
            // Assert
            // -------------------------------------------------------------------------------------------------------
            result.Should().NotContain("Piercer.Middleware");
        }
    }
}