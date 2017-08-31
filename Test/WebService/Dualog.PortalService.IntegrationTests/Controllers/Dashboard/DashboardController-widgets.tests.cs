using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using NCrunch.Framework;
using Newtonsoft.Json;
using Xunit;

namespace Dualog.PortalService.Controllers.Dashboard
{
    public partial class DashboardControllerTests
    {

        [Fact]
        public async Task GetSpecificWidget_ShouldFindWidget()
        {

            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                // Assign
                var objectLookup = await SetupData(@".\TestData\dashboardAndWidgets.json", server);
                _dashboard = objectLookup.GetObjectById<Dashboard>("db1");
                var widget = objectLookup.GetObjectById<Widget>("w1");


                // Act
                var response = await client.GetAsync( $"/api/v1/dashboards/widgets/{widget.Id}" );
                response.StatusCode.ShouldBeEquivalentTo( HttpStatusCode.OK );


                // Assert
                var result = JsonConvert.DeserializeObject<Widget>( await response.Content.ReadAsStringAsync() );
                result.Should().NotBeNull( "Because it is just inserted" );
            }
        }


        [Fact]
        public async Task GetWidgetDataIsOk()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                // Assign
                var objectLookup = await SetupData(@".\TestData\dashboardAndWidgets.json", server);
                _dashboard = objectLookup.GetObjectById<Dashboard>( "db1" );
                var widget = objectLookup.GetObjectById<Widget>("w1");

                // Act
                var response = await client.GetAsync( $"/api/v1/dashboards/widgets/{widget.Id}/data" );
                response.StatusCode.Should().Be( HttpStatusCode.OK );

                var result = await response.Content.ReadAsStringAsync();

                // Assert
            }
        }


        [Fact]
        public async Task AddNewWidget_ValidData_ShouldBeOk()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var objectLookup = await SetupData(@".\TestData\dashboard.json", server);
                _dashboard = objectLookup.GetObjectById<Dashboard>("db1");


                var widget = new Widget
                {

                    Height = 100,
                    HorizontalRank = 2,
                    Title = "TestWidget",
                    VerticalRank = 4,
                    WidgetName = "AntiVirusUpdateSummary",
                    WidgetType = "Pie",
                    Width = 100
                };


                var content = new StringContent( JsonConvert.SerializeObject( widget ) );
                content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

                // Act
                var response = await client.PostAsync( $"/api/v1/dashboards/{_dashboard.Id}/widgets", content );
                response.StatusCode.ShouldBeEquivalentTo( HttpStatusCode.Created );


                var result = JsonConvert.DeserializeObject<Widget>( await response.Content.ReadAsStringAsync() );
                widget.Id = result.Id;


                // Assert
                result.ShouldBeEquivalentTo( widget );
            }
        }


        [Fact]
        public async Task AddNewWidget_ValidationFails_ShouldBeBadRequest()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                var objectLookup = await SetupData(@".\TestData\dashboard.json", server);
                _dashboard = objectLookup.GetObjectById<Dashboard>("db1");


                var widget = new Widget
                {
                };


                var content = new StringContent( JsonConvert.SerializeObject( widget ) );
                content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

                // Act
                var response = await client.PostAsync( $"/api/v1/dashboards/{_dashboard.Id}/widgets", content );
                response.StatusCode.ShouldBeEquivalentTo( HttpStatusCode.BadRequest, "because the data is invalid" );
            }
        }


        [Fact]
        public async Task DeleteWidget_ShouldBeOk()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                // Assign
                var objectLookup = await SetupData(@".\TestData\dashboardAndWidgets.json", server);
                _dashboard = objectLookup.GetObjectById<Dashboard>("db1");
                var widget = objectLookup.GetObjectById<Widget>("w1");

                // Act
                var response = await client.DeleteAsync( $"/api/v1/dashboards/widgets/{widget.Id}" );
                response.StatusCode.ShouldBeEquivalentTo( HttpStatusCode.OK, "because it was just deleted." );


                response = await client.GetAsync( $"/api/v1/dashboards/{_dashboard.Id}/widgets" );
                response.StatusCode.ShouldBeEquivalentTo( HttpStatusCode.OK, "because the endpoint should be available." );

                var result = JsonConvert.DeserializeObject<Widget[]>( await response.Content.ReadAsStringAsync() );


                // Assert
                result.Any( w => w.Id == widget.Id ).Should().BeFalse( "Because this widget should be deleted." );
            }
        }


        [Fact]
        public async Task UpdateWidget_ShouldBeOk()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                // Assign
                var objectLookup = await SetupData(@".\TestData\dashboardAndWidgets.json", server);
                _dashboard = objectLookup.GetObjectById<Dashboard>("db1");
                var widget = objectLookup.GetObjectById<Widget>("w1");

                string json =
                    @"{
                    height: 21,
                    horizontalRank: 145,
                    title: 'The new title',
                    verticalRank: 65,
                    widgetname: 'CompanySummaryUsers',
                    WidgetType: 'Bar',
                    width: 500
                  }";

                var content = new StringContent( json );
                content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

                var request = new HttpRequestMessage( new HttpMethod( "PATCH" ), $"/api/v1/dashboards/widgets/{widget.Id}" );
                request.Content = content;



                // Act
                var response = await client.SendAsync( request );
                response.StatusCode.Should().Be( HttpStatusCode.OK );

                var result = JsonConvert.DeserializeObject<Widget>( await response.Content.ReadAsStringAsync() );

                // Assert
                result.Should().NotBeNull();

                //patchDocument.ApplyTo( widget );
                //result.ShouldBeEquivalentTo( widget );
            }
        }


        [Fact]
        public async Task UpdateWidgetWithInvalidWidgetType_ShouldBeBadRequest()
        {
            using( var server = CreateServer() )
            using( var client = server.CreateClient() )
            {
                // Assign
                var objectLookup = await SetupData(@".\TestData\dashboardAndWidgets.json", server);
                _dashboard = objectLookup.GetObjectById<Dashboard>("db1");
                var widget = objectLookup.GetObjectById<Widget>("w1");

                var json =
                    @"{
                    widgetType: 'InvalidWidgetType'
                  }";

                var content = new StringContent( json );
                content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

                var request = new HttpRequestMessage( new HttpMethod( "PATCH" ), $"/api/v1/dashboards/widgets/{widget.Id}" );
                request.Content = content;



                // Act
                var response = await client.SendAsync( request );


                // Assert
                response.StatusCode.Should().Be( HttpStatusCode.BadRequest, "Because the Widget Type is invalid" );
            }
        }
    }
}
