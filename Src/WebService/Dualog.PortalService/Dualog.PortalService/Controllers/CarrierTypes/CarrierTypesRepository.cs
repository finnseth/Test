using System;
using System.Collections.Generic;
using System.Linq;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Methods.Model;
using System.Threading.Tasks;
using Dualog.Data.Oracle.Shore.Model;
using System.Data.Entity;

namespace Dualog.PortalService.Controllers.Methods
{
    public class CarrierTypesRepository
    {
        IDataContextFactory _dcFactory;

        public CarrierTypesRepository( IDataContextFactory dcFactory )
        {
            _dcFactory = dcFactory;
        }

        public async Task<IEnumerable<CarrierTypeDetails>> GetMethods()
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                var q = from m in dc.GetSet<DsCarrier>()
                        where (m.RowStatus ?? 0) == 0
                        select new CarrierTypeDetails {
                            Id = m.Id,
                            ModemHandShake = m.ModemHandShake ?? 0,
                            Name = m.Name,
                            Priority = m.Priority ?? 0,
                        };

                return await q.ToListAsync();
            }
        }
    }
}
