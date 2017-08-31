using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.PortalService.Controllers.Quarantine.Model;
using FluentAssertions;
using NCrunch.Framework;
using Newtonsoft.Json;
using Xunit;
using Dualog.PortalService.Controllers.Users;

namespace Dualog.PortalService.Controllers.Quarantine.QuarantineControllerTests
{
    public class get_quarantine_vessel_list : ControllerTests
    {
        [Fact]
        public async Task when_contains_one_vessel()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient())
            {
                await UserRepository.InternalGrantPermission( DataContextFactory.CreateContext(), "EmailRestriction", 2, LoggedInUserId, LoggedInCompanyId );

                // Assign
                var objectLookup = await SetupData(@".\TestData\quarantine_vessel_config.json", server);

                var response = await client.GetAsync($"/api/v1/email/settings/quarantine/vessels");

                // Assert
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

                var s = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<QuarantineVesselConfig[]>(s);

                result.Should().NotBeEmpty();

                await UserRepository.InternalRevokePermission( DataContextFactory.CreateContext(), "EmailRestriction", LoggedInUserId, LoggedInCompanyId );
            }
        }
    }
}
