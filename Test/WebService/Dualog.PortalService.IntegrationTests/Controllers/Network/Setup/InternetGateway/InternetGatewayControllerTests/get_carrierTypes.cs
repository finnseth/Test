using Dualog.PortalService.Controllers.Network.Setup.InternetGateway.Model;
using Dualog.PortalService.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dualog.PortalService.Controllers.Setup.InternetGateway.InternetGatewayControllerTests
{
    public class get_carrierTypes : ControllerTests
    {
        [Fact]
        public async Task should_be_ok()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var stored = await client.GetAsync<GenericDataModel<IEnumerable<CarrierTypeModel>>>( $"api/v1/network/setup/internetgateway/gatewaytype", HttpStatusCode.OK);
                stored.Value.Should().NotBeEmpty();
            }

        }
    }
}
