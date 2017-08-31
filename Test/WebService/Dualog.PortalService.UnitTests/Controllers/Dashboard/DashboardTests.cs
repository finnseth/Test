using System;
using System.Linq;
using Xunit;

namespace Dualog.PortalService.Controllers.Dashboard
{
    public class DashboardTests
    {
        [Fact]
        public void Verify_allPropsSet_Ok()
        {
            ValidateTests.VerifyAllProperties<Dashboard>();
        }


        [Theory]
        [InlineData(nameof(Dashboard.Name), 131)]
        public void Verify_PropertyIsInvalid_ShouldFail(string propertyName, int length)
        {
            ValidateTests.VerifyInvalidStringLengthProperty<Dashboard>(propertyName, length);
        }


        [Theory]
        [InlineData(nameof(Dashboard.Name))]
        public void Verify_PropertyNotSet_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyRequiredProperty<Dashboard>(propertyName);
        }

        [Theory]
        [InlineData(nameof(Dashboard.Name))]
        public void Verify_PropertyIsWithespace_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyWhitespaceProperty<Dashboard>(propertyName);
        }

    }
}
