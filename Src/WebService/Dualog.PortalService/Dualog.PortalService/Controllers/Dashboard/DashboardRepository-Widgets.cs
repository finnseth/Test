using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dualog.Core.Translation;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Common.Model;
using Dualog.Data.Oracle.Entity;
using Dualog.PortalService.Core;
using Dualog.PortalService.Core.Data;
using Dualog.PortalService.Repositories;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Oracle.ManagedDataAccess.Client;
using Serilog;

namespace Dualog.PortalService.Controllers.Dashboard
{
    public partial class DashboardRepository
    {
        public async Task<IEnumerable<Widget>> GetWidgets(LanguageManager lm, long dashboardId)
        {
            Widget[] widgets;
            using (var dc = _dcFactory.CreateContext())
            {
                widgets = await CreateWidgetQuery(dc).Where(di => di.DashboardId == dashboardId).ToArrayAsync();
            }


            foreach (var widget in widgets)
            {
                var translation = lm.GetText(widget.TranslationScope, widget.Title);
                widget.Title = translation?.Text ?? widget.Title;
            }

            return widgets;
        }


        public async Task<Widget> GetWidgetById(LanguageManager lm, long id)
        {
            Widget widget;
            using (var dc = _dcFactory.CreateContext())
            {
                widget = await CreateWidgetQuery(dc).Where(di => di.Id == id).FirstOrDefaultAsync();
            }

            if (widget == null)
                return null;

            if (widget.Title == null)
                widget.Title = string.Empty;

            var translation = lm.GetText(widget.TranslationScope, widget.Title);
            widget.Title = translation?.Text ?? widget.Title;

            return widget;
        }


        IQueryable<Widget> CreateWidgetQuery(IDataContext dc)
        {
            var q = from di in dc.GetSet<ApDashboardInstance>()
                    from w in di.WidgetInstances
                    where w.RowStatus == 0
                    select new Widget
                    {

                        Id = w.Id,
                        Height = w.Height,
                        HorizontalRank = w.HorizontalRank,
                        Title = w.Title,
                        VerticalRank = w.VerticalRank,
                        Width = w.Widht,
                        WidgetType = w.DashboardWidgetGUI.DashboardWidgetType.Name,
                        WidgetName = w.DashboardWidgetGUI.DashboardWidget.Name,
                        TranslationScope = w.DashboardWidgetGUI.DashboardWidget.LanguageRef,
                        DashboardId = di.Id
                    };

            return q;
        }

        public async Task<Widget> CreateWidget(Widget widget, long dashboardId)
        {
            using (var dc = _dcFactory.CreateContext())
            using (var transaction = dc.BeginTransaction())
            {
                var db = ((OracleDataContext)dc).Database;

                try
                {
                    var t2 = GetDasboardGuiByType(dc, widget.WidgetType, widget.WidgetName);
                    var tDbWidgetSeq = dc.GetSequenceNumberAsync<ApDashboardWidgetInstance>();
                    var tSqlSelInstSeq = dc.GetSequenceNumberAsync<ApSqlSelectInstance>();
                    Task.WaitAll(tDbWidgetSeq, t2, tSqlSelInstSeq);

                    var sqlInstanceId = tSqlSelInstSeq.Result;


                    // Add a new SqlSelectInstance
                    var sql =
                        "INSERT INTO AP_SQLSELECTINSTANCE " +
                        "(SQI_SQLSELECTINSTANCEID, SQL_SQLSELECTID) " +
                        "VALUES (:pk1, :pk2)";

                    await db.ExecuteSqlCommandAsync(sql, sqlInstanceId, t2.Result.Item2.Id);


                    // Add a new Dashboard widget instance
                    sql =
                        "INSERT INTO AP_DASHBOARDWIDGETINSTANCE " +
                        "(DWI_DASHBOARDWIDGETINSTANCEID, SQI_SQLSELECTINSTANCEID, DWI_HORIZONTALRANK, DWI_VERTICALRANK, DWI_HEIGHT, DWI_WIDTH, DBI_DASHBOARDINSTANCEID, DWI_ROWSTATUS, DWG_DASHBOARDWIDGETGUIID, DWI_TITLE)" +
                        "VALUES (:id, :sqlselid, :hr, :vr, :height, :width, :dbinstid, 0, :dbwguiid, :title) ";

                    await db.ExecuteSqlCommandAsync(sql,
                                tDbWidgetSeq.Result,
                                sqlInstanceId,
                                widget.HorizontalRank,
                                widget.VerticalRank,
                                widget.Height,
                                widget.Width,
                                dashboardId,
                                t2.Result.Item1,
                                widget.Title);

                    // Get the parameters
                    var sqlModel = new SqlModelRepository(dc);
                    await sqlModel.CreateParametersForSelectInstance(t2.Result.Item2.Id, sqlInstanceId);


                    widget.Id = tDbWidgetSeq.Result;
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw exception;
                }
            }

            return widget;
        }


