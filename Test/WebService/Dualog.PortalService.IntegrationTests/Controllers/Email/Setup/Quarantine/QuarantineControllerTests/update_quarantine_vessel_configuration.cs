using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dualog.PortalService.Controllers.Email.Setup.Quarantine.Model;
using Dualog.PortalService.Controllers.Users;
using Dualog.PortalService.Controllers.Vessels;
using Dualog.PortalService.Controllers.Vessels.Model;
using FluentAssertions;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using Xunit;
using System.Net.Mail;

namespace Dualog.PortalService.Controllers.Email.Setup.Quarantine.QuarantineControllerTests
{
    public class update_quarantine_vessel_configuration : ControllerTests
    {
        public update_quarantine_vessel_configuration()
        {
            Fixture.Customize<QuarantineVesselModel>(c => c.With(p => p.NotificationSender, Fixture.Create<MailAddress>().Address));
            Fixture.Customize<QuarantineVesselModel>(c => c.With(p => p.NotificationOnHoldAdmins, Fixture.Create<MailAddress>().Address));

        }

        [Fact]
        public async Task update_all_fields_with_valid_values()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient())
            {
                await UserRepository.InternalGrantPermission( DataContextFactory.CreateContext(), "EmailRestriction", 2, LoggedInUserId, LoggedInCompanyId );

                // Assign
                var objectLookup = await SetupData(@".\TestData\quarantine_vessel_config.json", server);
                var vessel = objectLookup.GetObjectById<VesselDetails>("v1");

                var original = Fixture.Create<QuarantineCompanyModel>();

                var content = new StringContent(JsonConvert.SerializeObject(original));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var cmpResponse = await client.GetAsync($"/api/v1/email/setup/quarantine/shipquarantine/{vessel.Id}");
                var cmpQuarantine = JsonConvert.DeserializeObject<QuarantineCompanyModel>(await cmpResponse.Content.ReadAsStringAsync());


                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/v1/email/setup/quarantine/shipquarantine/{cmpQuarantine.QuarantineId}");
                request.Content = content;

                // Act
                var response = await client.SendAsync(request);
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

                var result = JsonConvert.DeserializeObject<QuarantineCompanyModel>(await response.Content.ReadAsStringAsync());


                // Assert
                result.Should().NotBeNull();
            }
        }


        [Fact]
        public async Task update_with_invalid_value_should_be_bad_request()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient())
            {
                await UserRepository.InternalGrantPermission( DataContextFactory.CreateContext(), "EmailRestriction", 2, LoggedInUserId, LoggedInCompanyId );

                var objectLookup = await SetupData(@".\TestData\quarantine_vessel_config.json", server);
                var vessel = objectLookup.GetObjectById<VesselDetails>("v1");


                var original = Fixture.Create<QuarantineCompanyModel>();
                original.MaxBodyLength = 9999999;

                var content = new StringContent(JsonConvert.SerializeObject(original));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var cmpResponse = await client.GetAsync($"/api/v1/email/setup/quarantine/shipquarantine/{vessel.Id}");
                var cmpQuarantine = JsonConvert.DeserializeObject<QuarantineCompanyModel>(await cmpResponse.Content.ReadAsStringAsync());


                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/v1/email/setup/quarantine/shipquarantine/{cmpQuarantine.QuarantineId}");
                request.Content = content;

                // Act
                var response = await client.SendAsync(request);
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);

                await UserRepository.InternalRevokePermission( DataContextFactory.CreateContext(), "EmailRestriction", LoggedInUserId, LoggedInCompanyId );
            }
        }


        protected override async void OnDispose()
        {
            var vesselRepo = new VesselRepository(DataContextFactory);
            foreach( var v in await vesselRepo.GetVessels( 2597, null ) )
            {
                await vesselRepo.DeleteVesselAsync(v.Id);
            }
        }

    }


}
