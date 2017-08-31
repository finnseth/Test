using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Dualog.PortalService.Controllers
{
    public static class WebApiFuncs
    {
        public static async Task<IActionResult> HandleGetAction<T>( this DualogController controller, Func<Task<T>> action )
        {
            return await controller.HandleAction( action, c => controller.Ok( c )  );
        }

        public static async Task<IActionResult> HandlePostAction<T>( this DualogController controller, Func<Task<T>> action )
        {
            return await controller.HandleAction( action, c => controller.Created( "", c ) );
        }

        public static async Task<IActionResult> HandleDeleteAction<T>( this DualogController controller, Func<Task<T>> action )
        {
            return await controller.HandleAction( action, c => controller.Ok() );
        }

        public static async Task<IActionResult> HandlePatchAction<T>( this DualogController controller, Func<Task<T>> action )
        {
            return await controller.HandleAction( action, c => controller.Ok( c ) );
        }


        public static async Task<IActionResult> HandleAction<T>( this DualogController controller, Func<Task<T>> action, Func<T, IActionResult> wrapper )
        {
            try
            {
                return wrapper( await action() );
            }

            catch( NotFoundException exception )
            {
                Log.Error( "{Url} {Verb} failed: {Exception}", controller.Request.Path, controller.Request.Method, exception );
                return controller.NotFound();
            }

            catch( ValidationException exception )
            {
                Log.Error( "{Url} {Verb} failed: {Exception}", controller.Request.Path, controller.Request.Method, exception );
                return controller.BadRequest( new ErrorObject { Message = exception.Message } );
            }

            catch( Exception exception )
            {
                Log.Error( "{Url} {Verb} failed: {Exception}", controller.Request.Path, controller.Request.Method, exception );
                return new StatusCodeResult( (int) HttpStatusCode.InternalServerError );
            }
        }
    }
}
