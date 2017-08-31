using System;
using System.Collections.Generic;
using System.Linq;
using Dualog.Data.Entity;
using FluentAssertions;
using Xunit;
using Moq;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Core.Data.JsonObjectGraphTests
{
    public class remove_from_collection
    {
        [Fact]
        public async Task remove_items()
        {
            // Assign
            var entity = new Entity();
            entity.Collection = new List<Entity>() {
                new Entity{ Id = 1,  Name = "Alberta" },
                new Entity{ Id = 2,  Name = "John" },
                new Entity{ Id = 3,  Name = "Johnny" }
            };

            var json =
                @"{
                    collection: {
                      delete: [1,3]
                    }
                  }";


            var jog = new JsonObjectGraph(JObject.Parse(json));

            // Act
            await jog.ApplyToAsync( entity, new DefaultContractResolver() );


            // Assert
            entity.Collection.Count.Should().Be( 1 );
        }


        [Fact]
        public void items_must_implement_entity()
        {
            var json =
                @"{
                    objectCollection: {
                      delete: []
                    }
                  }";

            var entity = new Entity();
            var jog = new JsonObjectGraph(JObject.Parse(json));

            Func<Task> action = () => jog.ApplyToAsync( entity, new DefaultContractResolver() );
            action.ShouldThrow<JsonObjectGraphException>().WithMessage( $"Items in collection to delete from must implement {nameof( IEntity )}" );
        }

        [Fact]
        public void non_entity_collection_should_throw_exception()
        {
            var json =
                @"{
                    untypedList: {
                      delete: {}                    
                    }
                  }";

            var entity = new Entity();
            var jog = new JsonObjectGraph(JObject.Parse(json));

            Func<Task> action = () => jog.ApplyToAsync( entity, new DefaultContractResolver() );
            action.ShouldThrow<JsonObjectGraphException>().WithMessage( "Expected a Json array." );
        }

        [Fact]
        public async Task string_id_should_use_external_lookup()
        {
            var json =
                @"{
                    collection: {
                      delete: [ 'Yo', 'dude' ]
                    }
                  }";



            var entity = new Entity();
            var jog = new JsonObjectGraph(JObject.Parse(json));

            bool called = false;

            jog.LookupObjectById = (p, o) =>
            {
                called = true;
                return entity;
            };

            await jog.ApplyToAsync( entity, new DefaultContractResolver() );
            called.Should().BeTrue();
        }

        

        [Fact]
        public async Task should_remove_from_data_context()
        {
            // Assign
            var dcMock = new Mock<IDataContext>();
            var dsMock = new Mock<IDataSet<Entity>>();

            dcMock.Setup( m => m.GetSet<Entity>() ).Returns( () => dsMock.Object as IDataSet<Entity> );

            var json =
                @"{
                    collection: {
                      delete: [1,3]
                    }
                  }";


            var jog = new JsonObjectGraph(JObject.Parse(json), dcMock.Object );


            // Act
            var entity = new Entity();
            await jog.ApplyToAsync( entity, new DefaultContractResolver() );


            // Assert
            //dcMock.Verify( m => m.GetSet<Entity>(), Times.Exactly( 2 ) );
            //dsMock.Verify( m => m.Remove( It.IsAny<Entity>() ), Times.Exactly( 2 ) );
        }
    }
}
