using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using Xunit;
using Dualog.PortalService.Controllers.Organization.Shipping.Ship.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.Ship;

namespace Dualog.PortalService.Controllers.Vessels
{
    public class VesselControllerTests : ControllerTests
    {
        Fixture _fixture = new Fixture();

        [Fact]
        public async Task AddVessel_ShouldBeOk()    
        {
            var vessel = _fixture.Create<ShipModel>();

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient())
            {
                // Act
                var content = new StringContent(JsonConvert.SerializeObject(vessel));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync("/api/v1/organization/shipping/ship", content);

                // Assert
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Created);

                var s = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ShipModel>(s);
                vessel.Id = result.Id;

                result.ShouldBeEquivalentTo(vessel, "because the vessel was just added");
            }

        }


        protected async override void OnDispose()
        {
            var vRepo = new ShipRepository(DataContextFactory);
            foreach (var v in (await vRepo.GetShip(2597)).Value)
            {
                await vRepo.DeleteVesselAsync(v.Id);
            }
        }
    }
}
