using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.PortalService.Core.Data;
using FluentAssertions;
using Newtonsoft.Json.Serialization;
using Xunit;
using Moq;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Core.Data.JsonObjectGraphTests
{
    public class update_item
    {
        [Fact]
        public void items_must_implement_entity()
        {
            var json =
                @"{
                    objectCollection: {
                      update:   {}
                    }
                  }";

            var entity = new Entity();
            var jog = new JsonObjectGraph(JObject.Parse(json));


            Func<Task> action = () => jog.ApplyToAsync(entity, new DefaultContractResolver());
            action.ShouldThrow<JsonObjectGraphException>().WithMessage($"Items in collection to update must implement {nameof(IEntity)}");
        }

        [Fact]
        public void non_entity_collection_should_throw_exception()
        {
            var json =
                @"{
                    untypedList: {
                      update: {}                    
                    }
                  }";

            var entity = new Entity();
            var jog = new JsonObjectGraph(JObject.Parse(json));

            Func<Task> action = () => jog.ApplyToAsync(entity, new DefaultContractResolver());
            action.ShouldThrow<JsonObjectGraphException>().WithMessage("Unable to infer type from collection. Type must be a generic List<T>.");
        }

        [Fact]
        public async Task when_item_exists_in()
        {
            // Assign
            var json =
                @"{
                    collection: {
                      update: {
                        1: {
                          name: 'Ferdinand',
                          age: 72
                      }}
                    }
                  }";


            var entity = new Entity();
            entity.Collection = new List<Entity>() { new Entity { Id = 1, Name = "Elise", Age = 17 } };

            var jog = new JsonObjectGraph(JObject.Parse(json));

            // Act
            await jog.ApplyToAsync(entity, new DefaultContractResolver());

            // Assert
            Entity result = entity.Collection.FirstOrDefault();

            result.Name.Should().Be("Ferdinand");
            result.Age.Should().Be(72);
        }


        [Fact]
        public async Task when_item_does_not_exists()
        {
            // Assign
            var dcMock = new Mock<IDataContext>();
            var dsMock = new Mock<IDataSet<IEntity>>();

            dcMock.Setup(m => m.GetSet<IEntity>() ).Returns(() => {
                return dsMock.Object;
            } );

            dsMock.Setup(m => m.Attach(It.IsAny<Entity>())).Returns<Entity>(v => {
                return v;
            } ); 

            var json =
                @"{
                    collection: {
                      update: {
                        1: {
                          name: 'Ferdinand',
                          age: 72
                      }}
                    }
                  }";


            var entity = new Entity();
            var jog = new JsonObjectGraph(JObject.Parse(json), dcMock.Object );

            // Act
            await jog.ApplyToAsync(entity, new DefaultContractResolver());

            // Assert
            dsMock.Verify(m => m.Attach(It.IsAny<Entity>()), Times.Never());

        }

        [Fact]
        public async Task when_id_is_string()
        {
            // Assign
            var dcMock = new Mock<IDataContext>();
            var dsMock = new Mock<IDataSet<IEntity>>();

            dcMock.Setup(m => m.GetSet<IEntity>()).Returns(() => {
                return dsMock.Object;
            });

            dsMock.Setup(m => m.Attach(It.IsAny<Entity>())).Returns<Entity>(v => {
                return v;
            });

            var json =
                @"{
                    collection: {
                      update: {
                        'string' : {
                          name: 'Ferdinand',
                          age: 72
                      }}
                    }
                  }";


            var entity = new Entity();
            var jog = new JsonObjectGraph(JObject.Parse(json), dcMock.Object);

            jog.LookupObjectById = (path, value) =>
            {
                return entity;
            };

            // Act
            await jog.ApplyToAsync(entity, new DefaultContractResolver() );

            // Assert
        }

    }
}
