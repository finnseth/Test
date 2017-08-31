using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;


namespace Dualog.PortalService.Authentication
{
    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
    {

        public TestAuthenticationHandler( )
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var identity = Options.CreateIdentity();

            var authTicket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                Options.AuthenticationScheme);

            return Task.FromResult( AuthenticateResult.Success( authTicket ) );
        }
    }
}
