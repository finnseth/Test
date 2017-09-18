using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Dualog.PortalService.Core
{
    public static class PaginationExtensions
    {
        public static IQueryable<T> Paginate<T>( this IQueryable<T> queryable, Pagination pagination )
        {
            var q = queryable;

            if( pagination.Offset.HasValue == true )
                q = q.Skip( pagination.Offset.Value );

            if( pagination.Count.HasValue == true )
                q = q.Take( pagination.Count.Value );

            return q;
        }

        public static Pagination Pagination( this HttpContext context )
        {
            int? offset = null;
            int? count = null;

            if( context.Request.Query.TryGetValue( "offset", out var qsOffset ) )
            {
                if( int.TryParse( qsOffset.FirstOrDefault(), out var v ) == true )
                    offset = v;
            }

            if( context.Request.Query.TryGetValue( "count", out var qsCount ) )
            {
                if( int.TryParse( qsCount.FirstOrDefault(), out var v ) == true )
                    count = v;
            }

            return new Pagination( offset, count );
        }
    }
}
