using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using LinqKit;

namespace Dualog.PortalService.Core
{
    public static class SearchExtensions
    {
        public static Search Search(this HttpContext context)
        {
            string searchString = null;
            int searchLimit = 10;

            if (context.Request.Query.TryGetValue("search", out var search))
            {
                searchString = search.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(searchString))
                    return Core.Search.Empty;
            }


            if (context.Request.Query.TryGetValue("searchLimit", out var sl))
                if (int.TryParse(sl.FirstOrDefault(), out var v) == true)
                    searchLimit = v;


            return new Search(searchString, searchLimit);
        }

        public static IQueryable<TSource> Search<TSource, TProperty>(this IQueryable<TSource> queryable, Search search, params Expression<Func<TSource, string>>[] props)
        {
            //if (search == Core.Search.Empty)
            //    return queryable;



            //foreach (var prop in props)
            //{
            //    var toString = typeof(object).GetMethod("ToString");
            //    ConstantExpression c = Expression.Constant( )
               
            //    prop.Expand()


            //    //Expression<TSource, bool> where = c => prop.ToUpper().Contains(search.SearchString.ToUpper());

            //    var compiled = prop.Compile();
            //    compiled( )

            //   //((queryable = queryable.Where(c => c.Name.ToUpper().Contains(search.SearchString.ToUpper()));
            //}


            //if (search.Limit > 0)
            //    queryable = queryable.Take(search.Limit);

            return queryable;

        }

    }
}
