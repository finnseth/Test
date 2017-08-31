using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.Dashboard
{
    public class WidgetData
    {
        internal List<SqlQueryResultParameter> _columns = new List<SqlQueryResultParameter>();
        internal List<WidgetDataItem> _data = new List<WidgetDataItem>();


        [Required] public SqlQueryResultParameter[] Columns => _columns.ToArray();
        [Required] public WidgetDataItem[] Data => _data.ToArray();
    }

    public class WidgetDataItem
    {
        [Required] public string Field { get; set; }
        [Required] public object Value { get; set; }
    }
}
