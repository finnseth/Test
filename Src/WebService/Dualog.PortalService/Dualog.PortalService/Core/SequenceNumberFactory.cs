using System;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;

namespace Dualog.PortalService.Core
{
    public static class SequenceNumberFactory
    {
        public static async Task<long> GetSequenceNumberAsync<T>(this IDataContext dataContext) where T : class, IEntity
        {
            if (dataContext is ICanCreateSequenceNumbers csn)
                return await csn.GetSequenceNumberAsync<T>();
            else
                throw new InvalidOperationException("Unknown data source. Cannot get sequence number,");
        }

        public static async Task<long> GetSequenceNumberAsync( this IDataContext dataContext, Type type )
        {
            if (dataContext is ICanCreateSequenceNumbers csn)
                return await csn.GetSequenceNumberAsync(type);
            else
                throw new InvalidOperationException("Unknown data source. Cannot get sequence number,");
        }

        public static async Task<int[]> GetSequenceNumbersAsync<T>( this IDataContext dataContext, int count ) where T : class, IEntity
        {
            if (dataContext is ICanCreateSequenceNumbers csn)
                return await csn.GetSequenceNumbersAsync<T>(count);
            else
                throw new InvalidOperationException("Unknown data source. Cannot get sequence number,");

        }
    }

}
