using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Dualog.PortalService.Controllers
{
    public class Result
    {
        public bool Success { get; private set; }
        public string Error { get; private set; }
        public bool Failure => !Success;

        protected Result( bool success, string error ) {

            Success = success;
            Error = error;
        }

        public static Result Fail( string message )
        {
            return new Result( false, message );
        }

        public static Result<T> Ok<T>( T value )
        {
            return new Result<T>( value, true, string.Empty );
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; private set; }

        protected internal Result( T value, bool success, string error )
            : base( success, error )
        {
            Value = value;
        }
    }

    public static class Functional
    {
        public static void Use<T>( this T obj, Action<T> action ) where T : IDisposable
        {
            using( obj )
            {
                action( obj );
            }
        }

        public static R Use<T, R>( this T obj, Func<T, R> action ) where T : IDisposable
        {
            R result = default(R);

            using( obj )
            {
                result = action( obj );
            }

            return result;
        }
    }

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
