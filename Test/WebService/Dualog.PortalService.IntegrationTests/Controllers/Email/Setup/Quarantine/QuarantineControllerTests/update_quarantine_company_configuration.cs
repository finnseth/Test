using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dualog.PortalService.Controllers.Email.Setup.Quarantine.Model;
using FluentAssertions;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using Xunit;
using System.Net.Mail;
using Dualog.PortalService.Controllers.Organization.Shipping.User;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.Email.Setup.Quarantine.QuarantineControllerTests
{
    public class update_quarantine_company_configuration : ControllerTests
    {

        public update_quarantine_company_configuration()
        {
            Fixture.Customize<QuarantineCompanyModel>(c => c.With(p => p.NotificationSender, Fixture.Create<MailAddress>().Address));
            Fixture.Customize<QuarantineCompanyModel>(c => c.With(p => p.NotificationOnHoldAdmins, Fixture.Create<MailAddress>().Address));
        }

        [Fact]
        public async Task update_all_fields_with_valid_values()
        {

            // Assign
            var original = Fixture.Create<QuarantineCompanyModel>();

            using( var server = CreateServer() )
            using( var client = server.CreateClient())
            {
                await UserRepository.InternalGrantPermission( DataContextFactory.CreateContext(), "EmailRestriction", 2, LoggedInUserId, LoggedInCompanyId );

                var content = new StringContent(JsonConvert.SerializeObject(original));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var cmpResponse = await client.GetAsync("/api/v1/email/setup/quarantine/companyquarantine");
                var cmpQuarantine = JsonConvert.DeserializeObject<GenericDataModel<QuarantineCompanyModel[]>>( await cmpResponse.Content.ReadAsStringAsync());


                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/v1/email/setup/quarantine/companyquarantine/{cmpQuarantine.Value[0].QuarantineId}");
                request.Content = content;

                // Act
                var response = await client.SendAsync(request);
                var sResponse = await response.Content.ReadAsStringAsync();

                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

                var result = JsonConvert.DeserializeObject<QuarantineCompanyModel>(await response.Content.ReadAsStringAsync());


                // Assert
                result.Should().NotBeNull();

                await UserRepository.InternalRevokePermission( DataContextFactory.CreateContext(), "EmailRestriction", LoggedInUserId, LoggedInCompanyId );
            }
        }


        [Fact]
        public async Task update_with_invalid_value_should_be_bad_request()
        {
            var original = Fixture.Create<QuarantineCompanyModel>();
            original.MaxBodyLength = 9999999;

            using( var server = CreateServer() )
            using( var client = server.CreateClient())
            {
                await UserRepository.InternalGrantPermission( DataContextFactory.CreateContext(), "EmailRestriction", 2, LoggedInUserId, LoggedInCompanyId );


                var content = new StringContent(JsonConvert.SerializeObject(original));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var cmpResponse = await client.GetAsync("/api/v1/email/setup/quarantine/companyquarantine");
                var cmpQuarantine = JsonConvert.DeserializeObject<GenericDataModel<QuarantineCompanyModel[]>>(await cmpResponse.Content.ReadAsStringAsync());


                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/v1/email/setup/quarantine/companyquarantine/{cmpQuarantine.Value[0].QuarantineId}");
                request.Content = content;

                // Act
                var response = await client.SendAsync(request);
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);

                await UserRepository.InternalRevokePermission( DataContextFactory.CreateContext(), "EmailRestriction", LoggedInUserId, LoggedInCompanyId );
            }
        }
    }
}
