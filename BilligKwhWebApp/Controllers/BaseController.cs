using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Core;
using BilligKwhWebApp.Infrastructure.DataTransferObjects.Common;
using BilligKwhWebApp.Services;
using BilligKwhWebApp.Services.Enums;

namespace BilligKwhWebApp.Controllers
{
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        private readonly ISystemLogger _logger;
        private readonly IWorkContext _workContext;

        protected ISystemLogger Logger => _logger;
        protected IWorkContext WorkContext => _workContext;

        protected IPermissionService _permissionService { get; private set; }

        public BaseController(ISystemLogger logger,
                              IWorkContext workContext,
                              IPermissionService permissionService)
        {
            _logger = logger;
            _workContext = workContext;
            _permissionService = permissionService;
        }

        #region Public

        public override BadRequestObjectResult BadRequest(object error)
        {
            var shortMessage = string.Format(CultureInfo.InvariantCulture, "BadRequest: Controller: {0}   Action: {1}",
                ControllerContext.RouteData.Values["controller"],
                ControllerContext.RouteData.Values["action"]);

            int logId = _logger.Warning(shortMessage, null, "", _workContext.CurrentUser, ControllerContext.RouteData.Values["controller"].ToString(), error);

            ErrorDto errorWithLogId = new(error, logId);

            return base.BadRequest(errorWithLogId);
        }

        public override ConflictObjectResult Conflict(object error)
        {
            var shortMessage = string.Format(CultureInfo.InvariantCulture, "Conflict: Controller: {0}   Action: {1}",
                ControllerContext.RouteData.Values["controller"],
                ControllerContext.RouteData.Values["action"]);

            int logId = _logger.Warning(shortMessage, null, "", _workContext.CurrentUser, ControllerContext.RouteData.Values["controller"].ToString(), error);

            ErrorDto errorWithLogId = new(error, logId);

            return base.Conflict(errorWithLogId);
        }


        public override NotFoundObjectResult NotFound(object value)
        {
            var shortMessage = string.Format(CultureInfo.InvariantCulture, "NotFound: Controller: {0}   Action: {1}",
                ControllerContext.RouteData.Values["controller"],
                ControllerContext.RouteData.Values["action"]);

            int logId = _logger.Warning(shortMessage, null, "", _workContext.CurrentUser, ControllerContext.RouteData.Values["controller"].ToString(), value);

            return base.NotFound(value);
        }

        /// <summary>
        /// Custom 403 Forbid taking an errorObject. This could just be a string with message or some JSON object
        /// </summary>
		protected ObjectResult ForbidWithMessage(object error)
        {
            var shortMessage = string.Format(CultureInfo.InvariantCulture, "Forbid: Controller: {0}   Action: {1}",
                ControllerContext.RouteData.Values["controller"],
                ControllerContext.RouteData.Values["action"]);

            int logId = _logger.Warning(shortMessage, null, "", _workContext.CurrentUser, ControllerContext.RouteData.Values["controller"].ToString(), error);

            ErrorDto errorWithLogId = new(error, logId);

            return base.StatusCode(403, errorWithLogId);
        }

        protected UnauthorizedObjectResult UnauthorizedWithMessage(string errorMessage)
        {
            var shortMessage = string.Format(CultureInfo.InvariantCulture, "Unauthorized: Controller: {0}   Action: {1}",
                ControllerContext.RouteData.Values["controller"],
                ControllerContext.RouteData.Values["action"]);

            int logId = _logger.Warning(shortMessage, null, "",
                _workContext.CurrentUser,
                ControllerContext.RouteData.Values["controller"].ToString(),
                new { ErrorMessage = errorMessage, WorkContext = _workContext });

            ErrorDto errorWithLogId = new(errorMessage, logId);

            return base.Unauthorized(errorWithLogId);
        }



        #endregion
    }
}