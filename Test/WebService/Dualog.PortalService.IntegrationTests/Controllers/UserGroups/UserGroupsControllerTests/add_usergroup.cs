using Dualog.PortalService.Controllers.UserGroups.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Xunit;
using FluentAssertions;
using Dualog.PortalService.Controllers.Users.Model;
using System.Net;
using Dualog.PortalService.Controllers.Permissions.Model;
using Dualog.PortalService.Core;

namespace Dualog.PortalService.Controllers.UserGroups.UserGroupsControllerTests
{
    public class add_usergroup : ControllerTests
    {
        [Fact]
        public async Task to_company_with_all_properties()
        {
            var userGroup = Fixture.Create<UserGroupDetails>();
            userGroup.Permissions = new List<PermissionDetails>()
            {
                new PermissionDetails{ Name = "EmailSetting", AllowType = AccessRights.Read },
                new PermissionDetails{ Name = "EmailDistributionlist", AllowType = AccessRights.Write },
            };

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var result = await client.AddAsync( "/api/v1/usersGroups", userGroup );
                userGroup.Id = result.Id;

                result.ShouldBeEquivalentTo( userGroup );

                var stored = await client.GetAsync<UserGroupDetails>($"/api/v1/usersGroups/{userGroup.Id}" );
                stored.ShouldBeEquivalentTo( userGroup );
            }
        }

        [Fact]
        public async Task to_company_with_nonexistent_permission_bad_request()
        {
            var userGroup = Fixture.Create<UserGroupDetails>();
            userGroup.Permissions = new List<PermissionDetails>()
            {
                new PermissionDetails{ Name = "EmailSetting", AllowType = AccessRights.Read },
                new PermissionDetails{ Name = "BantiBantBant", AllowType = AccessRights.Write },
            };

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var result = await client.AddAsync( "/api/v1/usersGroups", userGroup, HttpStatusCode.BadRequest );
            }
        }

    }

}