        public async Task<WidgetData> GetWidgetData(LanguageManager lm, Dictionary<string, string> runtimeParameters, long widgetId)
        {
            long sqlSelectInstanceId = 0;

            using (var dc = _dcFactory.CreateContext())
            {
                var q = from wi in dc.GetSet<ApDashboardWidgetInstance>()
                        where wi.Id == widgetId
                        select wi.SqlSelectInstance.Id;

                sqlSelectInstanceId = await q.FirstOrDefaultAsync();


                var result = new WidgetData();

                var data = await GetDataForWidget(runtimeParameters, sqlSelectInstanceId);
                if (data.Any() == false)
                    return result;

                var sqlModelRepository = new SqlModelRepository(dc);
                var columns = await sqlModelRepository.GetResultColumns(sqlSelectInstanceId);

                foreach (var k in data.First().Keys)
                {
                    var col = columns.FirstOrDefault(c => c.ColumnName.ToLower() == k.ToLower());
                    if (col == null)
                        continue;

                    var lt = lm.GetText(col.TranslationScope, col.Name);
                    col.Name = lt?.Text ?? col.Name;
                    result._columns.Add(col);
                }

                var dict = result._columns.ToDictionary(k => k.ColumnName.ToLower());

                var temp = new List<object>();
                foreach (var item in data)
                {
                    foreach (var k in item.Keys)
                    {
                        object val = item[k];

                        if (dict.ContainsKey(k.ToLower()) == true)
                        {
                            var col = dict[k.ToLower()];
                            if (col.ParameterType == "char")
                            {
                                var v = item[k].ToString();
                                var translation = lm.GetText(col.TranslationScope, v);
                                if (translation != null)
                                    val = translation.Text;
                            }
                        }

                        temp.Add(val);
                    }

                    result._data.Add(new WidgetDataItem
                    {
                        Field = temp[0].ToString(),
                        Value = temp[1]
                    });

                    temp.Clear();
                }

                return result;
            }
        }


        public async Task<Widget> UpdateWidgetAsync(int id, JObject json)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                (dc as IHasChangeDetection)?.EnableChangeDetection();

                var entity = await dc.GetSet<ApDashboardWidgetInstance>()
                                    .Include(di => di.DashboardInstance)
                                    .Include(p => p.DashboardWidgetGUI.DashboardWidget)
                                    .Include(p => p.DashboardWidgetGUI.DashboardWidgetType)
                                    .FirstOrDefaultAsync(di => di.Id == id);

                if (entity == null)
                    return null;

                // Create the object graph and do the patching
                var jog = new JsonObjectGraph(json, dc)
                    .AddPropertyMap("widgetname", "DashboardWidgetGUI")
                    .AddPropertyMap("widgettype", "DashboardWidgetGUI")
                    .AddPropertyMap("width", "Widht");

                bool guiIsHandled = false;


