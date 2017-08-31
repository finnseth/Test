using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dualog.PortalService.Controllers.Quarantine.Model;
using FluentAssertions;
using NCrunch.Framework;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using Xunit;
using Dualog.PortalService.Controllers.Users;

namespace Dualog.PortalService.Controllers.Quarantine.QuarantineControllerTests
{
    public class update_quarantine_company_configuration : ControllerTests
    {

        [Fact]
        public async Task update_all_fields_with_valid_values()
        {
            // Assign
            var original = Fixture.Create<QuarantineCompanyConfig>();

            using( var server = CreateServer() )
            using( var client = server.CreateClient())
            {
                await UserRepository.InternalGrantPermission( DataContextFactory.CreateContext(), "EmailRestriction", 2, LoggedInUserId, LoggedInCompanyId );


                var content = new StringContent(JsonConvert.SerializeObject(original));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/v1/email/settings/quarantine");
                request.Content = content;

                // Act
                var response = await client.SendAsync(request);
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

                var result = JsonConvert.DeserializeObject<QuarantineCompanyConfig>(await response.Content.ReadAsStringAsync());


                // Assert
                result.Should().NotBeNull();

                await UserRepository.InternalRevokePermission( DataContextFactory.CreateContext(), "EmailRestriction", LoggedInUserId, LoggedInCompanyId );
            }
        }


        [Fact]
        public async Task update_with_invalid_value_should_be_bad_request()
        {
            var original = Fixture.Create<QuarantineCompanyConfig>();
            original.MaxBodyLength = 9999999;

            using( var server = CreateServer() )
            using( var client = server.CreateClient())
            {
                await UserRepository.InternalGrantPermission( DataContextFactory.CreateContext(), "EmailRestriction", 2, LoggedInUserId, LoggedInCompanyId );


                var content = new StringContent(JsonConvert.SerializeObject(original));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/v1/email/settings/quarantine");
                request.Content = content;

                // Act
                var response = await client.SendAsync(request);
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);

                await UserRepository.InternalRevokePermission( DataContextFactory.CreateContext(), "EmailRestriction", LoggedInUserId, LoggedInCompanyId );
            }
        }
    }
}
