using System;
using System.Linq;
using Xunit;

namespace Dualog.PortalService.Controllers.Dashboard
{
    public class WidgetTest
    {
        [Fact]
        public void Verify_allPropsSet_Ok()
        {
            ValidateTests.VerifyAllProperties<Widget>();
        }

        [Theory]
        [InlineData(nameof(Widget.Title), 131)]
        [InlineData(nameof(Widget.WidgetType), 131)]
        [InlineData(nameof(Widget.WidgetName), 131)]
        public void Verify_PropertyIsInvalid_ShouldFail(string propertyName, int length)
        {
            ValidateTests.VerifyInvalidStringLengthProperty<Widget>(propertyName, length);
        }


        [Theory]
        [InlineData(nameof(Widget.Title))]
        [InlineData(nameof(Widget.WidgetType))]
        [InlineData(nameof(Widget.WidgetName))]
        public void Verify_PropertyNotSet_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyRequiredProperty<Widget>(propertyName);
        }


        [Theory]
        [InlineData(nameof(Widget.Title))]
        [InlineData(nameof(Widget.WidgetType))]
        [InlineData(nameof(Widget.WidgetName))]
        public void Verify_PropertyIsWithespace_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyWhitespaceProperty<Widget>(propertyName);
        }


        [Theory]
        [InlineData( nameof(Widget.Height )  )]
        [InlineData( nameof(Widget.Width ) )]
        [InlineData(nameof(Widget.HorizontalRank))]
        [InlineData(nameof(Widget.VerticalRank))]
        public void Verify_PropertyMaxRange_ShouldFail(string propertyName  )
        {
            ValidateTests.VerifyRangeMaxProperty<Widget>(propertyName );
        }


        [Theory]
        [InlineData(nameof(Widget.Height))]
        [InlineData(nameof(Widget.Width))]
        [InlineData(nameof(Widget.HorizontalRank))]
        [InlineData(nameof(Widget.VerticalRank))]
        public void Verify_PropertyMinRange_ShouldFail(string propertyName)
        {
            ValidateTests.VerifyRangeMinProperty<Widget>(propertyName);
        }

    }
}