                async Task<object> call(string path, object value)
                {
                    if (guiIsHandled == false)
                    {

                        string widgetName = json.GetValue("widgetName", StringComparison.OrdinalIgnoreCase)?.Value<string>() ?? entity?.DashboardWidgetGUI?.DashboardWidget?.Name;
                        string widgetType = json.GetValue("widgetType", StringComparison.OrdinalIgnoreCase)?.Value<string>() ?? entity?.DashboardWidgetGUI?.DashboardWidgetType?.Name;  




                        guiIsHandled = true;
                        return await HandleGuiChange(dc, widgetName, widgetType);
                    }

                    return value;
                }

                jog.AddPropertyChangeHandler("/widgettype", async (path, value) => {
                    jog.IgnoreProperty("/widgetname");
                    return await call(path, value);
                } );
                jog.AddPropertyChangeHandler("/widgetname", async (path, value) => {
                    jog.IgnoreProperty("/widgettype");
                    return await call(path, value);
                });

                await jog.ApplyToAsync(entity, new DefaultContractResolver());

                var widget = Widget.FromApDashboardWidgetInstance(entity);
                if (widget.Validate(out var message) == false)
                    throw new ValidationException(message);

                await dc.SaveChangesAsync();

                return widget;
            }
        }

        async Task<ApDashboardWidgetGUI> HandleGuiChange(IDataContext dc, string widgetName, string widgetType)
        {
            var q = from g in dc.GetSet<ApDashboardWidgetGUI>().Include( w => w.DashboardWidget ).Include( w => w.DashboardWidgetType )
                    where g.DashboardWidgetType.Name.ToLower() == widgetType.ToLower()
                    where g.DashboardWidget.Name.ToLower() == widgetName.ToLower()
                    select g;

            var gui = await q.FirstOrDefaultAsync();
            if (gui == null)
                throw new ValidationException("Either the widget type or name does not exists, or the combination is invalid.");

            return gui;
        }


        public async Task<bool> HasWidgetAccess(long userId, long widgetId)
        {
            return true;

            //var dashboards = await GetDashboardsForUser( userId );

            //using( var dc = _dcFactory.CreateContext() )
            //{
            //    var tmRepository = new TargetModelRepository( dc );

            //    var q = from wi in dc.GetSet<ApDashboardWidgetInstance>()
            //            where dashboards.Contains( wi.DashboardInstance.Id )
            //            select wi.Id;

            //    return await q.AnyAsync();
            //}
        }


        public async Task DeleteWidget(long widget, long userId)
        {
            using( var dc = _dcFactory.CreateContext() )
            using( var transaction = dc.BeginTransaction() )
            {
                try
                {
                    await InternalDeleteWidget( dc, widget );

                    transaction.Commit();
                }
                catch( Exception exception )
                {
                    transaction.Rollback();
                    Log.Error( "Failed to delete widget with id {id}. Reason: {exception}", widget, exception );
                    throw exception;
                }
            }
        }


        private static async Task InternalDeleteWidget(IDataContext dc, long widgetId)
        {
            var db = ((OracleDataContext)dc).Database;

            string sql;

            // Delete the Widget Instance
            sql = "delete from AP_DASHBOARDWIDGETINSTANCE " +
                  "WHERE DWI_DASHBOARDWIDGETINSTANCEID = :pk";
            await db.ExecuteSqlCommandAsync(sql, widgetId);


            // Delete the SqlSelectInstance for the widget
            var sqlInstanceId = await dc.GetSet<ApDashboardWidgetInstance>()
                .Where(w => w.Id == widgetId)
                .Select(w => w.Id)
                .FirstOrDefaultAsync();

            var sqlModel = new SqlModelRepository(dc);
            await sqlModel.DeleteSqlSelectInstance(sqlInstanceId);
        }



        private async Task<WidgetName[]> GetWidgetNames(LanguageManager lm)
        {
            List<WidgetName> result;

            using (var dc = _dcFactory.CreateContext())
            {
                var q = from d in dc.GetSet<ApDashboardWidget>()
                        select new WidgetName
                        {

                            Name = d.Name,
                            Category = d.Category.Name,
                            LangScope = d.LanguageRef
                        };

                result = await q.ToListAsync();
            }

            foreach (var item in result)
            {
                var text = lm.GetText(item.LangScope, item.Name);
                item.Caption = text != null ? text.Text : item.Name;
            }

            return result.ToArray();
        }

        private async Task<WidgetType[]> GetWidgetTypes(LanguageManager lm)
        {
            List<WidgetType> result;


            using (var dc = _dcFactory.CreateContext())
            {
                var q = from d in dc.GetSet<ApDashboardWidgetType>()
                        select new WidgetType
                        {

                            Name = d.Name,
                            LangScope = d.LanguageRef
                        };

                result = await q.ToListAsync();
            }

            foreach (var item in result)
            {
                var text = lm.GetText(item.LangScope, item.Name);
                item.Caption = text != null ? text.Text : item.Name;
            }


            return result.ToArray();
        }


        private async Task<IEnumerable<Dictionary<string, object>>> GetDataForWidget(Dictionary<string, string> runtimeParameters, long sqlInstanceId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var q = from sqlInstance in dc.GetSet<ApSqlSelectInstance>()
                        from qp in sqlInstance.Parameters
                        where sqlInstance.Id == sqlInstanceId
                        where qp.Runtime == true
                        select new
                        {
                            qp.Id,
                            qp.SelectParam.SqlParam.BindName,
                        };
                var parameters = await q.ToArrayAsync();


                var x = new XDocument();
                var root = new XElement("parameters");
                x.Add(root);


                foreach (var p in parameters)
                {
                    var xParam = new XElement("param", new XAttribute("key", p.Id), new XAttribute("value", runtimeParameters[p.BindName]));
                    root.Add(xParam);
                }


                var conn = (OracleConnection)((IHasDataConnection)dc).GetDataConnection();
                conn.Open();


                OracleCommand command = conn.CreateCommand();
                command.CommandText = "RUN_SELECTINSTANCE_XML";
                command.CommandType = CommandType.StoredProcedure;
                command.BindByName = true;

                command.Parameters.Add(new OracleParameter("SQLSELECTINSTANCEID", OracleDbType.Long, 22, ParameterDirection.Input)).Value = sqlInstanceId;
                command.Parameters.Add(new OracleParameter("runtimeparameters", OracleDbType.XmlType)
                {
                    Direction = ParameterDirection.Input,
                    Value = x.ToString()
                });
                var outcursor = command.Parameters.Add("OUTCURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                var sqlstatid = command.Parameters.Add("SQLSTATID", OracleDbType.Decimal, ParameterDirection.Output);


                var da = new OracleDataAdapter(command);


                var dataSet = new DataSet();
                da.Fill(dataSet);


                var result = new List<Dictionary<string, object>>();

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var drow = new Dictionary<string, object>();

                    foreach (DataColumn column in dataSet.Tables[0].Columns)
                    {
                        drow[column.ColumnName] = row[column.ColumnName];
                    }

                    result.Add(drow);
                }

                return result;
            }
        }


        private async Task<Tuple<long, ApSqlSelect>> GetDasboardGuiByType(IDataContext dc, string type, string widgetName)
        {
            var q = from g in dc.GetSet<ApDashboardWidgetGUI>()
                    where g.DashboardWidgetType.Name.ToLower() == type.ToLower()
                    where g.DashboardWidget.Name.ToLower() == widgetName.ToLower()
                    select new
                    {
                        guidId = g.Id,
                        SqlSelect = g.DashboardWidget.SqlSelect
                    };

            var r = await q.FirstOrDefaultAsync();
            if (r == null)
                throw new InvalidOperationException($"Could not get widget information for type: {type} and widgetname {widgetName}");

            return new Tuple<long, ApSqlSelect>(r.guidId, r.SqlSelect);
        }
    }
}
