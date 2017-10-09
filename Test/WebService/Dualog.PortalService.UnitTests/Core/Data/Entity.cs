using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Entity;

namespace Dualog.PortalService.Core.Data
{
    [TableSequence("TestSequence")]
    public class Entity : IEntity
    {
        public long Id { get; set; }
        public bool IsSometing { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }


        public Entity Nested { get; set; }
        public List<Entity> Collection { get; set; }
        public ArrayList UntypedList { get; set; }
        public List<object> ObjectCollection { get; set; }
    }
}
