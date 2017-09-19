using System;
using System.Linq;
using Dualog.PortalService.Controllers;
using Xunit;
using Dualog.PortalService.Controllers.Email.Setup.Quarantine.Model;

namespace Dualog.PortalService.UnitTests.Controllers.Quarantine
{
    public class QuarantineCompanyModelTests
    {

        [Fact]
        public void Verify_allPropsSet_Ok()
        {
            // ValidateTests.VerifyAllProperties<QuarantineCompanyModel>();
        }

        [Theory]
        [InlineData(nameof(QuarantineCompanyModel.NotificationOnHoldAdmins), 3000)]
        [InlineData(nameof(QuarantineCompanyModel.NotificationSender), 3000)]
        public void Verify_PropertyIsInvalid_ShouldFail(string propertyName, int length)
        {
            ValidateTests.VerifyInvalidStringLengthProperty<QuarantineCompanyModel>(propertyName, length);
        }


        [Theory]
        [InlineData(nameof(QuarantineCompanyModel.NotificationOnHoldAdmins))]
        [InlineData(nameof(QuarantineCompanyModel.NotificationSender))]
        public void Verify_PropertyIsWithespace_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyWhitespaceProperty<QuarantineCompanyModel>(propertyName);
        }


        [Theory]
        [InlineData(nameof(QuarantineCompanyModel.MaxBodyLength))]
        [InlineData(nameof(QuarantineCompanyModel.OnHoldDuration))]
        public void Verify_PropertyMaxRange_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyRangeMaxProperty<QuarantineCompanyModel>(propertyName);
        }


        [Theory]
        [InlineData(nameof(QuarantineCompanyModel.MaxBodyLength))]
        [InlineData(nameof(QuarantineCompanyModel.OnHoldDuration))]
        public void Verify_PropertyMinRange_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyRangeMinProperty<QuarantineCompanyModel>(propertyName);
        }
    }
}

