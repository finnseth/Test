using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Common.Model;
using Serilog;
using Dualog.PortalService.Core;

namespace Dualog.PortalService.Repositories
{
    public class TargetModelRepository
    {
        IDataContext _dc;

        public TargetModelRepository( IDataContext dc )
        {
            _dc = dc;
        }



        public async Task<ApTargetType> GetTargetType( TargetType targetType )
        {
            var q = from tt in _dc.GetSet<ApTargetType>()
                    where tt.Name == targetType.ToString()
                    select tt;

            return await q.FirstOrDefaultAsync();
        }


        public async Task<ApTarget> GetTarget( TargetType targetType, string targetValue )
        {
            string tt = targetType.ToString();

            var q = from t in _dc.GetSet<ApTarget>()
                    where t.TargetType.Name == tt && t.Value == targetValue
                    select t;

            return await q.FirstOrDefaultAsync();
        }


        public async Task<ApTarget> CreateTarget( TargetType targetType, string targetValue )
        {
            var target = _dc.Add( new ApTarget {
                Id = await _dc.GetSequenceNumberAsync<ApTarget>(),
                TargetType = _dc.Attach( await GetTargetType( targetType ) ),
                Value = targetValue
            } );

            await _dc.SaveChangesAsync();
            Log.Debug( "Added ApTarget with {Id}", target.Id );

            return target;
        }

    }
}
