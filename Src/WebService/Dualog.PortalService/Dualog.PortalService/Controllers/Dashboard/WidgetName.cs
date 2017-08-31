using System;
using System.Linq;
using Newtonsoft.Json;

namespace Dualog.PortalService.Controllers.Dashboard
{
    public class WidgetName
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Category { get; set; }
        [JsonIgnore] public string LangScope { get; set; }
    }
}
