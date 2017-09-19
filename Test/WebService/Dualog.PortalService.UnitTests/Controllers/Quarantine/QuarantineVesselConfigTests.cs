using System;
using System.Linq;
using Dualog.PortalService.Controllers;
using Dualog.PortalService.Controllers.Email.Setup.Quarantine.Model;
using Xunit;

namespace Dualog.PortalService.UnitTests.Controllers.Quarantine
{
    public class QuarantineVesselModelTests
    {
        [Fact]
        public void Verify_allPropsSet_Ok()
        {
            // ValidateTests.VerifyAllProperties<QuarantineVesselModel>();
        }

        [Theory]
        [InlineData(nameof(QuarantineVesselModel.NotificationOnHoldAdmins), 3000)]
        [InlineData(nameof(QuarantineVesselModel.NotificationSender), 3000)]
        public void Verify_PropertyIsInvalid_ShouldFail(string propertyName, int length)
        {
            ValidateTests.VerifyInvalidStringLengthProperty<QuarantineVesselModel>(propertyName, length);
        }


        [Theory]
        [InlineData(nameof(QuarantineVesselModel.NotificationOnHoldAdmins))]
        [InlineData(nameof(QuarantineVesselModel.NotificationSender))]
        public void Verify_PropertyIsWithespace_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyWhitespaceProperty<QuarantineVesselModel>(propertyName);
        }


        [Theory]
        [InlineData(nameof(QuarantineVesselModel.MaxBodyLength))]
        [InlineData(nameof(QuarantineVesselModel.OnHoldDuration))]
        public void Verify_PropertyMaxRange_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyRangeMaxProperty<QuarantineVesselModel>(propertyName);
        }


        [Theory]
        [InlineData(nameof(QuarantineVesselModel.MaxBodyLength))]
        [InlineData(nameof(QuarantineVesselModel.OnHoldDuration))]
        public void Verify_PropertyMinRange_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyRangeMinProperty<QuarantineVesselModel>(propertyName);
        }
    }
}

