using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Xunit;
using Dualog.PortalService.Controllers.Network.Setup.Services.Model;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.Setup.Services.ServicesControllerTests
{
    public class get_service : ControllerTests
    {
        [Fact]
        public async Task single_existing_should_be_ok()
        {
            var service = Fixture.Create<ServiceDetailModel>();

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var added = await client.AddAsync( $"{ApiUrl.CompanyServiceApi}", service, HttpStatusCode.Created );
                var stored = await client.GetAsync<ServiceDetailModel>( $"{ApiUrl.ServiceApi}/{added.Id}", HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task single_nonexistent_should_be_not_found()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var stored = await client.GetAsync<ServiceDetailModel>($"{ApiUrl.ServiceApi}/0", HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public async Task set_for_company_many_should_be_ok()
        {
            var services = new []{
                Fixture.Create<ServiceDetailModel>(),
                Fixture.Create<ServiceDetailModel>(),
                Fixture.Create<ServiceDetailModel>()
            };

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                foreach( var ServiceDetailModel in services )
                {
                    var added = await client.AddAsync( $"{ApiUrl.CompanyServiceApi}", ServiceDetailModel, HttpStatusCode.Created );
                    ServiceDetailModel.Id = added.Id;
                }

                var result = await client.GetAsync<GenericDataModel<IEnumerable<ServiceDetailModel>>>( ApiUrl.ServiceApi, HttpStatusCode.OK );
            }

        }
    }
}
