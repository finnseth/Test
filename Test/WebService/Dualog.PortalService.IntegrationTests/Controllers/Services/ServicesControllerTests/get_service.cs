using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.PortalService.Controllers.Services.Model;
using Ploeh.AutoFixture;
using Xunit;

namespace Dualog.PortalService.Controllers.Services.ServicesControllerTests
{
    public class get_service : ControllerTests
    {
        [Fact]
        public async Task single_existing_should_be_ok()
        {
            var service = Fixture.Create<ServiceDetails>();

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var added = await client.AddAsync( "api/v1/internet/networkcontrol/services", service, HttpStatusCode.Created );
                var stored = await client.GetAsync<ServiceDetails>( $"api/v1/internet/networkcontrol/services/{added.Id}", HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task single_nonexistent_should_be_not_found()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var stored = await client.GetAsync<ServiceDetails>( $"api/v1/internet/networkcontrol/services/0", HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public async Task set_for_company_many_should_be_ok()
        {
            var services = new []{
                Fixture.Create<ServiceDetails>(),
                Fixture.Create<ServiceDetails>(),
                Fixture.Create<ServiceDetails>()
            };

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                foreach( var serviceDetails in services )
                {
                    var added = await client.AddAsync( "api/v1/internet/networkcontrol/services", serviceDetails, HttpStatusCode.Created );
                    serviceDetails.Id = added.Id;
                }

                var result = await client.GetAsync<IEnumerable<ServiceDetails>>( "api/v1/internet/networkcontrol/services", HttpStatusCode.OK );
            }

        }
    }
}
