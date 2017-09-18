using System;
using System.Collections.Generic;
using System.Linq;

namespace Dualog.PortalService.Core
{
    public class Pagination
    {
        public Pagination( int? offset, int? count )
        {
            Offset = offset;
            Count = count;
        }

        public int? Offset { get; private set; }
        public int? Count { get; private set; }
    }

    public class Filtering
    {
        Dictionary<string, string[]> _filters = new Dictionary<string, string[]>();

        public Filtering()
        {
        }
    }
}
