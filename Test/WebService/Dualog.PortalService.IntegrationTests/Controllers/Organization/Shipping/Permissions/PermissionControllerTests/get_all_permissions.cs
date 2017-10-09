using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Permissions.PermissionControllerTests
{
    public class get_all_permissions : ControllerTests
    {
        [Fact]
        public async Task should_be_ok()
        {
            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient())
            {
                // Act
                var response = await client.GetAsync("/api/v1/organization/shipping/permission");

                // Assert
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task regular_user_permissions()
        {
            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient())
            {
                // Act
                var response = await client.GetAsync("/api/v1/organization/shipping/permission");

                // Assert
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            }
        }
    }
}
