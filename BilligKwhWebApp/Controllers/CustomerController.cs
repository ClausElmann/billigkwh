using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilligKwhWebApp.Core;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Localization;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Models;
using BilligKwhWebApp.Core.Factories;
using MediatR;
using BilligKwhWebApp.Services;
using BilligKwhWebApp.Infrastructure.DataTransferObjects.Common;
using BilligKwhWebApp.Services.Customers;

namespace BilligKwhWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(UserRolePermissionProvider.Bearer)]
    public class CustomerController : BaseController
    {
        // Dependencies
        private readonly ICustomerService _customerService;
        private readonly ICustomerFactory _customerfactory;
        private readonly IUserFactory _userFactory;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly IMediator _mediator;
        private readonly IApplicationSettingService _applicationSettingService;
        private readonly IEmailService _emailService;

        // Ctor
        public CustomerController(
            ISystemLogger logger,
            ICustomerService customerService,
            IWorkContext workContext,
            IUserFactory userFactory,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            ICustomerFactory customerFactory,
            IApplicationSettingService applicationSettingService,
            IMediator mediator,
            IEmailService emailService) : base(logger, workContext, permissionService)
        {
            _customerService = customerService;
            _customerfactory = customerFactory;
            _applicationSettingService = applicationSettingService;
            _mediator = mediator;
            _userFactory = userFactory;
            _workContext = workContext;
            _localizationService = localizationService;
            _emailService = emailService;
        }

        // Public Api
        /// <summary>
        /// Get customer by either customer ID or the public GUID id. If no id is provided, current customer is returned.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        [ProducesDefaultResponseType]
        public ActionResult<CustomerModel> GetCustomer(int? id, Guid? publicId)
        {
            Customer customer;

            if (id != null)
            {
                customer = _customerService.Get(id.Value);
            }
            //else if (publicId.HasValue)
            //{
            //    customer = _customerService.GetByGuid(publicId.Value);
            //}
            else
            {
                customer = _workContext.CurrentCustomer;
            }

            if (customer == null) return BadRequest(new { ErrorMessage = "Customer not found", WorkContext = _workContext });

            var model = _customerfactory.CreateCustomerModel(customer);

            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UpdateCustomer([FromBody] CustomerModel model)
        {
            if (model == null) return BadRequest("Model is null");
            if (!_workContext.IsUserSuperAdmin())
            {
                return ForbidWithMessage("User must have role ManageCustomer or SubscriptionModule");
            }

            if (model.Id > 0)
            {
                var entity = _customerService.Get(model.Id);

                bool isSuperAdmin = PermissionService.DoesUserHaveRole(_workContext.CurrentUser.Id, UserRolesEnum.SuperAdmin);

                // Only allow this if current customer is the one to update. If not, user must be super admin
                if (entity != null)
                {
                    if (_workContext.CurrentCustomer.Id != entity.Id && !isSuperAdmin)
                        return ForbidWithMessage("User must be super admin to update a different customer than current!");

                    entity.Name = model.Name;
                    entity.Address = model.Address;
                    entity.Deleted = model.Deleted;
                    entity.LanguageId = model.LanguageId;
                    entity.Name = model.Name;
                    entity.CountryId = model.CountryId;
                    entity.CompanyRegistrationId = model.CompanyRegistrationId;
                    entity.TimeZoneId = model.TimeZoneId;
                 
                    entity.SetTidzoneId(entity.CountryId);

                    _customerService.Update(entity);


                    //if (!string.IsNullOrWhiteSpace(entity.FakturaMail))
                    //{
                    //    _customerService.CreateOrUpdateEconomicCustomer(entity.Id);
                    //}

                    return Ok(entity.Id);
                }
                return BadRequest(new { ErrorMessage = "Customer not found", Model = entity });
            }
            else
            {
                var customer = _customerfactory.CreateCustomerEntity(model);
                _customerService.Create(customer);

                //if (!string.IsNullOrWhiteSpace(customer.FakturaMail))
                //{
                //    _customerService.CreateOrUpdateEconomicCustomer(customer.Id);
                //}
                return Ok(customer.Id);
            }
        }
             

        /// <summary>
        /// Get all customers. Optionally, you can pass a countryId for filtering by country
        /// </summary>
        /// <param name="inclDeleted">Set true if the result should include deleted customers as well</param>
        [HttpGet, Authorize(UserRolePermissionProvider.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CustomerModel>))]
        public IActionResult GetCustomers(int? countryId, bool onlyDeleted = false)
        {
            var customers = _customerService.GetAll(onlyDeleted);
            if (customers != null)
            {
                if (countryId.HasValue)
                    customers = customers.Where(c => c.CountryId == countryId.Value).ToList();

                var customerModels = customers.Select(c => _customerfactory.CreateCustomerModel(c));

                return Ok(customerModels.OrderBy(c => c.Name));
            }
            else
            {
                return Ok(new List<CustomerModel>());
            }
        }

        /// <summary>
        /// Returns a list of all possible user roles along with a flag telling whether users will become the role or not when created 
        /// </summary>
        /// <param name="onlyHasAccess">Set true if only the user roles that the customer's user has access to should be returned</param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserRoleAccessModel>))]
        public IActionResult GetCustomerUserRoleAccess(int customerId, bool? onlyHasAccess)
        {
            var user = _workContext.CurrentUser;
            var roles = PermissionService.GetAllUserRoles();
            var mappings = PermissionService.GetCustomerUserRoleMappings(customerId);
            var accessModels = _customerfactory.PrepareCustomerUserRoleAccessModels(customerId, roles.ToList(), mappings.ToList(), user.LanguageId);

            if (onlyHasAccess.HasValue && onlyHasAccess.Value)
                accessModels = accessModels.Where(model => model.HasAccess == true).ToList(); // fiter away roles that users will not have access to

            return Ok(accessModels.OrderBy(ua => ua.UserRole.NameLocalized));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void SendUnsentEmails()
        {
            var unsendEmails = _emailService.GetAllUnsent(DateTime.Now.AddDays(-30));

            foreach (var item in unsendEmails)
            {
                _emailService.SendMail(item);
            }
        }

        //[HttpPost]
        //public async Task<ActionResult<int>> CreateOrUpdateEconomicCustomer(int kundeId)
        //{
        //    try
        //    {
        //        var economicId = await _customerService.CreateOrUpdateEconomicCustomer(kundeId).ConfigureAwait(false);
        //        return Ok(economicId);
        //    }
        //    catch (EconomicException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}



        //// Public Api
        ///// <summary>
        ///// Get customer by either customer ID or the public GUID id. If no id is provided, current customer is returned.
        ///// </summary>
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        //[ProducesDefaultResponseType]
        //public async Task<IActionResult> CreateInEconomic([FromBody] CustomerModel model)
        //{
        //    Kunde customer = _customerService.Get(model.Id);

        //  //  var cust = await _economicClient.CreateCustomer(model).ConfigureAwait(false);

        //    var economicInvoiceDraft = await _economicHttpClient.CreateInvoice(model).ConfigureAwait(false);


        //    return Ok(model);
        //}


    }
}