using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;
using Dualog.PortalService.Controllers.Services.Model;
using System.Net;

namespace Dualog.PortalService.Controllers.Services.ServicesControllerTests
{
    public class delete_service : ControllerTests
    {
        [Fact]
        public async Task should_be_ok()
        {
            var service = Fixture.Create<ServiceDetails>();

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var added = await client.AddAsync( "api/v1/internet/networkcontrol/services", service, HttpStatusCode.Created );

                var response = await client.DeleteAsync($"api/v1/internet/networkcontrol/services/{added.Id}" );
                response.StatusCode.Should().Be( HttpStatusCode.OK );

                var stored = await client.GetAsync<ServiceDetails>( $"internet/networkcontrol/services/{added.Id}", HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public async Task delete_nonexistent_service_should_be_ok()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var response = await client.DeleteAsync($"api/v1/internet/networkcontrol/services/0" );
                response.StatusCode.Should().Be( HttpStatusCode.OK );
            }

        }
    }
}
