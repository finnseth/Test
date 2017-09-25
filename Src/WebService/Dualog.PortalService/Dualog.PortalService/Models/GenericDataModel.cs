using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dualog.PortalService.Models
{
    public class GenericDataModel<T>
    {
        public T Value { get; set; }
        public int TotalCount { get; set; }
    }

}