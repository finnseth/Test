using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using System.ComponentModel.DataAnnotations;
using Ploeh.AutoFixture;

namespace Dualog.PortalService.Utils.AutoFixture
{
    public enum RangeTest
    {
        BelowMin,
        AboveMax
    }

    public class RangePropertySetter : ISpecimenBuilder
    {
        string _propertyName;
        RangeTest _rangeTest;

        public RangePropertySetter(string propertyName, RangeTest rangeTest )
        {
            _propertyName = propertyName;
            _rangeTest = rangeTest;
        }


        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;
            if( pi != null && pi.Name == _propertyName )
            {
                var ra = pi.GetCustomAttribute<RangeAttribute>();
                return _rangeTest == RangeTest.AboveMax ? (int)ra.Maximum + 1000 : (int)ra.Minimum - 1000;
            }

            return new NoSpecimen();
        }
    }
}
