using System;
using Microsoft.AspNetCore.Authentication;

namespace Dualog.PortalService.Authentication
{
    public static class TestAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder authenticationBuilder, string authenticationScheme, string displayName, Action<TestAuthenticationOptions> configureOptions)
        {
            return authenticationBuilder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
