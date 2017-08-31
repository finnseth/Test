using System;
using System.Linq;

namespace Dualog.PortalService.Controllers.Dashboard
{
    public class DashboardConfig
    {
        public WidgetName[] Widgets { get; set; }
        public WidgetType[] WidgetTypes { get; set; }
    }
}
