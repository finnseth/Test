using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.Core.Translation;
using Dualog.Data.Configuration;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore;
using Dualog.Data.Provider;
using Dualog.Data.Utils.Common;
using Dualog.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using NJsonSchema;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dualog.PortalService.Controllers
{

    public class DualogController : Controller
    {
        LanguageManager _languageManager;
        IDataContextFactory _dataContextFactory;


        /// <summary>
        /// Initializes a new instance of the <see cref="DualogController"/> class.
        /// </summary>
        public DualogController( IDataContextFactory factory )
        {
            _dataContextFactory = factory;
        }


        /// <summary>
        /// Gets the repository factory.
        /// </summary>
        /// <value>
        /// The repository factory.
        /// </value>
        protected IDataContextFactory DataContextFactory => _dataContextFactory;


        /// <summary>
        /// Gets the company identifier for the company for which te current authorized user belongs to.
        /// </summary>
        /// <returns></returns>
        protected long CompanyId
        {
            get
            { 
                // Get the Company Id as a string
                var claimsIdentity =  HttpContext.User.Identities.First();
                var sCompanyId = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "xdcid")?.Value;

                if (sCompanyId == null)
                    sCompanyId = HttpContext.Request.Headers["xdcid"];


                // Convert the string to a long
                long cid;
                return long.TryParse(sCompanyId, out cid ) == false ? 0 : cid;
            }
        }

        protected long UserId
        {
            get
            {
                var claimsIdentity = HttpContext.User.Identities.First();
                var sCompanyId = claimsIdentity.Claims.FirstOrDefault( c => c.Type == "sub" ).Value;
                long userId;
                return long.TryParse( sCompanyId, out userId ) == false ? 0 : userId;
            }

        }

        protected bool IsDualogAdmin
        {
            get
            {
                var claimsIdentity = HttpContext.User.Identities.First();
                return claimsIdentity.Claims.Any(c => c.Type == "xdadmin");
            }
        }


        protected async Task<LanguageManager> GetLanguageManager()
        {
            if( _languageManager == null )
            {

                var ts = new TranslationService( DataContextFactory, new NullLogger() );
                _languageManager = await ts.CreateLanguageManager();
            }

            return _languageManager;

        }

        /// <summary>
        /// Sanitizes the and validate the period of time passed in.
        /// </summary>
        /// <param name="start">The start of the period.</param>
        /// <param name="end">The end.</param>
        /// <param name="lengthOfPeriod">The length of the period.</param>
        /// <param name="httpActionResult">The http information to return in case of failure.</param>
        protected bool SanitizeAndValidatePeriod( ref DateTime? start, ref DateTime? end, int lengthOfPeriod, out IActionResult httpActionResult )
        {
            // Sanitize the period
            SanitizePeriod( ref start, ref end );


            // Validate the period
            return ValidatePeriod( start.Value, end.Value, TimeSpan.FromDays( lengthOfPeriod ), out httpActionResult );
        }


        /// <summary>
        /// Validates the given period.
        /// </summary>
        /// <param name="start">The start of the period.</param>
        /// <param name="end">The end of the period.</param>
        /// <param name="length">The length.</param>
        /// <param name="httpActionResult">The http information to return in case of failure.</param>
        /// <returns></returns>
        protected bool ValidatePeriod( DateTime start, DateTime end, TimeSpan? length, out IActionResult httpActionResult )
        {
            // Check if end is after start
            if( start > end )
            {
                httpActionResult = BadRequest( "Start of period cannot be after the end." );
                return false;
            }


            // Check if the period is to long according to the specified length
            if( length != null && (end - start) > length )
            {
                httpActionResult = BadRequest( $"The period length cannot exceed {length.Value.Days} days." );
                return false;
            }

            httpActionResult = null;
            return true;
        }



        /// <summary>
        /// Sanitizes the period of time.
        /// </summary>
        /// <param name="start">The start of the period.</param>
        /// <param name="end">The end of the period.</param>
        protected void SanitizePeriod( ref DateTime? start, ref DateTime? end )
        {
            const int DefaultPeriodLength = 7;


            // If both start and end of period is not set.
            if( start == null && end == null )
            {
                end = DateTime.UtcNow;
                start = DateTime.UtcNow.Date.AddDays( DefaultPeriodLength * -1 );
            }


            // If start of period is not set
            else if( start == null && end != null )
            {
                start = end.Value.AddDays( DefaultPeriodLength * -1 );
            }


            // If end of period is not set.
            else if( start != null && end == null )
            {
                end = start.Value.AddDays( DefaultPeriodLength );
            }

            start = start.Value.ToUniversalTime();
            end = end.Value.ToUniversalTime();
        }


        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        public override void OnActionExecuting( ActionExecutingContext context )
        {
            var headers = context.HttpContext.Request.GetTypedHeaders();


            if( headers.Accept == null )
                return;

            
            if( headers.Accept.Contains( new MediaTypeHeaderValue( "application/schema+json" ) ) == false )
            {
                base.OnActionExecuting( context );
                return;
            }

            var cad = context.ActionDescriptor as ControllerActionDescriptor;
            var ra = cad.MethodInfo.GetCustomAttributes( false )
                            .OfType<SwaggerResponseAttribute>()
                            .Where( a => a.StatusCode == (int) HttpStatusCode.OK )
                            .FirstOrDefault();

            if( ra == null )
            {
                base.OnActionExecuting( context );
                return;
            }


            // Run this synchronously to prevent the actual action to be called
            var schema = JsonSchema4.FromTypeAsync( ra.Type, new NJsonSchema.Generation.JsonSchemaGeneratorSettings {
                
                DefaultPropertyNameHandling = PropertyNameHandling.CamelCase,
                FlattenInheritanceHierarchy = true                
            }).Result;

            var r = schema.ToJson( new NJsonSchema.Generation.JsonSchemaGeneratorSettings {

                DefaultPropertyNameHandling = PropertyNameHandling.CamelCase,
            });

            context.Result = Ok( r );
        }
    }
}
