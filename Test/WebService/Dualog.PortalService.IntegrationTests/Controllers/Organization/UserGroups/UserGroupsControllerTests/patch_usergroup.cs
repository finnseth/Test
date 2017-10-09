using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.PortalService.Core;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Ploeh.AutoFixture;
using Xunit;
using Dualog.PortalService.Controllers.Organization.Shipping.UserGroup.Model;
using Dualog.PortalService.Controllers.Network.Setup.Services.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.Permission.Model;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.UserGroups.UserGroupsControllerTests
{
    public class patch_usergroup : ControllerTests
    {
        [Fact]
        public async Task with_all_properties()
        {
            // The original
            //var userGroup = Fixture.Create<UserGroupDetailModel>();
            //userGroup.Permissions = null;

            //// The patch
            //var ugPatch = Fixture.Create<UserGroupDetailModel>();
            //ugPatch.Permissions = null;

            //var patch = $@"{{
            //                  allowFax: '{ugPatch.AllowFax}',
            //                  allowSms: '{ugPatch.AllowSms}',
            //                  allowTelex: '{ugPatch.AllowTelex}',
            //                  attachmentRule: '{ugPatch.AttachmentRule}',
            //                  concurrentDevices: '{ugPatch.ConcurrentDevices}',
            //                  daysAutoSignOff: '{ugPatch.DaysAutoSignOff}',
            //                  daysToKeepMessages: '{ugPatch.DaysToKeepMessages}',
            //                  deleteOldMessages: '{ugPatch.DeleteOldMessages}',
            //                  description: '{ugPatch.Description}',
            //                  faxDeliveryReport: '{ugPatch.FaxDeliveryReport}',
            //                  faxNotDeliveredReport: '{ugPatch.FaxNotDeliveredReport}',
            //                  ipTimeout: '{ugPatch.IpTimeout}',
            //                  minutesLoginPerDay: '{ugPatch.MinutesLoginPerDay}',
            //                  name: '{ugPatch.Name}',
            //                  needApproval: '{ugPatch.NeedApproval}',
            //                  signinApproval: '{ugPatch.SigninApproval}',
            //                  smsDeliveryReport: '{ugPatch.SmsDeliveryReport}',
            //                  smsNotDeliveredReport: '{ugPatch.SmsNotDeliveredReport}',
            //                  smtpRelay: '{ugPatch.SmtpRelay}',
            //                  telexDeliveryReport: '{ugPatch.TelexDeliveryReport}',
            //                  telexNotDeliveredReport: '{ugPatch.TelexNotDeliveredReport}',
            //                  useImap: '{ugPatch.UseImap}',
            //                  usePop: '{ugPatch.UsePop}',
            //               }}";


            //// Assign
            //using( var server = CreateServer() )
            //using( var client = server.CreateClient() )
            //{
            //    var added = await client.AddAsync("{ApiUrl.UserGroupServiceApi}", userGroup );
            //    userGroup.Id = added.Id;
            //    ugPatch.Id = added.Id;

            //    var patched = await client.PatchAsync<UserGroupDetailModel>($"{ApiUrl.UserGroupServiceApi}/{added.Id}", JObject.Parse( patch ));

            //    var stored = await client.GetAsync<UserGroupDetailModel>($"{ApiUrl.UserGroupServiceApi}/{userGroup.Id}");
            //    stored.ShouldBeEquivalentTo( patched );
            //}
        }

        [Fact]
        public async Task change_permissions()
        {
            var userGroup = Fixture.Create<UserGroupDetailModel>();
            userGroup.Permissions = new List<PermissionDetailModel>()
            {
                new PermissionDetailModel{ Name = "OrganizationShip", AllowType = AccessRights.Read },
                new PermissionDetailModel{ Name = "FileTransferSetup", AllowType = AccessRights.Write },
            };

            var patch = $@"{{
                              permissions: {{
                                update: {{
                                  'OrganizationShip': {{ allowType: 1 }},
                                  'FileTransferSetup': {{ allowType: 2 }}
                                }}
                              }}
                           }}";


            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                await GrantPermission(LoggedInUserId, "OrganizationUser", AccessRights.Write);

                var added = await client.AddAsync($"{ApiUrl.UserGroupServiceApi}", userGroup );
                userGroup.Id = added.Id;

                var patched = await client.PatchAsync<UserGroupDetailModel>($"{ApiUrl.UserGroupServiceApi}/{added.Id}", JObject.Parse( patch ));

                var stored = await client.GetAsync<GenericDataModel<UserGroupDetailModel>>($"{ApiUrl.UserGroupServiceApi}/{userGroup.Id}");
                stored.Value.ShouldBeEquivalentTo( patched,
                    o => o.Excluding(p => p.CompanyId)
                          .Excluding(p => p.VesselId)
                          .Excluding(p => p.VesselName)
                          .Excluding(p => p.CompanyName)
                          .Excluding(p => p.Rowstatus)
                          .Excluding(p => p.Members));


                var permissions = stored.Value.Permissions.ToDictionary( k => k.Name, v => v.AllowType);

                permissions.Count().Should().Be( 2 );
                permissions.Should().ContainKeys("OrganizationShip", "FileTransferSetup");
                permissions.Should().NotContainKey( "EmailSetting" );
            }
        }

        [Fact]
        public async Task change_permissions_nonexistent_should_be_bad_request()
        {
            var userGroup = Fixture.Create<UserGroupDetailModel>();
            userGroup.Permissions = new List<PermissionDetailModel>()
            {
                new PermissionDetailModel{ Name = "NetworkProxy", AllowType = AccessRights.Read },
                new PermissionDetailModel{ Name = "AntiVirusManagement", AllowType = AccessRights.Write },
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
                var added = await client.AddAsync(ApiUrl.UserGroupServiceApi, userGroup );
                userGroup.Id = added.Id;

                var patched = await client.PatchAsync<UserGroupDetailModel>($"{ApiUrl.UserGroupServiceApi}/{added.Id}", JObject.Parse( patch ), HttpStatusCode.BadRequest );
            }
        }

        [Fact]
        public async Task change_permissions_nonexistent_should_be_not_found()
        {
            var userGroup = Fixture.Create<UserGroupDetailModel>();
            userGroup.Permissions = null;

            var patch = $@"{{
                              allowFax: true,
                           }}";


            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var added = await client.AddAsync(ApiUrl.UserGroupServiceApi, userGroup );
                userGroup.Id = added.Id;

                var patched = await client.PatchAsync<UserGroupDetailModel>($"{ApiUrl.UserGroupServiceApi}/{1}", JObject.Parse( patch ), HttpStatusCode.NotFound );
            }
        }


    }
}
