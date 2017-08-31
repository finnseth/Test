using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using Ploeh.AutoFixture;

namespace Dualog.PortalService.Controllers.Dashboard
{
    public partial class DashboardControllerTests : ControllerTests
    {
        Dashboard _dashboard;

        public DashboardControllerTests()
        {
            _dashboard = Fixture.Create<Dashboard>();
        }

        [Fact]
        public async Task AddDashboard_ShouldBeOk()
        {


            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                // Act
                var content = new StringContent( JsonConvert.SerializeObject( _dashboard ) );
                content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

                var response = await client.PostAsync( "/api/v1/dashboards", content );

                // Assert
                response.StatusCode.ShouldBeEquivalentTo( HttpStatusCode.Created );

                var s = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Dashboard>( s );
                _dashboard.Id = result.Id;

                result.ShouldBeEquivalentTo( _dashboard, "because the dashboard was just added" );
            }
        }


        [Fact]
        public async Task AddDashboard_InvalidData_ShouldBeBadRequest()
        {
            _dashboard.Name = "";

            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                // Act
                var content = new StringContent( JsonConvert.SerializeObject( _dashboard ) );
                content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

                var response = await client.PostAsync( "/api/v1/dashboards", content );

                // Assert
                response.StatusCode.ShouldBeEquivalentTo( HttpStatusCode.BadRequest );
            }
        }


        [Fact]
        public async Task GetDashboards_IsOk()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                // Assign
                var objectLookup = await SetupData(@".\TestData\dashboard.json", server);
                _dashboard = objectLookup.GetObjectById<Dashboard>( "db1" );


                // Act
                var response = await client.GetAsync( "/api/v1/dashboards" );


                // Assert
                response.StatusCode.ShouldBeEquivalentTo( HttpStatusCode.OK );

                var s = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Dashboard[]>( s );

                result.Length.ShouldBeEquivalentTo( 1 );
                result[ 0 ].ShouldBeEquivalentTo( _dashboard );
            }
        }


        [Fact]
        public async Task CreateDashboard_ShouldReturnOk()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var content = new StringContent( JsonConvert.SerializeObject( _dashboard ) );
                content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

                // Act
                var response = await client.PostAsync( "/api/v1/dashboards", content );

                // Assert
                response.StatusCode.ShouldBeEquivalentTo( HttpStatusCode.Created );

                var s = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Dashboard>( s );

                result.Name.ShouldBeEquivalentTo( _dashboard.Name );
            }
        }


        [Fact]
        public async Task DeleteDashboard_ShouldBeOk()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                // Assign

                // Act
                var response = await client.DeleteAsync( $"/api/v1/dashboards/{_dashboard.Id}" );

                // Assert
                response.StatusCode.ShouldBeEquivalentTo( HttpStatusCode.OK );
            }
        }


        [Fact]
        public async Task DeleteDashboard_NonExistent_ShouldBeOk()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                // Assign
                var objectLookup = await SetupData(@".\TestData\dashboard.json", server);
                _dashboard = objectLookup.GetObjectById<Dashboard>( "db1" );


                // Act
                var response = await client.DeleteAsync($"/api/v1/dashboards/{_dashboard.Id+10000}");


                // Assert
                response.StatusCode.ShouldBeEquivalentTo( HttpStatusCode.OK );
            }

        }



        [Fact]
        public async Task UpdateDashboard_ShouldBeOk()
        {
            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var objectLookup = await SetupData(@".\TestData\dashboard.json", server);
                _dashboard = objectLookup.GetObjectById<Dashboard>( "db1" );


                var json =
                  @"{
                    name: 'The new name'
                  }";


                var content = new StringContent( json );
                content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

                var request = new HttpRequestMessage( new HttpMethod( "PATCH" ), $"/api/v1/dashboards/{_dashboard.Id}" );
                request.Content = content;



                // Act
                var response = await client.SendAsync( request );
                response.StatusCode.ShouldBeEquivalentTo( HttpStatusCode.OK );

                var result = JsonConvert.DeserializeObject<Dashboard>( await response.Content.ReadAsStringAsync() );



                // Assert
                result.Should().NotBeNull();
                result.Name.Should().Be( "The new name" );
            }
        }


        [Fact]
        public async Task UpdateDashboard_NameIsInvalid_ShouldBeBadRequest()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                // Assign
                var objectLookup = await SetupData(@".\TestData\dashboard.json", server);
                _dashboard = objectLookup.GetObjectById<Dashboard>( "db1" );

                var json =
                  @"{
                    name: ''
                  }";


                var content = new StringContent( json );
                content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

                var request = new HttpRequestMessage( new HttpMethod( "PATCH" ), $"/api/v1/dashboards/{_dashboard.Id}" );
                request.Content = content;


                // Act
                var response = await client.SendAsync( request );
                response.StatusCode.ShouldBeEquivalentTo( HttpStatusCode.BadRequest, "because the name is invalid" );

                var result = JsonConvert.DeserializeObject<Dashboard>( await response.Content.ReadAsStringAsync() );
            }
        }


        [Fact]
        public async Task GetDashboardConfig_ShouldBeOk()
        {
            // Assign
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                // Act
                var response = await client.GetAsync("/api/v1/dashboards/config");

                // Assert
                response.StatusCode.ShouldBeEquivalentTo( HttpStatusCode.OK );

                var s = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<DashboardConfig>(s);

                result.Should().NotBeNull();
            }
        }

        protected async override void OnDispose()
        {
            // Wipe dashboard data
            var dbRepo = new DashboardRepository(DataContextFactory);
            var dashboards = await dbRepo.GetDashboards(LoggedInUserId);
            foreach( var dashboard in dashboards )
            {
                await dbRepo.DeleteDashboard( dashboard.Id, LoggedInUserId );
            }
        }
    }
}