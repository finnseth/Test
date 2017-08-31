using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Dualog.PortalService.Utils.AutoFixture
{
    public class StringLengthValueSetter : ISpecimenBuilder
    {
        string _propertyName;
        int _length;


        public StringLengthValueSetter( string propertyName, int length )
        {
            _propertyName = propertyName;
            _length = length;
        }

        public object Create( object request, ISpecimenContext context )
        {
            var propInfo = request as PropertyInfo;
            if( propInfo != null && _propertyName == propInfo.Name )
            {

                var c = (_length / 36) + 1;

                StringBuilder sb = new StringBuilder();

                for( int i = 0; i < c; i++ )
                    sb.Append( Guid.NewGuid().ToString() );

                return sb.ToString().Substring(0, _length );
            }

            return new NoSpecimen();
        }
    }
}
