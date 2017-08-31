using Dualog.PortalService.Controllers.Methods.Model;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dualog.PortalService.Controllers.Methods.MethodsControllerTests
{
    public class get_carrierTypes : ControllerTests
    {
        [Fact]
        public async Task should_be_ok()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var stored = await client.GetAsync<IEnumerable<CarrierTypeDetails>>( $"api/v1/core/carrierType", HttpStatusCode.OK);
                stored.Should().NotBeEmpty();
            }

        }
    }
}
