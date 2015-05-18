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

            appBuilder.UsePiercer();

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
    }
}