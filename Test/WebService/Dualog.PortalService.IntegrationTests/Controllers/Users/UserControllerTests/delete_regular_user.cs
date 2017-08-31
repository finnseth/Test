using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.PortalService.Controllers.Users.Model;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;
using Dualog.PortalService.Core;
using Dualog.PortalService.Controllers.Permissions.Model;

namespace Dualog.PortalService.Controllers.Users.UserControllerTests
{
    public class delete_regular_user : ControllerTests
    {
        [Fact]
        public async Task where_user_exists_is_ok()
        {
            var user = Fixture.Create<UserDetailsModel>();
            user.Permissions = new List<PermissionDetails>
            {
                new PermissionDetails{ Name = "EmailOperation", AllowType = AccessRights.Read },
                new PermissionDetails{ Name = "EmailSetting", AllowType = AccessRights.Write },
            };
            user.UserGroups = new List<UserGroupModel>();

            var originalPermissions = user.Permissions.ToDictionary( k => k.Name, v => v.AllowType );

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                // Act
                var added = await client.AddAsync( "api/v1/users", user );
                user.Id = added.Id;

                var result  = await client.DeleteAsync( $"api/v1/users/{user.Id}" );
                result.StatusCode.Should().Be( HttpStatusCode.OK );

            }

        }
    }
}
