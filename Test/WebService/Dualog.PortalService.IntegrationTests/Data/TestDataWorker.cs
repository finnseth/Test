using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Windows;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Dualog.PortalService.Data
{
    public class TestDataWorker
    {
        TestServer _server;
        Dictionary<string, object> _objectLookup = new Dictionary<string, object>();
        Fixture fixture = new Fixture();


        public TestDataWorker( TestServer server )
        {
            _server = server;
        }


        public async Task<ObjectLookup> Execute( TestData testData )
        {
            foreach( var item in testData.Entities )
            {
                var type = Type.GetType( item.Type, assemblyResolver, tr );
                if( type == null )
                    continue;

                object o;
                if (item.Properties != null && item.Properties.Count > 0)
                    o = CreateFromProperties(item, type);
                else
                    o = new SpecimenContext(fixture).Resolve(type);


                // Add the object and get the result back
                var response = await PostAsync( ParseValue( item.Host ), o );
                var responseObject = JsonConvert.DeserializeObject( response, type );

                if( string.IsNullOrEmpty( item.Id ) == false )
                {
                    if( _objectLookup.ContainsKey( item.Id ) == false )
                        _objectLookup[ item.Id ] = responseObject;
                }
            }

            return new ObjectLookup( _objectLookup );
        }

        private object CreateFromProperties(Entity item, Type type)
        {
            object o = Activator.CreateInstance(type);
            foreach (var prop in item.Properties)
            {
                var pi = type.GetProperty(prop.Key);
                if (pi == null)
                    continue;

                Type pt = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;

                object value = prop.Value is string ? ParseValue((string)prop.Value) : prop.Value;
                value = Convert.ChangeType(value, pt);

                pi.SetValue(o, value);
            }

            return o;
        }

        string ParseValue( string value )
        {
            var tokenizer = new ValueTokenizer();
            var tokens = tokenizer.Tokenize( value ).ToArray();
            return tokenizer.Parse( tokens, new ObjectLookup( _objectLookup ) );
        }


        Assembly assemblyResolver( AssemblyName an )
        {
            return null;
        }

        Type tr( Assembly a, string s, bool b )
        {
            var dbtype = typeof( Dualog.PortalService.Controllers.Dashboard.Dashboard );
            var type = dbtype.Assembly.GetType( s );

            return type;
        }


        private async Task<string> PostAsync( string requestUri, object o )
        {
            var s = JsonConvert.SerializeObject( o );
            var content = new StringContent( s );
            content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );


            using( var client = _server.CreateClient() )
            {
                var response = await client.PostAsync( requestUri, content );

                s = await response.Content.ReadAsStringAsync();
                return s;
            }
        }
    }
}
