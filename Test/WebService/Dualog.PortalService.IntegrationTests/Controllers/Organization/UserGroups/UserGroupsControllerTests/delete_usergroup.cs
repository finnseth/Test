using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using FluentAssertions;
using System.Net;
using Xunit;
using Dualog.PortalService.Controllers.Organization.Shipping.UserGroup.Model;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.UserGroups.UserGroupsControllerTests
{
    public class delete_usergroup : ControllerTests
    {
        [Fact]
        public async Task existing_usergroup_should_be_ok()
        {
            // The original
            var userGroup = Fixture.Create<UserGroupDetailModel>();
            userGroup.Permissions = null;

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                await GrantPermission(LoggedInUserId, "OrganizationUser", Core.AccessRights.Read); 

                var added = await client.AddAsync(ApiUrl.UserGroupServiceApi, userGroup );

                var result = await client.DeleteAsync($"{ApiUrl.UserGroupServiceApi}/{added.Id}" );
                result.StatusCode.Should().Be( HttpStatusCode.OK );

                var ugResult = await client.GetAsync<GenericDataModel<UserGroupDetailModel>>($"{ApiUrl.UserGroupServiceApi}/{added.Id}", HttpStatusCode.OK);
            }

        }
    }
}
