using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Dualog.PortalService.Core.Data.JsonObjectGraphTests
{
    public class add_to_collection
    {
        [Fact]
        public async Task add_objectAsync()
        {
            // Assign
            var json =
                @"{
                    collection: {
                      add: [
                        { name: 'Alberta' },
                        { name: 'John' },
                        { name: 'Johnny' }
                      ]
                    }
                  }";
            var jog = new JsonObjectGraph(JObject.Parse(json));


            // Act
            var entity = new Entity();
            await jog.ApplyToAsync(entity, new Newtonsoft.Json.Serialization.DefaultContractResolver());

            entity.Collection.Count.Should().Be(3);
            entity.Collection[0].Name.Should().Be("Alberta");
            entity.Collection[1].Name.Should().Be("John");
            entity.Collection[2].Name.Should().Be("Johnny");
        }

        [Fact]
        public async Task add_object_should_fire_event()
        {
            // Assign
            var json =
                @"{
                    collection: {
                      add: [
                        { name: 'Alberta' },
                      ]
                    }
                  }";
            var jog = new JsonObjectGraph(JObject.Parse(json));


            jog.MonitorEvents();

            // Act
            var entity = new Entity();
            await jog.ApplyToAsync( entity, new Newtonsoft.Json.Serialization.DefaultContractResolver() );

            jog.ShouldRaise( "CollectionChanging" )
                .WithSender( jog )
                .WithArgs<CollectionChangingEventArgs>( args => args.Path == "/collection" )
                .WithArgs<CollectionChangingEventArgs>( args => args.Action == CollectionAction.Add )
                .WithArgs<CollectionChangingEventArgs>( args => ((Entity) args.Value).Name == "Alberta" );
        }

        [Fact]
        public async Task add_object_and_change()
        {
            // Assign
            var json =
                @"{
                    collection: {
                      add: [
                        { name: 'Alberta' },
                      ]
                    }
                  }";
            var jog = new JsonObjectGraph(JObject.Parse(json));


            // Act
            jog.CollectionChanging += ( s, e ) =>
            {
                Entity n = e.Value as Entity;
                n.Name = "John";
            };

            var entity = new Entity();
            await jog.ApplyToAsync( entity, new Newtonsoft.Json.Serialization.DefaultContractResolver() );


            // Assert
            entity.Collection.First().Name.Should().Be( "John" );
        }



        [Fact]
        public void add_to_untyped_list_should_throw_exception()
        {
            var json =
                @"{
                    untypedList: {
                      add: [
                        { name: 'Alberta' },
                        { name: 'John' },
                        { name: 'Johnny' }
                      ]
                    }
                  }";

            var entity = new Entity();
            var jog = new JsonObjectGraph(JObject.Parse(json));

            Func<Task> action = async () => await jog.ApplyToAsync(entity, new Newtonsoft.Json.Serialization.DefaultContractResolver());
            action.ShouldThrow<JsonObjectGraphException>();
        }

        [Fact]
        public void add_property_without_array()
        {
            var json =
                @"{
                    untypedList: {
                      add: {}                    }
                  }";

            var entity = new Entity();
            var jog = new JsonObjectGraph(JObject.Parse(json));

            Func<Task> action = async () => await jog.ApplyToAsync(entity, new Newtonsoft.Json.Serialization.DefaultContractResolver());
            action.ShouldThrow<JsonObjectGraphException>().WithMessage("Expected a Json array.");
        }

        [Fact]
        public async Task add_entity_should_get_sequence_number()
        {
            // Assign
            var dcMock = new Mock<IDataContext>();       
            var odcMock = dcMock.As<ICanCreateSequenceNumbers>();

            odcMock.Setup(m => m.GetSequenceNumberAsync(It.IsAny<Type>())).Returns(() => Task.FromResult(1));

            var json =
                @"{
                    collection: {
                      add: [
                        { name: 'Alberta' },
                      ]
                    }
                  }";
            var jog = new JsonObjectGraph(JObject.Parse(json), dcMock.Object);


            // Act
            var entity = new Entity();
            await jog.ApplyToAsync(entity, new Newtonsoft.Json.Serialization.DefaultContractResolver());


            // Assert
            odcMock.Verify(m => m.GetSequenceNumberAsync( It.IsAny<Type>()), Times.Once());

            entity.Collection.First().Id.Should().Be(1);
        }

        [Fact]
        public async Task add_entity_to_null_collection_where_json_property_is_null()
        {
            // Assign
            var dcMock = new Mock<IDataContext>();
            var odcMock = dcMock.As<ICanCreateSequenceNumbers>();

            odcMock.Setup(m => m.GetSequenceNumberAsync(It.IsAny<Type>())).Returns(() => Task.FromResult(1));

            var json =
                @"{
                    collection: null
                  }";
            var jog = new JsonObjectGraph(JObject.Parse(json), dcMock.Object);


            // Act
            var entity = new Entity();
            await jog.ApplyToAsync(entity, new Newtonsoft.Json.Serialization.DefaultContractResolver());


            entity.Collection.Should().BeNull();
        }

        [Fact]
        public async Task add_entity_to_empty_collection_where_json_property_is_null()
        {
            // Assign
            var dcMock = new Mock<IDataContext>();
            var odcMock = dcMock.As<ICanCreateSequenceNumbers>();

            odcMock.Setup(m => m.GetSequenceNumberAsync(It.IsAny<Type>())).Returns(() => Task.FromResult(1));

            var json =
                @"{
                    collection: null
                  }";
            var jog = new JsonObjectGraph(JObject.Parse(json), dcMock.Object);


            // Act
            var entity = new Entity();
            entity.Collection = new List<Entity>();

            await jog.ApplyToAsync(entity, new Newtonsoft.Json.Serialization.DefaultContractResolver());


            entity.Collection.Should().BeEmpty();
        }
    }
}
