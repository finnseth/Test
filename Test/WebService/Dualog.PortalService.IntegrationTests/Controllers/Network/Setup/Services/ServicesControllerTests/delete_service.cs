using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;
using Dualog.PortalService.Controllers.Network.Setup.Services.Model;

namespace Dualog.PortalService.Controllers.Setup.Services.ServicesControllerTests
{
    public class delete_service : ControllerTests
    {
        [Fact]
        public async Task should_be_ok()
        {
            var service = Fixture.Create<ServiceDetailModel>();

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var added = await client.AddAsync( ApiUrl.CompanyServiceApi, service, HttpStatusCode.Created );

                var response = await client.DeleteAsync($"{ApiUrl.ServiceApi}/{added.Id}" );
                response.StatusCode.Should().Be( HttpStatusCode.OK );

                var stored = await client.GetAsync<ServiceDetailModel>($"{ApiUrl.CompanyServiceApi}/{added.Id}", HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public async Task delete_nonexistent_service_should_be_ok()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var response = await client.DeleteAsync($"{ApiUrl.ServiceApi}/0" );
                response.StatusCode.Should().Be( HttpStatusCode.OK );
            }

        }
    }
}
