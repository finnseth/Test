using System;
using System.Linq;

namespace Dualog.PortalService.Repositories
{
    public class SqlQueryParameter
    {
        public long ParameterId { get; set; }
        public long SelectId { get; set; }
        public string BindName { get; set; }

    }
}
