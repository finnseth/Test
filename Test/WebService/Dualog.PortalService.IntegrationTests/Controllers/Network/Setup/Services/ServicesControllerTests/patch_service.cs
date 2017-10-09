using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;
using Newtonsoft.Json.Linq;
using Dualog.PortalService.Controllers.Network.Setup.Services.Model;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.Setup.Services.ServicesControllerTests
{
    public class patch_service : ControllerTests
    {
        [Fact]
        public async Task should_be_ok()
        {
            var service = Fixture.Create<ServiceDetailModel>();
            var patchService = Fixture.Create<ServiceDetailModel>();

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
                var added = await client.AddAsync(ApiUrl.CompanyServiceApi, service, HttpStatusCode.Created );
                service.Id = added.Id;
                patchService.Id = added.Id;

                var patched = await client.PatchAsync<ServiceDetailModel>($"{ApiUrl.ServiceApi}/{service.Id}", JObject.Parse( patch ), HttpStatusCode.OK );
                patched.ShouldBeEquivalentTo( patchService, o =>
                   o.Excluding(p => p.Company).Excluding(p => p.Ship)
                );

                var stored = await client.GetAsync<GenericDataModel<ServiceDetailModel>>($"{ApiUrl.ServiceApi}/{service.Id}", HttpStatusCode.OK);
                patched.ShouldBeEquivalentTo( stored.Value, o =>
                   o.Excluding(p => p.Company).Excluding(p => p.Ship)
                );
            }
        }

        [Fact]
        public async Task where_service_is_nonexistent_should_be_404()
        {
            var patchService = Fixture.Create<ServiceDetailModel>();

            var patch = $@"{{
                              name: '{patchService.Name}',
                              port: '{patchService.Port}',
                              ports: '{patchService.Ports}',
                              protocol: '{patchService.Protocol}',
                           }}";

            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {

                var patched = await client.PatchAsync<ServiceDetailModel>($"{ApiUrl.ServiceApi}/0", JObject.Parse( patch ), HttpStatusCode.NotFound );
            }
        }

        [Fact]
        public async Task where_properties_are_invalid_should_be_bad_request()
        {
            var service = Fixture.Create<ServiceDetailModel>();

            var patch = $@"{{
                              protocol: 10000000,
                           }}";

            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var added = await client.AddAsync( ApiUrl.CompanyServiceApi, service, HttpStatusCode.Created );

                var patched = await client.PatchAsync<ServiceDetailModel>($"{ApiUrl.ServiceApi}/{added.Id}", JObject.Parse( patch ), HttpStatusCode.BadRequest );
            }

        }
    }
}
