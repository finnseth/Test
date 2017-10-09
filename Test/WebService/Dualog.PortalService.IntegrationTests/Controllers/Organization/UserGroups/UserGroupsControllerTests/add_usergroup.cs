using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.PortalService.Controllers.Organization.Shipping.Permission.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.UserGroup.Model;
using Dualog.PortalService.Core;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.UserGroups.UserGroupsControllerTests
{
    public class add_usergroup : ControllerTests
    {
        [Fact]
        public async Task to_company_with_all_properties()
        {
            await GrantPermission(LoggedInUserId, "OrganizationUser", AccessRights.Write);

            var userGroup = Fixture.Create<UserGroupDetailModel>();
            userGroup.Permissions = new List<PermissionDetailModel>()
            {
                new PermissionDetailModel{ Name = "AntiVirusManagement", AllowType = AccessRights.Read },
                new PermissionDetailModel{ Name = "EmailAddressing", AllowType = AccessRights.Write },
            };

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var result = await client.AddAsync(ApiUrl.UserGroupServiceApi, userGroup );
                userGroup.Id = result.Id;

                result.ShouldBeEquivalentTo( userGroup );

                var stored = await client.GetAsync<GenericDataModel<UserGroupDetailModel>>($"{ApiUrl.UserGroupServiceApi}/{userGroup.Id}" );
                stored.Value.ShouldBeEquivalentTo(
                    userGroup,
                    o => o.Excluding(p => p.CompanyId)
                          .Excluding(p => p.VesselId)
                          .Excluding(p => p.VesselName)
                          .Excluding(p => p.CompanyName)
                          .Excluding(p => p.Members) );
            }
        }

        [Fact]
        public async Task to_company_with_nonexistent_permission_bad_request()
        {
            var userGroup = Fixture.Create<UserGroupDetailModel>();
            userGroup.Permissions = new List<PermissionDetailModel>()
            {
                new PermissionDetailModel{ Name = "EmailAddressing", AllowType = AccessRights.Read },
                new PermissionDetailModel{ Name = "BantiBantBant", AllowType = AccessRights.Write },
            };

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var result = await client.AddAsync(ApiUrl.UserGroupServiceApi, userGroup, HttpStatusCode.BadRequest );
            }
        }

    }

}
