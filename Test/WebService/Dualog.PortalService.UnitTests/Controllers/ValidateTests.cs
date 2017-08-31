using System;
using System.Linq;
using Dualog.PortalService.Core.Validation;
using Dualog.PortalService.Utils.AutoFixture;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.DataAnnotations;

namespace Dualog.PortalService.Controllers
{
    public class ValidateTests
    {
        public static void VerifyAllProperties<T>() where T : IValidatable
        {
            var fixture = new Fixture();
            var widget = fixture.Create<T>();

            widget.Validate(out string result).Should().BeTrue("because it should validate");
        }

        public static void VerifyInvalidStringLengthProperty<T>(string propertyName, int length) where T : IValidatable
        {
            var fixture = new Fixture();

            var customization = fixture.Customizations.FirstOrDefault(p => p.GetType() == typeof(StringLengthAttributeRelay));
            if (customization != null)
                fixture.Customizations.Remove(customization);

            fixture.Customizations.Add(new StringLengthValueSetter(propertyName, length));
            var widget = fixture.Create<T>();

            widget.Validate(out string result).Should().BeFalse($"because {propertyName} is invalid");
        }

        public static void VerifyRequiredProperty<T>(string propertyName) where T : IValidatable
        {
            var fixture = new Fixture();

            var customization = fixture.Customizations.FirstOrDefault(p => p.GetType() == typeof(StringLengthAttributeRelay));
            if (customization != null)
                fixture.Customizations.Remove(customization);

            fixture.Customizations.Add(new PropertyNameOmitter(propertyName));

            var widget = fixture.Create<T>();

            widget.Validate(out string result).Should().BeFalse($"because {propertyName} is invalid");
        }


        public static void VerifyWhitespaceProperty<T>(string propertyName) where T : IValidatable
        {
            var fixture = new Fixture();

            var customization = fixture.Customizations.FirstOrDefault(p => p.GetType() == typeof(StringLengthAttributeRelay));
            if (customization != null)
                fixture.Customizations.Remove(customization);

            fixture.Customizations.Add(new WhitespacePropertySetter(propertyName));

            var widget = fixture.Create<T>();

            widget.Validate(out string result).Should().BeFalse($"because {propertyName} is invalid");
        }

        public static void VerifyRangeMinProperty<T>(string propertyName) where T : IValidatable
        {
            var fixture = new Fixture();

            var customization = fixture.Customizations.FirstOrDefault(p => p.GetType() == typeof(RangeAttributeRelay));
            if (customization != null)
                fixture.Customizations.Remove(customization);

            fixture.Customizations.Add(new RangePropertySetter(propertyName, RangeTest.BelowMin));

            var widget = fixture.Create<T>();

            widget.Validate(out string result).Should().BeFalse($"because {propertyName} is above max range.");
        }



        public static void VerifyRangeMaxProperty<T>(string propertyName) where T : IValidatable
        {
            var fixture = new Fixture();

            var customization = fixture.Customizations.FirstOrDefault(p => p.GetType() == typeof(RangeAttributeRelay));
            if (customization != null)
                fixture.Customizations.Remove(customization);

            fixture.Customizations.Add(new RangePropertySetter(propertyName, RangeTest.AboveMax));

            var widget = fixture.Create<T>();

            widget.Validate(out string result).Should().BeFalse($"because {propertyName} is below min range.");
        }
    }
}
