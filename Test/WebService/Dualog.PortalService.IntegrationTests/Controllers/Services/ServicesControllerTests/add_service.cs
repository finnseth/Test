using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.PortalService.Controllers.Services.Model;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;
using Dualog.PortalService.Controllers.Vessels.Model;

namespace Dualog.PortalService.Controllers.Services.ServicesControllerTests
{
    public class add_service : ControllerTests
    {
        [Fact]
        public async Task to_company_with_all_properties_should_be_ok()
        {
            var service = Fixture.Create<ServiceDetails>();

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var result = await client.AddAsync( "api/v1/internet/networkcontrol/services", service, HttpStatusCode.Created );
                service.Id = result.Id;

                result.ShouldBeEquivalentTo( service );

                var stored = await client.GetAsync<ServiceDetails>($"api/v1/internet/networkcontrol/services/{service.Id}" );
                stored.ShouldBeEquivalentTo( service );
            }
        }

        [Fact]
        public async Task to_company_with_invalid_property_should_be_bad_request()
        {
            var service = Fixture.Create<ServiceDetails>();
            service.Protocol = 10000;

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var result = await client.AddAsync( "api/v1/internet/networkcontrol/services", service, HttpStatusCode.BadRequest );
            }
        }

        [Fact]
        public async Task to_vessel_with_all_properties_should_be_ok()
        {
            var vessel = Fixture.Create<VesselDetails>();
            var service = Fixture.Create<ServiceDetails>();

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var addedVessel = await client.AddAsync<VesselDetails>( "api/v1/vessels", vessel );

                var result = await client.AddAsync( $"api/v1/internet/networkcontrol/services/vessels/{addedVessel.Id}", service, HttpStatusCode.Created );
                service.Id = result.Id;

                result.ShouldBeEquivalentTo( service );

                var stored = await client.GetAsync<ServiceDetails>($"api/v1/internet/networkcontrol/services/{service.Id}" );
                stored.ShouldBeEquivalentTo( service );
            }
        }

        [Fact]
        public async Task to_vessel_with_invalid_property_should_be_bad_request()
        {
            var vessel = Fixture.Create<VesselDetails>();
            var service = Fixture.Create<ServiceDetails>();
            service.Protocol = 10000;

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var addedVessel = await client.AddAsync<VesselDetails>( "api/v1/vessels", vessel );
                var result = await client.AddAsync( $"api/v1/internet/networkcontrol/services/vessels/{addedVessel.Id}", service, HttpStatusCode.BadRequest );
            }
        }


    }
}
