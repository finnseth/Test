using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Dualog.PortalService.Utils.AutoFixture
{
    public class PropertyNameOmitter : ISpecimenBuilder
    {
        string _propertyName;


        public PropertyNameOmitter( string propertyName )
        {
            _propertyName = propertyName;
        }

        public object Create( object request, ISpecimenContext context )
        {
            var cs = request as ConstrainedStringRequest;

            var propInfo = request as PropertyInfo;
            if( propInfo != null && _propertyName == propInfo.Name )
                return new OmitSpecimen();

            return new NoSpecimen();
        }
    }
}
