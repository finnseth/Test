using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.PortalService.Controllers.Services.Model;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Controllers.Services.ServicesControllerTests
{
    public class patch_service : ControllerTests
    {
        [Fact]
        public async Task should_be_ok()
        {
            var service = Fixture.Create<ServiceDetails>();
            var patchService = Fixture.Create<ServiceDetails>();

            var patch = $@"{{
                              name: '{patchService.Name}',
                              port: '{patchService.Port}',
                              ports: '{patchService.Ports}',
                              protocol: '{patchService.Protocol}',
                           }}";

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var added = await client.AddAsync( "api/v1/internet/networkcontrol/services", service, HttpStatusCode.Created );
                service.Id = added.Id;
                patchService.Id = added.Id;

                var patched = await client.PatchAsync<ServiceDetails>($"api/v1/internet/networkcontrol/services/{service.Id}", JObject.Parse( patch ), HttpStatusCode.OK );
                patched.ShouldBeEquivalentTo( patchService );

                var stored = await client.GetAsync<ServiceDetails>($"api/v1/internet/networkcontrol/services/{service.Id}", HttpStatusCode.OK);
                patched.ShouldBeEquivalentTo( stored );
            }
        }

        [Fact]
        public async Task where_service_is_nonexistent_should_be_404()
        {
            var patchService = Fixture.Create<ServiceDetails>();

            var patch = $@"{{
                              name: '{patchService.Name}',
                              port: '{patchService.Port}',
                              ports: '{patchService.Ports}',
                              protocol: '{patchService.Protocol}',
                           }}";

            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {

                var patched = await client.PatchAsync<ServiceDetails>($"api/v1/internet/networkcontrol/services/0", JObject.Parse( patch ), HttpStatusCode.NotFound );
            }
        }

        [Fact]
        public async Task where_properties_are_invalid_should_be_bad_request()
        {
            var service = Fixture.Create<ServiceDetails>();

            var patch = $@"{{
                              protocol: 10000000,
                           }}";

            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var added = await client.AddAsync( "api/v1/internet/networkcontrol/services", service, HttpStatusCode.Created );

                var patched = await client.PatchAsync<ServiceDetails>($"api/v1/internet/networkcontrol/services/{added.Id}", JObject.Parse( patch ), HttpStatusCode.BadRequest );
            }

        }
    }
}
