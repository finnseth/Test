using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Common.Model;
using Dualog.PortalService.Models;
using Dualog.PortalService.Repositories;

namespace Dualog.PortalService.Controllers.Dashboard
{
    public partial class DashboardRepository
    {
        IDataContextFactory _dcFactory;

        public DashboardRepository( IDataContextFactory dcFactory )
        {
            _dcFactory = dcFactory;
        }

    }
}
