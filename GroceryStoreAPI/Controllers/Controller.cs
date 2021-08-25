using GroceryStoreAPI.BusinessLogic;
using GroceryStoreAPI.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Controllers
{
    [Produces("application/json")]
    [ApiController, Route("api/v{version:apiVersion}/customers/")]
    public class Controller : ControllerBase
    {
        private readonly ILogger<Controller> _logger;
        private readonly IBusinessLogic _businessLogic;

        public Controller(IBusinessLogic bl, ILogger<Controller> logger)
        {
            _businessLogic = bl;
            _logger = logger;
        }

        /// <summary>
        ///     Return a list of all customers
        /// </summary>
        /// <returns>List of customers</returns>
        /// <response code="200">OK</response>
        [AllowAnonymous]
        [HttpGet("/customers")]
        public async Task<IEnumerable<SelectContract>> GetAllCustomersAsync()
        {
            return await _businessLogic.GetAllAsync();
        }

        /// <summary>
        ///     Return a customer by id
        /// </summary>
        /// <returns>List of customers by id</returns>
        /// <response code="200">OK</response>
        [AllowAnonymous]
        [HttpGet("/customers/{customerId}")]
        public async Task<SelectContract> GetCustomerByIdAsync(Guid customerId)
        {
            return await _businessLogic.GetCustomersByIdAsync(customerId);
        }

        /// <summary>
        ///     Delete all customers associated to a particular id
        /// </summary>
        /// <returns>boolean, true if a record was deleted for a specific id</returns>
        /// <response code="200">OK</response>
        [HttpPost("/cleanup/{customerId}")]
        public async Task<bool> DeleteCustomersByIdAsync(Guid customerId)
        {
            return await _businessLogic.DeleteAsync(customerId);
        }

        /// <summary>
        ///     Update customers associated to a particular id.
        /// </summary>
        /// <returns>boolean, true if a record was updated for specific id</returns>
        /// <response code="200">OK</response>
        [HttpPost("/update")]
        public async Task<bool> UpdateCustomersByIdAsync(UpdateContract updateContract)
        {
            return await _businessLogic.UpdateAsync(updateContract);
        }

        /// <summary>
        ///     Insert a new customer.
        /// </summary>
        /// <returns>boolean, true  if a new record was added</returns>
        /// <response code="200">OK</response>
        [HttpPost("/insert")]
        public async Task<bool> InsertCustomerAsync(string name)
        {
            return await _businessLogic.InsertAsync(name);
        }
    }
}