using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.PortalService.Controllers.Quarantine.Model;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using Dualog.PortalService.Controllers.Users;

namespace Dualog.PortalService.Controllers.Quarantine.QuarantineControllerTests
{
    public class get_quarantine_company_config : ControllerTests
    {

        public get_quarantine_company_config()
        {
        }

        [Fact]
        public async Task should_be_ok()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient())
            {
                await UserRepository.InternalGrantPermission( DataContextFactory.CreateContext(), "EmailRestriction", 2, LoggedInUserId, LoggedInCompanyId );

                // Assign
                var response = await client.GetAsync("/api/v1/email/settings/quarantine");

                // Assert
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

                var s = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<QuarantineCompanyConfig>(s);

                result.Should().NotBeNull();

                await UserRepository.InternalRevokePermission( DataContextFactory.CreateContext(), "EmailRestriction", LoggedInUserId, LoggedInCompanyId );
            }
        }
    }
}
