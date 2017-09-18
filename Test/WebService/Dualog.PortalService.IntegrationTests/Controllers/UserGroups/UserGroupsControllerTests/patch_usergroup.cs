using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.PortalService.Controllers.Permissions.Model;
using Dualog.PortalService.Controllers.UserGroups.Model;
using Dualog.PortalService.Core;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Ploeh.AutoFixture;
using Xunit;

namespace Dualog.PortalService.Controllers.UserGroups.UserGroupsControllerTests
{
    public class patch_usergroup : ControllerTests
    {
        [Fact]
        public async Task with_all_properties()
        {
            // The original
            var userGroup = Fixture.Create<UserGroupDetails>();
            userGroup.Permissions = null;

            // The patch
            var ugPatch = Fixture.Create<UserGroupDetails>();
            ugPatch.Permissions = null;

            var patch = $@"{{
                              allowFax: '{ugPatch.AllowFax}',
                              allowSms: '{ugPatch.AllowSms}',
                              allowTelex: '{ugPatch.AllowTelex}',
                              attachmentRule: '{ugPatch.AttachmentRule}',
                              concurrentDevices: '{ugPatch.ConcurrentDevices}',
                              daysAutoSignOff: '{ugPatch.DaysAutoSignOff}',
                              daysToKeepMessages: '{ugPatch.DaysToKeepMessages}',
                              deleteOldMessages: '{ugPatch.DeleteOldMessages}',
                              description: '{ugPatch.Description}',
                              faxDeliveryReport: '{ugPatch.FaxDeliveryReport}',
                              faxNotDeliveredReport: '{ugPatch.FaxNotDeliveredReport}',
                              ipTimeout: '{ugPatch.IpTimeout}',
                              minutesLoginPerDay: '{ugPatch.MinutesLoginPerDay}',
                              name: '{ugPatch.Name}',
                              needApproval: '{ugPatch.NeedApproval}',
                              signinApproval: '{ugPatch.SigninApproval}',
                              smsDeliveryReport: '{ugPatch.SmsDeliveryReport}',
                              smsNotDeliveredReport: '{ugPatch.SmsNotDeliveredReport}',
                              smtpRelay: '{ugPatch.SmtpRelay}',
                              telexDeliveryReport: '{ugPatch.TelexDeliveryReport}',
                              telexNotDeliveredReport: '{ugPatch.TelexNotDeliveredReport}',
                              useImap: '{ugPatch.UseImap}',
                              usePop: '{ugPatch.UsePop}',
                           }}";


            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var added = await client.AddAsync("/api/v1/userGroups", userGroup );
                userGroup.Id = added.Id;
                ugPatch.Id = added.Id;

                var patched = await client.PatchAsync<UserGroupDetails>($"/api/v1/userGroups/{added.Id}", JObject.Parse( patch ));

                var stored = await client.GetAsync<UserGroupDetails>($"/api/v1/userGroups/{userGroup.Id}");
                stored.ShouldBeEquivalentTo( patched );
            }
        }

        [Fact]
        public async Task change_permissions()
        {
            var userGroup = Fixture.Create<UserGroupDetails>();
            userGroup.Permissions = new List<PermissionDetails>()
            {
                new PermissionDetails{ Name = "EmailSetting", AllowType = AccessRights.Read },
                new PermissionDetails{ Name = "EmailDistributionlist", AllowType = AccessRights.Write },
            };

            var patch = $@"{{
                              permissions: {{
                                update: {{
                                  'EmailSetting': {{ allowType: 0 }},
                                  'EmailAddressbook': {{ allowType: 2 }}
                                }}
                              }}
                           }}";


            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var added = await client.AddAsync("/api/v1/userGroups", userGroup );
                userGroup.Id = added.Id;

                var patched = await client.PatchAsync<UserGroupDetails>($"/api/v1/userGroups/{added.Id}", JObject.Parse( patch ));

                var stored = await client.GetAsync<UserGroupDetails>($"/api/v1/userGroups/{userGroup.Id}");
                stored.ShouldBeEquivalentTo( patched );


                var permissions = stored.Permissions.ToDictionary( k => k.Name, v => v.AllowType);

                permissions.Count().Should().Be( 2 );
                permissions.Should().ContainKeys( "EmailDistributionlist", "EmailAddressbook" );
                permissions.Should().NotContainKey( "EmailSetting" );
            }
        }

        [Fact]
        public async Task change_permissions_nonexistent_should_be_bad_request()
        {
            var userGroup = Fixture.Create<UserGroupDetails>();
            userGroup.Permissions = new List<PermissionDetails>()
            {
                new PermissionDetails{ Name = "EmailSetting", AllowType = AccessRights.Read },
                new PermissionDetails{ Name = "EmailDistributionlist", AllowType = AccessRights.Write },
            };

            var patch = $@"{{
                              permissions: {{
                                update: {{
                                  'BantiBant': {{ allowType: 0 }},
                                }}
                              }}
                           }}";


            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var added = await client.AddAsync("/api/v1/userGroups", userGroup );
                userGroup.Id = added.Id;

                var patched = await client.PatchAsync<UserGroupDetails>($"/api/v1/userGroups/{added.Id}", JObject.Parse( patch ), HttpStatusCode.BadRequest );
            }
        }

        [Fact]
        public async Task change_permissions_nonexistent_should_be_not_found()
        {
            var userGroup = Fixture.Create<UserGroupDetails>();
            userGroup.Permissions = null;

            var patch = $@"{{
                              allowFax: true,
                           }}";


            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var added = await client.AddAsync("/api/v1/userGroups", userGroup );
                userGroup.Id = added.Id;

                var patched = await client.PatchAsync<UserGroupDetails>($"/api/v1/userGroups/{1}", JObject.Parse( patch ), HttpStatusCode.NotFound );
            }
        }


    }
}
