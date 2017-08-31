using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Controllers
{
    public static class HttpClientHelpers
    {
        public static async Task<T> PatchAsync<T>( this HttpClient client, string url, JObject patch, HttpStatusCode expectedStatus = HttpStatusCode.OK )
        {
            var content = new StringContent( patch.ToString() );
            content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

            var request = new HttpRequestMessage( new HttpMethod( "PATCH" ), url );
            request.Content = content;

            var response = await client.SendAsync( request );
            response.StatusCode.Should().Be( expectedStatus );

            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>( result );
        }

        public static async Task<T> PatchAsync<T>( this HttpClient client, string url, string patch, HttpStatusCode expectedStatus = HttpStatusCode.OK )
        {
            var content = new StringContent( patch );
            content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

            var request = new HttpRequestMessage( new HttpMethod( "PATCH" ), url );
            request.Content = content;

            var response = await client.SendAsync( request );
            response.StatusCode.Should().Be( expectedStatus );

            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>( result );
        }


        public static async Task<T> GetAsync<T>( this HttpClient client, string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK )
        {
            var response = await client.GetAsync( url );
            response.StatusCode.Should().Be( expectedStatusCode );

            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>( content );
        }


        public static async Task<T> AddAsync<T>( this HttpClient client, string url, T obj, HttpStatusCode expectedStatusCode = HttpStatusCode.Created )
        {
            string json = JsonConvert.SerializeObject( obj );
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

            var response = await client.PostAsync(url, content);

            // Assert
            response.StatusCode.ShouldBeEquivalentTo( expectedStatusCode );

            var s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>( s );
        }


    }
}
