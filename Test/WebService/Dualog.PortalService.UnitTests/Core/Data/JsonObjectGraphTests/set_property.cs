using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Core.Data.JsonObjectGraphTests
{
    public class set_property
    {
        [Fact]
        public async Task simple_property()
        {
            // Assign
            var json =
                @"{
                    id: 132,
                    isSometing: true,
                    name: 'Alberta',
                    age: 42,
                    address: null
                  }";
            var bant = new JsonObjectGraph(JObject.Parse(json));

            // Act
            var entity = new Entity();
            await bant.ApplyToAsync(entity, new Newtonsoft.Json.Serialization.DefaultContractResolver());

            // Assert
            entity.Name.Should().Be("Alberta");
            entity.Age.Should().Be(42);
            entity.Address.Should().BeNull();
            entity.IsSometing.Should().BeTrue();
        }

        [Fact]
        public async Task object_property()
        {
            // Assign
            var json =
                @"{
                    nested: {
                      id: 132,
                      isSometing: true,
                      name: 'Alberta',
                      age: 42,
                      address: null
                    }
                  }";
            var bant = new JsonObjectGraph(JObject.Parse(json));


            // Act
            var entity = new Entity();
            await bant.ApplyToAsync(entity, new Newtonsoft.Json.Serialization.DefaultContractResolver());

            // Assert
            entity.Nested.Should().NotBeNull();
            entity.Nested.Name.Should().Be("Alberta");
            entity.Nested.Age.Should().Be(42);
            entity.Nested.Address.Should().BeNull();
            entity.Nested.IsSometing.Should().BeTrue();
        }

        [Fact]
        public async Task update_property_with_wrong_name()
        {
            var json =
                @"{
                      surname: 'Alberta',
                  }";
            var bant = new JsonObjectGraph(JObject.Parse(json));
            bant.AddPropertyMap("surname", "name");

            // Act
            var entity = new Entity();
            await bant.ApplyToAsync(entity, new Newtonsoft.Json.Serialization.DefaultContractResolver());

            entity.Name.Should().Be("Alberta");
        }

        [Fact]
        public async Task update_property_with_property_change_handler()
        {
            var json =
                @"{
                      name: 'Alberta',
                  }";
            var bant = new JsonObjectGraph(JObject.Parse(json));
            bant.AddPropertyChangeHandler("/name", (path, value) =>
            {
                return Task.FromResult<object>( "John" );
            });

            // Act
            var entity = new Entity();
            await bant.ApplyToAsync(entity, new Newtonsoft.Json.Serialization.DefaultContractResolver());

            entity.Name.Should().Be("John");
        }

        [Fact]
        public async Task update_property_with_property_change_handler_in_nested_object()
        {
            var json =
                @"{
                    Nested: {
                      Name: 'Alberta',
                    }
                  }";
            var bant = new JsonObjectGraph(JObject.Parse(json));
            bant.AddPropertyChangeHandler("/nested/name", (path, value) => {

                return Task.FromResult<object>("John");
            });

            // Act
            var entity = new Entity();
            await bant.ApplyToAsync(entity, new Newtonsoft.Json.Serialization.DefaultContractResolver());

            entity.Nested.Name.Should().Be("John");
        }

        [Fact]
        public async Task property_should_be_ignored()
        {
            // Assign
            var json =
                @"{
                    id: 132,
                    isSometing: true,
                    name: 'Alberta',
                    age: 42,
                    address: null
                  }";
            var jog = new JsonObjectGraph(JObject.Parse(json));
            jog.IgnoreProperty("/age");

            // Act
            var entity = new Entity();
            await jog.ApplyToAsync(entity, new Newtonsoft.Json.Serialization.DefaultContractResolver());

            // Assert
            entity.Name.Should().Be("Alberta");
            entity.Age.Should().Be(0);
            entity.Address.Should().BeNull();
            entity.IsSometing.Should().BeTrue();
        }

    }
}
