using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.PortalService.Controllers.Email.Setup.Quarantine.Model;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using Dualog.PortalService.Controllers.Organization.Shipping.User;
using Dualog.PortalService.Controllers.Organization.Shipping.Ship.Model;
using Ploeh.AutoFixture;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.Email.Setup.Quarantine.QuarantineControllerTests
{
    public class get_quarantine_vessel_config : ControllerTests
    {
        [Fact]
        public async Task should_be_ok()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient())
            {
                await UserRepository.InternalGrantPermission( DataContextFactory.CreateContext(), "EmailRestriction", 2, LoggedInUserId, LoggedInCompanyId );


                // Assign
                var vessel = await client.AddAsync("/api/v1/organization/shipping/ship/", Fixture.Create<ShipModel>());

                var response = await client.GetAsync($"/api/v1/email/setup/quarantine/shipquarantine/{vessel.Id}");

                // Assert
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

                var s = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GenericDataModel<QuarantineVesselModel>>(s);

                result.Value.VesselId.Should().Be(vessel.Id);

                await UserRepository.InternalRevokePermission( DataContextFactory.CreateContext(), "EmailRestriction", LoggedInUserId, LoggedInCompanyId );
            }
        }
    }
}
