using Dualog.PortalService.Controllers;
using Dualog.PortalService.Controllers.Quarantine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dualog.PortalService.UnitTests.Controllers.Quarantine
{
    public class QuarantineCompanyConfigTests
    {
        [Fact]
        public void Verify_allPropsSet_Ok()
        {
            ValidateTests.VerifyAllProperties<QuarantineCompanyConfig>();
        }

        [Theory]
        [InlineData(nameof(QuarantineCompanyConfig.NotificationOnHoldAdmins), 3000)]
        [InlineData(nameof(QuarantineCompanyConfig.NotificationSender), 3000)]
        public void Verify_PropertyIsInvalid_ShouldFail(string propertyName, int length)
        {
            ValidateTests.VerifyInvalidStringLengthProperty<QuarantineCompanyConfig>(propertyName, length);
        }


        [Theory]
        [InlineData(nameof(QuarantineCompanyConfig.NotificationOnHoldAdmins))]
        [InlineData(nameof(QuarantineCompanyConfig.NotificationSender))]
        public void Verify_PropertyIsWithespace_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyWhitespaceProperty<QuarantineCompanyConfig>(propertyName);
        }


        [Theory]
        [InlineData(nameof(QuarantineCompanyConfig.MaxBodyLength))]
        [InlineData(nameof(QuarantineCompanyConfig.OnHoldDuration))]
        public void Verify_PropertyMaxRange_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyRangeMaxProperty<QuarantineCompanyConfig>(propertyName);
        }


        [Theory]
        [InlineData(nameof(QuarantineCompanyConfig.MaxBodyLength))]
        [InlineData(nameof(QuarantineCompanyConfig.OnHoldDuration))]
        public void Verify_PropertyMinRange_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyRangeMinProperty<QuarantineCompanyConfig>(propertyName);
        }
    }
}

