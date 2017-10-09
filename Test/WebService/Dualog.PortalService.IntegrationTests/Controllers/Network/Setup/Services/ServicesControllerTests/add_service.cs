using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;
using Dualog.PortalService.Controllers.Network.Setup.Services.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.Ship.Model;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.Setup.Services.ServicesControllerTests
{
    public class add_service : ControllerTests
    {
        [Fact]
        public async Task to_company_with_all_properties_should_be_ok()
        {
            var service = Fixture.Create<ServiceDetailModel>();

            // Assign
            using (var server = CreateServer())
            using (var client = server.CreateClient())
            {
                var result = await client.AddAsync($"{ApiUrl.CompanyServiceApi}", service, HttpStatusCode.Created);
                service.Id = result.Id;

                result.ShouldBeEquivalentTo(service);

                var stored = await client.GetAsync<GenericDataModel<ServiceDetailModel>>($"{ApiUrl.ServiceApi}/{service.Id}");
                stored.Value.ShouldBeEquivalentTo(service, o =>
                   o.Excluding(p => p.Company).Excluding(p => p.Ship)
                );
            }
        }

        [Fact]
        public async Task to_company_with_invalid_property_should_be_bad_request()
        {
            var service = Fixture.Create<ServiceDetailModel>();
            service.Protocol = 10000;

            // Assign
            using (var server = CreateServer())
            using (var client = server.CreateClient())
            {
                var result = await client.AddAsync($"{ApiUrl.CompanyServiceApi}", service, HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task to_vessel_with_all_properties_should_be_ok()
        {
            var vessel = Fixture.Create<ShipModel>();
            var service = Fixture.Create<ServiceDetailModel>();

            // Assign
            using (var server = CreateServer())
            using (var client = server.CreateClient())
            {
                var addedVessel = await client.AddAsync<ShipModel>($"{ApiUrl.ShipApi}", vessel);

                var result = await client.AddAsync($"{ApiUrl.ShipServiceApi}/{addedVessel.Id}", service, HttpStatusCode.Created);
                service.Id = result.Id;

                result.ShouldBeEquivalentTo(service);

                var stored = await client.GetAsync<GenericDataModel<ServiceDetailModel>>($"{ApiUrl.ServiceApi}/{service.Id}");
                stored.Value.ShouldBeEquivalentTo(service, o =>
                   o.Excluding(p => p.Company).Excluding(p => p.Ship)
                );
            }
        }

        [Fact]
        public async Task to_vessel_with_invalid_property_should_be_bad_request()
        {
            var vessel = Fixture.Create<ShipModel>();
            var service = Fixture.Create<ServiceDetailModel>();
            service.Protocol = 10000;

            // Assign
            using (var server = CreateServer())
            using (var client = server.CreateClient())
            {
                var addedVessel = await client.AddAsync($"{ApiUrl.ShipApi}", vessel);
                var result = await client.AddAsync($"{ApiUrl.ShipServiceApi}/{addedVessel.Id}", service, HttpStatusCode.BadRequest);
            }
        }


    }
}
