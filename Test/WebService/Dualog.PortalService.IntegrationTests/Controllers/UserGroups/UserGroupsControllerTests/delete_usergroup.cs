using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Dualog.PortalService.Controllers.UserGroups.Model;
using FluentAssertions;
using System.Net;
using Xunit;

namespace Dualog.PortalService.Controllers.UserGroups.UserGroupsControllerTests
{
    public class delete_usergroup : ControllerTests
    {
        [Fact]
        public async Task existing_usergroup_should_be_ok()
        {
            // The original
            var userGroup = Fixture.Create<UserGroupDetails>();
            userGroup.Permissions = null;

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var added = await client.AddAsync("/api/v1/userGroups", userGroup );

                var result = await client.DeleteAsync($"/api/v1/userGroups/{added.Id}" );
                result.StatusCode.Should().Be( HttpStatusCode.OK );

                var ugResult = await client.GetAsync<UserGroupDetails>($"/api/v1/userGroups/{added.Id}", HttpStatusCode.OK);
            }

        }
    }
}
