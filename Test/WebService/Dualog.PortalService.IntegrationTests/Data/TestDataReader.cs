using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Dualog.PortalService.Data
{
    public class TestDataReader
    {
        public TestData ReadAsync( string path )
        {
            if( File.Exists( path) == false )
                throw new FileNotFoundException( path );

            var file = File.ReadAllText( path );

            var testData = JsonConvert.DeserializeObject<TestData>( file );
            return testData;
        }
    }
}
