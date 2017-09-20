using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dualog.PortalService.Core
{
    public class Search
    {
        static readonly Search _empty = new Search(null, 0);

        public Search(string searchString, int limit)
        {
            SearchString = searchString;
            Limit = limit;
        }

        public string SearchString { get; }
        public int Limit { get; }

        public static Search Empty => _empty;

        public static bool operator == (Search left, Search right)
        {
            if (left == null && right == null)
                return true;

            return left.SearchString == right.SearchString;
        }

        public static bool operator !=(Search left, Search right)
        {
            return (left.Equals( null ) || right.Equals(null)) && !(left == right);

        }

    }
}
