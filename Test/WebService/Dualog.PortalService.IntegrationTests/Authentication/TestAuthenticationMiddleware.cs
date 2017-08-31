using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dualog.PortalService.Authentication
{

    public class TestAuthenticationMiddleware : AuthenticationMiddleware<TestAuthenticationOptions>
    {
        private readonly RequestDelegate _next;

        public TestAuthenticationMiddleware(
            RequestDelegate next,
            IOptions<TestAuthenticationOptions> options,
            ILoggerFactory loggerFactory ) : base(next, options, loggerFactory, UrlEncoder.Default)
        {
            _next = next;
        }

        protected override AuthenticationHandler<TestAuthenticationOptions> CreateHandler()
        {
            return new TestAuthenticationHandler(  );
        }
    }
}
