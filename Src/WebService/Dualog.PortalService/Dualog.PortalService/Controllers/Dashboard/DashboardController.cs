using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json.Linq;
using Dualog.PortalService.Core;

namespace Dualog.PortalService.Controllers.Dashboard
{
    [Authorize]
    [IsInCompany]
    [Route("api/v1")]
    public class DashboardController : DualogController
    {
        DashboardRepository _dbRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        public DashboardController( IDataContextFactory dcFactory )
            : base( dcFactory )
        {
            _dbRepository = new DashboardRepository(dcFactory);
        }


        /// <summary>
        /// Adds a new dashboard for the current logged in user.
        /// </summary>
        /// <param name="dashboard">The dashboard to add.</param>
        /// <returns></returns>
        /// <remarks>
        /// The Id property of the object will be ignored.
        /// </remarks>
        [HttpPost, Route("dashboards")]
        [SwaggerResponse((int)HttpStatusCode.Created, typeof(Dashboard), "The dashboard was successfully created.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(ErrorObject), "Something went wrong when creation the object.")]
        public async Task<IActionResult> AddDashboard([FromBody] Dashboard dashboard)
        {
            try
            {
                if (dashboard.Validate(out var result) == false)
                    return new BadRequestObjectResult(new ErrorObject { Message = result });


                var created = await _dbRepository.CreateDashboard(dashboard, UserId);
                return Created($"{Request.Host}/api/v1/dashboards", dashboard);
            }
            catch (Exception exception)
            {
                Log.Error("AddDashboard failed: {Exception}", exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }

        }


        /// <summary>
        /// Create a new widget for the specified dashboard
        /// </summary>
        /// <param name="id">The id of the dashboards for which to create a widget.</param>
        /// <param name="widget">The widget data.</param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.Created, typeof(Widget), "The created widget.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(ErrorObject), "Something went wrong when adding the widget.")]
        [HttpPost, Route("dashboards/{id}/widgets")]
        public async Task<IActionResult> AddWidgetToDashboard(long id, [FromBody] Widget widget)
        {
            try
            {
                string result;
                if (widget.Validate(out result) == false)
                    return new BadRequestObjectResult(new ErrorObject { Message = result });


                var created = await _dbRepository.CreateWidget(widget, id);
                return Created($"{Request.Host}/api/v1/dashboards/widgets/{created.Id}", created);
            }
            catch (Exception exception)
            {
                Log.Error("GetWidgets failed: {Exception}", exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Deletes the specified dashboard.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "The operation was successful.")]
        [HttpDelete, Route("dashboards/{id}")]
        public async Task<IActionResult> DeleteDashboard(int id)
        {
            try
            {
                await _dbRepository.DeleteDashboard(id, UserId);
                return Ok();
            }
            catch (Exception exception)
            {
                Log.Error("DeleteDashboard failed: {exception}", exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Updates the dashboard with the specified id.
        /// </summary>
        /// <param name="id">The id of the dashboard to modify.</param>
        /// <param name="json">The patch document describing the changes.</param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Dashboard), "The dashboard was successfully updated.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(ErrorObject), "Something went wrong when updating the dashboard.")]
        [HttpPatch, Route("dashboards/{id}")]
        public async Task<IActionResult> UpdateDashboard(long id, [FromBody] JObject json )
        {
            try
            {
                var dashboard = await _dbRepository.UpdateDashboardAsync(id, json);
                return Ok(dashboard);
            }

            catch(ValidationException exception)
            {
                return new BadRequestObjectResult(new ErrorObject { Message = exception.Message });
            }

            catch (Exception exception)
            {
                Log.Error("Updating dashboard failed: {exception}", exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Gets the configuration data for dashboards.
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(DashboardConfig), "The operation was successful.")]
        [HttpGet, Route("dashboards/config")]
        public async Task<IActionResult> GetDashboardConfig()
        {
            try
            {
                return Ok(await _dbRepository.GetDashboardConfig(await GetLanguageManager()));
            }
            catch (Exception exception)
            {
                Log.Error("GetDashboardConfig failed: {Exception}", exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Gets the all dashboards for the current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("dashboards")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Dashboard), "The operation was successful.")]
        public async Task<IActionResult> GetDashboards()
        {
            try
            {
                Log.Information("Getting dashboards for user {user}", UserId);
                return Ok(await _dbRepository.GetDashboards(UserId));
            }
            catch (Exception exception)
            {
                Log.Error("GetDashboards failed: {Exception}", exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Gets the widget with the specified id
        /// </summary>
        /// <param name="id">The id of the widget to get.</param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Widget), "The operation was successful.")]
        [HttpGet, Route("dashboards/widgets/{id}")]
        public async Task<IActionResult> GetWidgetById(int id)
        {
            try
            {
                var w = await _dbRepository.GetWidgetById(await GetLanguageManager(), id);
                return Ok(w);
            }
            catch (Exception exception)
            {
                Log.Error("GetWidgetById failed: {Exception}", exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }

        }

        /// <summary>
        /// Deletes the specified widget.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "The operation was successful.")]
        [HttpDelete, Route("dashboards/widgets/{id}")]
        public async Task<IActionResult> DeleteWidget(int id)
        {
            try
            {
                await _dbRepository.DeleteWidget(id, UserId);
                return Ok();
            }
            catch (Exception exception)
            {
                Log.Error("Failed to delete the widget with id {id}: {Exception}", id, exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Updates the widget with the specified id.
        /// </summary>
        /// <param name="id">The id of the widget to modify.</param>
        /// <param name="patchDocument">The patch document describing the changes.</param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Widget), "The operation was successful.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(ErrorObject), "Something went wrong when updating the widget.")]
        [HttpPatch, Route("dashboards/widgets/{id}")]
        public async Task<IActionResult> UpdateWidget(int id, [FromBody] JObject json )
        {
            try
            {
                var widget = await _dbRepository.UpdateWidgetAsync(id, json);
                return Ok(widget);
            }

            catch( ValidationException exception)
            {
                return new BadRequestObjectResult(new ErrorObject { Message = exception.Message });
            }

            catch (Exception exception)
            {
                Log.Error("Failed to update the widget with id {id}: {Exception}", id, exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Gets the widget data for the given widget.
        /// </summary>
        /// <param name="id">The id of the widget for which to get the data.</param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(WidgetData), "The operation was successful.")]
        [SwaggerResponse((int)HttpStatusCode.Forbidden, Description = "The user is not allowed to retrieve data for the specified widget.")]
        [HttpGet, Route("dashboards/widgets/{id}/data")]
        public async Task<IActionResult> GetWidgetData(long id)
        {
            try
            {
                // Check whether the user has access to the specified data
                if (await _dbRepository.HasWidgetAccess(UserId, id) == false)
                    return Forbid();

                var rp = new Dictionary<string, string>()
                {
                    ["com_companyid"] = CompanyId.ToString(),
                    ["usr_userid"] = UserId.ToString()
                };

                return Ok(await _dbRepository.GetWidgetData(await GetLanguageManager(), rp, id));
            }
            catch (Exception exception)
            {
                Log.Error("GetWidgetData failed: {Exception}", exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Gets the widgets available for the current user.
        /// </summary>
        /// <param name="id">The id of the dashboard for which to get the widgets.</param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Widget), "The operation was successful.")]
        [SwaggerResponse((int)HttpStatusCode.Forbidden, Description = "The user is not allowed to retrieve the specified widgets.")]
        [HttpGet, Route("dashboards/{id}/widgets")]
        public async Task<IActionResult> GetWidgets(long id)
        {
            try
            {
                // Check whether the user has access to the specified data
                if (await _dbRepository.HasWidgetAccess(UserId, id) == false)
                    return Forbid();

                return Ok(await _dbRepository.GetWidgets(await GetLanguageManager(), id));

            }
            catch (Exception exception)
            {
                Log.Error("GetWidgets failed: {Exception}", exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
