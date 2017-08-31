using System;
using System.Linq;
using Dualog.PortalService.Controllers;
using Dualog.PortalService.Controllers.Quarantine.Model;
using Xunit;

namespace Dualog.PortalService.UnitTests.Controllers.Quarantine
{
    public class QuarantineVesselConfigTests
    {
        [Fact]
        public void Verify_allPropsSet_Ok()
        {
            ValidateTests.VerifyAllProperties<QuarantineVesselConfig>();
        }

        [Theory]
        [InlineData(nameof(QuarantineVesselConfig.NotificationOnHoldAdmins), 3000)]
        [InlineData(nameof(QuarantineVesselConfig.NotificationSender), 3000)]
        public void Verify_PropertyIsInvalid_ShouldFail(string propertyName, int length)
        {
            ValidateTests.VerifyInvalidStringLengthProperty<QuarantineVesselConfig>(propertyName, length);
        }


        [Theory]
        [InlineData(nameof(QuarantineVesselConfig.NotificationOnHoldAdmins))]
        [InlineData(nameof(QuarantineVesselConfig.NotificationSender))]
        public void Verify_PropertyIsWithespace_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyWhitespaceProperty<QuarantineVesselConfig>(propertyName);
        }


        [Theory]
        [InlineData(nameof(QuarantineVesselConfig.MaxBodyLength))]
        [InlineData(nameof(QuarantineVesselConfig.OnHoldDuration))]
        public void Verify_PropertyMaxRange_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyRangeMaxProperty<QuarantineVesselConfig>(propertyName);
        }


        [Theory]
        [InlineData(nameof(QuarantineVesselConfig.MaxBodyLength))]
        [InlineData(nameof(QuarantineVesselConfig.OnHoldDuration))]
        public void Verify_PropertyMinRange_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyRangeMinProperty<QuarantineVesselConfig>(propertyName);
        }
    }
}

