using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Common.Model;
using Dualog.PortalService.Models;
using Serilog;
using Dualog.PortalService.Core;

namespace Dualog.PortalService.Repositories
{
    public class SqlModelRepository
    {
        IDataContext _dc;

        public SqlModelRepository( IDataContext dc )
        {
            _dc = dc;
        }


        public async Task<IEnumerable<SqlQueryResultParameter>> GetResultColumns( long sqlSelectInstanceId )
        {
            var qSqlId = from si in _dc.GetSet<ApSqlSelectInstance>()
                         from p in si.SqlSelect.ReturnParameters
                         where si.Id == sqlSelectInstanceId
                         select new SqlQueryResultParameter {

                             Id = p.Id,
                             ColumnName = p.Column.Name,
                             ParameterType = p.Column.ParameterType.Type,
                             TranslationScope = p.Column.LanguageRef,
                             Name = p.Column.Name
                         };


            return await qSqlId.ToArrayAsync();
        }


        public async Task CreateParametersForSelectInstance( long sqlSelectId, long sqlSelectInstanceId )
        {
            var sqlParameters = await GetQueryParametersAsync( sqlSelectId );
            var pkList = await _dc.GetSequenceNumbersAsync<ApSqlSelectParamInstance>( sqlParameters.Length );

            var db = ((Dualog.Data.Oracle.Entity.OracleDataContext) _dc).Database;

            for( int i = 0; i < sqlParameters.Length; i++ )
            {
                var param = sqlParameters[ i ];

                var sql =
                    "INSERT INTO AP_SQLSELECTPARAMINSTANCE " +
                    "(SPI_SQLSELECTPARAMINSTANCEID, SQI_SQLSELECTINSTANCEID, SSP_SQLSELECTPARAMID, SPI_RUNTIME) " +
                    "VALUES (:pk, :selid, :paramid, :runtime)";

                await db.ExecuteSqlCommandAsync( sql,
                       pkList[ i ],
                       sqlSelectInstanceId,
                       param.ParameterId,
                       param.BindName.ToLower() == "com_company" ? 1 : 0 );
            }
        }


        public async Task<SqlQueryParameter[]> GetQueryParametersAsync( long sqlSelectId )
        {
            var q = from si in _dc.GetSet<ApSqlSelect>()
                    from p in si.Parameters
                    where si.Id == sqlSelectId
                    select new SqlQueryParameter {
                        ParameterId = p.Id,
                        SelectId = p.SqlSelect.Id,
                        BindName = p.SqlParam.BindName
                    };

            var result = await q.ToArrayAsync();
            return result;
        }


        public async Task DeleteSqlSelectInstance( long instanceId )
        {
            var db = ((Dualog.Data.Oracle.Entity.OracleDataContext) _dc).Database;

            try
            {
                // Delete parameters
                var sql = "delete from AP_SQLSELECTPARAMINSTANCE " +
                          "where SQI_SQLSELECTINSTANCEID = :pk";
                await db.ExecuteSqlCommandAsync( sql, instanceId );


                // Delete the stats associated with the select instance
                sql = "delete from AP_SQLSELECTSTATS " +
                      "where SQI_SQLSELECTINSTANCEID = :pk";
                await db.ExecuteSqlCommandAsync( sql, instanceId );


                // Delete SqlSelectInstance
                sql = "delete from AP_SQLSELECTINSTANCE " +
                      "where SQI_SQLSELECTINSTANCEID = :pk";
                await db.ExecuteSqlCommandAsync( sql, instanceId );

            }
            catch( Exception exception )
            {
                Log.Error( "Unable to delete the SqlSelectInstance. reason: {exception}", exception );
                throw exception;
            }
        }
    }
}

