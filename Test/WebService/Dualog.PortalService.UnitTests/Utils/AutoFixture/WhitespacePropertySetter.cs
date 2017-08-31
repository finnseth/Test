using Ploeh.AutoFixture.Kernel;
using System;
using System.Linq;
using System.Reflection;

namespace Dualog.PortalService.Utils.AutoFixture
{
    public class WhitespacePropertySetter : ISpecimenBuilder
    {
        string _propertyName;

        public WhitespacePropertySetter(string propertyName)
        {
            _propertyName = propertyName;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var propInfo = request as PropertyInfo;
            if (propInfo != null && _propertyName == propInfo.Name)
            {
                return new string(' ', new Random().Next(1, 6));
            }

            return new NoSpecimen();
        }
    }
}
