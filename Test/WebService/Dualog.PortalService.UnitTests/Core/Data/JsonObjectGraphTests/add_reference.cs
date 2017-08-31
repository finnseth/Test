using System;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using FluentAssertions;
using Xunit;
using Newtonsoft.Json.Serialization;
using Moq;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Core.Data.JsonObjectGraphTests
{
    public class add_reference
    {


        [Fact]
        public async Task added_objects_should_be_added_to_data_context()
        {
            var dcMock = new Mock<IDataContext>();
            var dsMock = new Mock<IDataSet<Entity>>();

            dcMock.Setup(m => m.GetSet<Entity>()).Returns(() => {
                return dsMock.Object;
            } );


            var json =
                @"{
                    collection: {
                      add: [ {id: 100}, {id: 200}, {id: 300}]
                    }
                  }";

            var entity = new Entity();
            var jog = new JsonObjectGraph(JObject.Parse(json), dcMock.Object);

            await jog.ApplyToAsync(entity, new DefaultContractResolver());

            dsMock.Verify(m => m.Add(It.IsAny<Entity>()), Times.Exactly(3));
        }

    }
}
