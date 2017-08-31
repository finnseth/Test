using System;
using System.Collections.Generic;
using System.Linq;

namespace Dualog.PortalService.Data
{
    public class TestData
    {
        public Entity[] Entities { get; set; }

    }

    public class Entity
    {
        public string Id { get; set; }
        public string Host { get; set; }
        public string Type { get; set; }
        public Dictionary<string,object> Properties {get;set;}
    }
}
