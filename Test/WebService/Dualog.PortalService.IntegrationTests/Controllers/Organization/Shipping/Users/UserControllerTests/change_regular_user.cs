using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dualog.PortalService.Core;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Ploeh.AutoFixture;
using Xunit;
using Dualog.PortalService.Controllers.Organization.Shipping.User.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.Permission.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.UserGroup.Model;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Users.UserControllerTests
{
    public class change_regular_user : ControllerTests
    {
        [Fact]
        public async Task add_existing_permissions_should_be_ok()
        {
            // Assign
            using (var server = CreateServer())
            using (var client = server.CreateClient())
            {
                await GrantPermission( LoggedInUserId, "OrganizationUser", AccessRights.Read);


                var original = await client.GetAsync<UserDetailModel>($"{ApiUrl.UserApi}/{LoggedInUserId}");

                string json = $@"{{
                                  permissions: {{
                                    add: [
                                      {{name: 'EmailRestriction', allowType: 2}}, 
                                      {{name: 'EmailSetupTechnical', allowType: 1}}
                                    ]
                                  }}
                                }}";

                // Act
                await client.PatchAsync<UserDetailModel>($"{ApiUrl.UserApi}/{LoggedInUserId}", JObject.Parse(json));
                var result = await client.GetAsync<GenericDataModel<UserDetailModel>>($"{ApiUrl.UserApi}/{LoggedInUserId}");

                var permissions = result.Value.Permissions.ToDictionary(k => k.Name, v => v.AllowType);
                permissions.Should().ContainKeys("EmailSetupTechnical", "EmailRestriction");
                permissions["EmailSetupTechnical"].Should().Be(AccessRights.Read);
                permissions["EmailRestriction"].Should().Be(AccessRights.Write);
            }
        }

        [Fact]
        public async Task add_nonexistent_permissions_should_be_badRequest()
        {
            // Assign
            using (var server = CreateServer())
            using (var client = server.CreateClient())
            {
                await GrantPermission(LoggedInUserId, "OrganizationUser", AccessRights.Read);

                var original = await client.GetAsync<UserDetailModel>($"{ApiUrl.UserApi}/{LoggedInUserId}");

                string json = $@"{{
                                  permissions: {{
                                    add: [
                                      {{name: 'NonExistant', allowType: 2}}, 
                                    ]
                                  }}
                                }}";

                // Act
                await client.PatchAsync<UserDetailModel>($"{ApiUrl.UserApi}/{LoggedInUserId}", JObject.Parse(json), HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task delete_existent_permission_should_be_ok()
        {
            var user = Fixture.Create<UserDetailModel>();
            user.Permissions = new List<PermissionDetailModel>
            {
                new PermissionDetailModel{ Name = "NetworkInternetGateway", AllowType = AccessRights.Read },
                new PermissionDetailModel{ Name = "NetworkHTTPRules", AllowType = AccessRights.Write },
            };
            user.UserGroups = new List<UserGroupModel>();

            // Assign
            using (var server = CreateServer())
            using (var client = server.CreateClient())
            {
                await GrantPermission(LoggedInUserId, "OrganizationUser", AccessRights.Read);

                // Act
                var added = await client.AddAsync($"{ApiUrl.UserApi}", user);
                user.Id = added.Id;

                added.Permissions.Should().HaveCount(2);

                string json = $@"{{
                                  permissions: {{
                                    delete: [ 'NetworkInternetGateway', 'NetworkHTTPRules']
                                  }}
                                }}";

                // Act
                var patched = await client.PatchAsync<UserDetailModel>($"{ApiUrl.UserApi}/{added.Id}", JObject.Parse(json), HttpStatusCode.OK);
                patched.Permissions.Should().BeEmpty();

                var result = await client.GetAsync<GenericDataModel<UserDetailModel>>($"{ApiUrl.UserApi}/{added.Id}");
                result.Value.ShouldBeEquivalentTo(patched);
            }
        }

        [Fact]
        public async Task delete_nonexistent_permission_should_be_bad_request()
        {
            var user = Fixture.Create<UserDetailModel>();
            user.Permissions = new List<PermissionDetailModel>
            {
                new PermissionDetailModel{ Name = "EmailOperation", AllowType = AccessRights.Read },
                new PermissionDetailModel{ Name = "EmailSetting", AllowType = AccessRights.Write },
            };
            user.UserGroups = new List<UserGroupModel>();

            // Assign
            using (var server = CreateServer())
            using (var client = server.CreateClient())
            {

                await GrantPermission(LoggedInUserId, "OrganizationUser", AccessRights.Read);

                // Act
                var added = await client.AddAsync($"{ApiUrl.UserApi}", user);
                user.Id = added.Id;

                added.Permissions.Should().HaveCount(2);

                string json = $@"{{
                                  permissions: {{
                                    delete: [ 'NonExistent' ]
                                  }}
                                }}";

                // Act
                var patched = await client.PatchAsync<UserDetailModel>($"{ApiUrl.UserApi}/{added.Id}", JObject.Parse(json), HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task change_existent_permission_should_be_ok()
        {
            var user = Fixture.Create<UserDetailModel>();
            user.Permissions = new List<PermissionDetailModel>
            {
                new PermissionDetailModel{ Name = "EmailSetupDelivery", AllowType = AccessRights.Read },
                new PermissionDetailModel{ Name = "NetworkHTTPRules", AllowType = AccessRights.Write },
            };
            user.UserGroups = new List<UserGroupModel>();

            var originalPermissions = user.Permissions.ToDictionary(k => k.Name, v => v.AllowType);

            // Assign
            using (var server = CreateServer())
            using (var client = server.CreateClient())
            {
                await GrantPermission(LoggedInUserId, "OrganizationUser", AccessRights.Read);

                // Act
                var added = await client.AddAsync($"{ApiUrl.UserApi}", user);
                user.Id = added.Id;

                added.Permissions.Should().HaveCount(2);

                string json = $@"{{
                                  permissions: {{
                                    update: {{
                                      'EmailSetupDelivery': {{ AllowType: 2 }},
                                      'NetworkHTTPRules': {{ AllowType: 1 }} 
                                    }}
                                  }}
                                }}";

                // Act
                var patched = await client.PatchAsync<UserDetailModel>($"{ApiUrl.UserApi}/{added.Id}", JObject.Parse(json), HttpStatusCode.OK);
                var patchedPermissions = patched.Permissions.ToDictionary(k => k.Name, v => v.AllowType);

                var stored = await client.GetAsync<GenericDataModel<UserDetailModel>>($"{ApiUrl.UserApi}/{added.Id}");
                var storedPermissions = stored.Value.Permissions.ToDictionary(k => k.Name, v => v.AllowType);

                storedPermissions.ShouldBeEquivalentTo(patchedPermissions);
            }
        }

        [Fact]
        public async Task change_nonexistent_permission_should_be_bad_request()
        {
            var user = Fixture.Create<UserDetailModel>();
            user.Permissions = new List<PermissionDetailModel>
            {
                new PermissionDetailModel{ Name = "EmailOperation", AllowType = AccessRights.Read },
                new PermissionDetailModel{ Name = "EmailSetting", AllowType = AccessRights.Write },
            };
            user.UserGroups = new List<UserGroupModel>();

            var originalPermissions = user.Permissions.ToDictionary(k => k.Name, v => v.AllowType);

            // Assign
            using (var server = CreateServer())
            using (var client = server.CreateClient())
            {
                await GrantPermission(LoggedInUserId, "OrganizationUser", AccessRights.Read);

                // Act
                var added = await client.AddAsync($"{ApiUrl.UserApi}", user);
                user.Id = added.Id;

                added.Permissions.Should().HaveCount(2);

                string json = $@"{{
                                  permissions: {{
                                    update: {{
                                      'NonExistent': {{ AllowType: 2 }},
                                    }}
                                  }}
                                }}";

                // Act
                var patched = await client.PatchAsync<UserDetailModel>($"{ApiUrl.UserApi}/{added.Id}", JObject.Parse(json), HttpStatusCode.BadRequest);
            }
        }


        [Fact]
        public async Task nonexistent_user_should_be_not_found()
        {
            // Assign
            using (var server = CreateServer())
            using (var client = server.CreateClient())
            {
                await GrantPermission(LoggedInUserId, "OrganizationUser", AccessRights.Read);

                var original = await client.GetAsync<UserDetailModel>($"{ApiUrl.UserApi}/{LoggedInUserId}");

                string json = $@"{{
                                    name: 'Test User',
                                 }}";

                // Act
                await client.PatchAsync<UserDetailModel>($"{ApiUrl.UserApi}/1", JObject.Parse(json), HttpStatusCode.NotFound);
            }
        }


        [Fact]
        public async Task json_format()
        {
            // Assign
            using (var server = CreateServer())
            using (var client = server.CreateClient())
            {
                await GrantPermission(LoggedInUserId, "OrganizationUser", AccessRights.Read);

                var original = await client.GetAsync<UserDetailModel>($"{ApiUrl.UserApi}/{LoggedInUserId}");

                string json = $@"{{
                                    ""name"": ""Test User"",
                                 }}";

                // Act
                await client.PatchAsync<UserDetailModel>($"{ApiUrl.UserApi}/1", JObject.Parse(json), HttpStatusCode.NotFound);
            }
        }


        [Fact]
        public async Task add_usergroups_should_be_ok()
        {
            // Assign
            using (var server = CreateServer())
            using (var client = server.CreateClient())
            {
                await GrantPermission(LoggedInUserId, "OrganizationUser", AccessRights.Read);

                var original = await client.GetAsync<UserDetailModel>($"{ApiUrl.UserApi}/{LoggedInUserId}");


                // Act
                var id1 = (await AddUSerGroup(client)).Id;
                var id2 = (await AddUSerGroup(client)).Id;

                string json = $@"{{
                                  userGroups: {{
                                    add: [
                                      {{id: {id1} }}, 
                                      {{id: {id2} }}, 
                                    ]
                                  }}
                                }}";

                await client.PatchAsync<UserDetailModel>($"{ApiUrl.UserApi}/{LoggedInUserId}", JObject.Parse(json));
                var result = await client.GetAsync<GenericDataModel<UserDetailModel>>($"{ApiUrl.UserApi}/{LoggedInUserId}");

                var permissions = result.Value.UserGroups.Select(ug => ug.Id);
                permissions.Should().Contain(new long[] { id1, id2 });
            }
        }


        [Fact]
        public async Task delete_usergroups_should_be_ok()
        {
            // Assign
            using (var server = CreateServer())
            using (var client = server.CreateClient())
            {
                await GrantPermission(LoggedInUserId, "OrganizationUser", AccessRights.Write);

                var id1 = (await AddUSerGroup(client)).Id;
                var id2 = (await AddUSerGroup(client)).Id;

                string json = $@"{{
                                  userGroups: {{
                                    add: [
                                      {{id: {id1} }}, 
                                      {{id: {id2} }}, 
                                    ]
                                  }}
                                }}";

                await client.PatchAsync<UserDetailModel>($"{ApiUrl.UserApi}/{LoggedInUserId}", JObject.Parse(json));

                var original = await client.GetAsync<UserDetailModel>($"{ApiUrl.UserApi}/{LoggedInUserId}");

                json = $@"{{
                            userGroups: {{
                            delete: [ {id1}, {id2} ],                                     
                            }}
                        }}";

                // Act
                await client.PatchAsync<UserDetailModel>($"{ApiUrl.UserApi}/{LoggedInUserId}", JObject.Parse(json));
                var result = await client.GetAsync<GenericDataModel<UserDetailModel>>($"{ApiUrl.UserApi}/{LoggedInUserId}");

                var permissions = result.Value.UserGroups.Select(ug => ug.Id);
                permissions.Should().NotContain(new long[] { id1, id2 });
            }
        }


        private async Task<UserGroupDetailModel> AddUSerGroup(HttpClient client)
        {
            var userGroup = Fixture.Create<UserGroupDetailModel>();
            userGroup.Permissions = new List<PermissionDetailModel>()
            {
                new PermissionDetailModel{ Name = "OrganizationUser", AllowType = AccessRights.Read },
                new PermissionDetailModel{ Name = "OrganizationUserGroup", AllowType = AccessRights.Write },
            };

            return await client.AddAsync($"{ApiUrl.UserApi}group", userGroup);
        }

    }
}
