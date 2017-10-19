using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace Dualog.PortalService.Core
{
    public static class SearchExtensions
    {
        public static Search Search(this HttpContext context)
        {
            string searchString = null;

            if (context.Request.Query.TryGetValue("search", out var search))
            {
                searchString = search.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(searchString))
                    return new Core.Search(null);
            }

            return new Search(searchString);
        }

        public static IQueryable<TSource> Search<TSource>(this IQueryable<TSource> queryable, Search search, params Expression<Func<TSource, string>>[] props)
        {
            if (string.IsNullOrWhiteSpace(search?.SearchString))
                return queryable;

            Expression predicate = null;
            var selectorParameter = Expression.Parameter(typeof(TSource), "c");

            foreach (var prop in props)
            {
                var propName = ((MemberExpression)prop.Body).Member.Name;

                // Create an expression that evaluates to:
                // c => c.ToLower().Contains(searchString.ToLower())
                var e1 = Expression.Call(
                                Expression.Call(
                                    Expression.PropertyOrField(selectorParameter, propName),
                                    "ToLower", null),
                                "Contains", null,
                                Expression.Constant(search.SearchString.ToLower()));

                // Or the expressions together if there are more than one expression
                if (predicate == null)
                    predicate = e1;
                else
                    predicate = Expression.OrElse(predicate, e1);
            }

            // Create a lambda of the predicate and wrap it inside a where clause
            var l = Expression.Lambda<Func<TSource, bool>>(predicate, selectorParameter);
            return queryable.Where(l);
        }

    }
}
