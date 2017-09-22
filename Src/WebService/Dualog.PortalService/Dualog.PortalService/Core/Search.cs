using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dualog.PortalService.Core
{
    public class Search
    {

        public Search(string searchString )
        {
            SearchString = searchString;
        }

        public string SearchString { get; }

    }
}
