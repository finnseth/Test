using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.PortalService.Controllers.Users.Model;
using FluentAssertions;
using Xunit;
using Newtonsoft.Json.Linq;
using Ploeh.AutoFixture;
using Dualog.PortalService.Controllers.Permissions.Model;
using Dualog.PortalService.Core;

namespace Dualog.PortalService.Controllers.Users.UserControllerTests
{
    public class change_regular_user : ControllerTests
    {
        [Fact]
        public async Task add_existing_permissions_should_be_ok()
        {
            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var original = await client.GetAsync<UserDetailsModel>( $"api/v1/users/{LoggedInUserId}" );

                string json = $@"{{
                                  permissions: {{
                                    add: [
                                      {{name: 'EmailOperation', allowType: 2}}, 
                                      {{name: 'EmailRestriction', allowType: 1}}
                                    ]
                                  }}
                                }}";

                // Act
                await client.PatchAsync<UserDetailsModel>( $"api/v1/users/{LoggedInUserId}", JObject.Parse(json) );
                var result = await client.GetAsync<UserDetailsModel>( $"api/v1/users/{LoggedInUserId}" );

                var permissions = result.Permissions.ToDictionary( k => k.Name, v => v.AllowType );
                permissions.Should().ContainKeys( "EmailOperation", "EmailRestriction" );
                permissions[ "EmailOperation" ].Should().Be( 2 );
                permissions[ "EmailRestriction" ].Should().Be( 1 );
            }
        }

        [Fact]
        public async Task add_nonexistent_permissions_should_be_badRequest()
        {
            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var original = await client.GetAsync<UserDetailsModel>( $"api/v1/users/{LoggedInUserId}" );

                string json = $@"{{
                                  permissions: {{
                                    add: [
                                      {{name: 'NonExistant', allowType: 2}}, 
                                    ]
                                  }}
                                }}";

                // Act
                await client.PatchAsync<UserDetailsModel>( $"api/v1/users/{LoggedInUserId}", JObject.Parse( json ), HttpStatusCode.BadRequest );
            }
        }

        [Fact]
        public async Task delete_existent_permission_should_be_ok()
        {
            var user = Fixture.Create<UserDetailsModel>();
            user.Permissions = new List<PermissionDetails>
            {
                new PermissionDetails{ Name = "EmailOperation", AllowType = AccessRights.Read },
                new PermissionDetails{ Name = "EmailSetting", AllowType = AccessRights.Write },
            };
            user.UserGroups = new List<UserGroupModel>();

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                // Act
                var added = await client.AddAsync( "api/v1/users", user );
                user.Id = added.Id;

                added.Permissions.Should().HaveCount( 2 );

                string json = $@"{{
                                  permissions: {{
                                    delete: [ 'EmailOperation', 'EmailSetting']
                                  }}
                                }}";

                // Act
                var patched = await client.PatchAsync<UserDetailsModel>( $"api/v1/users/{added.Id}", JObject.Parse( json ), HttpStatusCode.OK );
                patched.Permissions.Should().BeEmpty();

                var result = await client.GetAsync<UserDetailsModel>($"api/v1/users/{added.Id}");
                result.ShouldBeEquivalentTo( patched );
            }
        }

        [Fact]
        public async Task delete_nonexistent_permission_should_be_bad_request()
        {
            var user = Fixture.Create<UserDetailsModel>();
            user.Permissions = new List<PermissionDetails>
            {
                new PermissionDetails{ Name = "EmailOperation", AllowType = AccessRights.Read },
                new PermissionDetails{ Name = "EmailSetting", AllowType = AccessRights.Write },
            };
            user.UserGroups = new List<UserGroupModel>();

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                // Act
                var added = await client.AddAsync( "api/v1/users", user );
                user.Id = added.Id;

                added.Permissions.Should().HaveCount( 2 );

                string json = $@"{{
                                  permissions: {{
                                    delete: [ 'NonExistent' ]
                                  }}
                                }}";

                // Act
                var patched = await client.PatchAsync<UserDetailsModel>( $"api/v1/users/{added.Id}", JObject.Parse( json ), HttpStatusCode.BadRequest );
            }
        }

        [Fact]
        public async Task change_existent_permission_should_be_ok()
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

                added.Permissions.Should().HaveCount( 2 );

                string json = $@"{{
                                  permissions: {{
                                    update: {{
                                      'EmailOperation': {{ AllowType: 2 }},
                                      'EmailSetting': {{ AllowType: 1 }} 
                                    }}
                                  }}
                                }}";

                // Act
                var patched = await client.PatchAsync<UserDetailsModel>( $"api/v1/users/{added.Id}", JObject.Parse( json ), HttpStatusCode.OK );
                var patchedPermissions = patched.Permissions.ToDictionary( k => k.Name, v => v.AllowType );

                var stored = await client.GetAsync<UserDetailsModel>($"api/v1/users/{added.Id}");
                var storedPermissions = stored.Permissions.ToDictionary( k => k.Name, v => v.AllowType );

                storedPermissions.ShouldBeEquivalentTo( patchedPermissions );
            }
        }

        [Fact]
        public async Task change_nonexistent_permission_should_be_bad_request()
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

                added.Permissions.Should().HaveCount( 2 );

                string json = $@"{{
                                  permissions: {{
                                    update: {{
                                      'NonExistent': {{ AllowType: 2 }},
                                    }}
                                  }}
                                }}";

                // Act
                var patched = await client.PatchAsync<UserDetailsModel>( $"api/v1/users/{added.Id}", JObject.Parse( json ), HttpStatusCode.BadRequest );
            }
        }


        [Fact]
        public async Task nonexistent_user_should_be_not_found()
        {
            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var original = await client.GetAsync<UserDetailsModel>( $"api/v1/users/{LoggedInUserId}" );

                string json = $@"{{
                                    name: 'Test User',
                                 }}";

                // Act
                await client.PatchAsync<UserDetailsModel>( $"api/v1/users/1", JObject.Parse( json ), HttpStatusCode.NotFound );
            }
        }


        [Fact]
        public async Task json_format()
        {
            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var original = await client.GetAsync<UserDetailsModel>( $"api/v1/users/{LoggedInUserId}" );

                string json = $@"{{
                                    ""name"": ""Test User"",
                                 }}";

                // Act
                await client.PatchAsync<UserDetailsModel>( $"api/v1/users/1", JObject.Parse( json ), HttpStatusCode.NotFound );
            }
        }

    }
}
