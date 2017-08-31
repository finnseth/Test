using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;


namespace Dualog.PortalService.Authentication
{
    public class TestAuthenticationOptions : AuthenticationOptions
    {
        public long CompanyId { get; set; }
        public long UserId { get; set; }

        public virtual ClaimsIdentity CreateIdentity()
        {
            return new ClaimsIdentity( new Claim[]
            {
                new Claim(  JwtRegisteredClaimNames.Sub, UserId.ToString() ),
                new Claim( "xdcid", CompanyId.ToString() ),
            }, "automatic" );
        }

        public TestAuthenticationOptions()
        {
            AuthenticationScheme = "Automatic";
            AutomaticAuthenticate = true;
        }
    }
}
