using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.PortalService.Controllers.Quarantine.Model;
using Dualog.PortalService.Controllers.Vessels.Model;
using FluentAssertions;
using NCrunch.Framework;
using Newtonsoft.Json;
using Xunit;
using Dualog.PortalService.Controllers.Users;

namespace Dualog.PortalService.Controllers.Quarantine.QuarantineControllerTests
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
                var objectLookup = await SetupData(@".\TestData\quarantine_vessel_config.json", server);
                var vessel = objectLookup.GetObjectById<VesselDetails>("v1");

                var response = await client.GetAsync($"/api/v1/email/settings/quarantine/vessels/{vessel.Id}");

                // Assert
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

                var s = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<QuarantineVesselConfig>(s);

                result.VesselId.Should().Be(vessel.Id);

                await UserRepository.InternalRevokePermission( DataContextFactory.CreateContext(), "EmailRestriction", LoggedInUserId, LoggedInCompanyId );
            }
        }
    }
}
