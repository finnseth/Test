using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Dualog.PortalService.Authentication
{
    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public TestAuthenticationOptions()
        {
        }

        public long CompanyId { get; set; }
        public long UserId { get; set; }
        public ClaimsIdentity Identity
        {
            get
            {
                Claim[] claims = new Claim[]
                {
                    new Claim(  JwtRegisteredClaimNames.Sub, UserId.ToString() ),
                    new Claim( "xdcid", CompanyId.ToString() ),
                };
                return new ClaimsIdentity(claims, "integrationTests");
            }
        }

        public override void Validate()
        {
            base.Validate();
        }
    }
}
