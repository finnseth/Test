using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dualog.PortalService.Controllers.Vessels.Model;
using FluentAssertions;
using NCrunch.Framework;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using Xunit;

namespace Dualog.PortalService.Controllers.Vessels
{
    public class VesselControllerTests : ControllerTests
    {
        Fixture _fixture = new Fixture();

        [Fact]
        public async Task AddVessel_ShouldBeOk()    
        {
            var vessel = _fixture.Create<VesselDetails>();

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient())
            {
                // Act
                var content = new StringContent(JsonConvert.SerializeObject(vessel));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync("/api/v1/vessels", content);

                // Assert
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Created);

                var s = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<VesselDetails>(s);
                vessel.Id = result.Id;

                result.ShouldBeEquivalentTo(vessel, "because the vessel was just added");
            }

        }


        protected async override void OnDispose()
        {
            var vRepo = new VesselRepository(DataContextFactory);
            foreach (var v in await vRepo.GetVessels(2597, null))
            {
                await vRepo.DeleteVesselAsync(v.Id);
            }
        }
    }
}
