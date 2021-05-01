using GroceryStoreAPI.Application.Commands;
using GroceryStoreAPI.Application.Queries;
using GroceryStoreAPI.Domain.Models;
using GroceryStoreAPI.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class CustomersControllers : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly ILogger<CustomersControllers> _logger;

        public CustomersControllers(IMediator mediator,
            IIdentityService identityService,
            ILogger<CustomersControllers> logger)
        {
            _mediator = mediator;
            _identityService = identityService;
            _logger = logger;
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand createCustomerCommand)
        {
            _logger.LogInformation(
                "----- Sending command: {CommandName} - {Name}: {CommandId} ({@Command})",
                nameof(createCustomerCommand),
                nameof(createCustomerCommand.Name),
                createCustomerCommand.Id,
                createCustomerCommand);

            bool commandResult = await _mediator.Send(createCustomerCommand);
            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        [Route("{customerId:Guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetCustomerAsync([FromQuery] Guid customerId)
        {
            var query = new GetCustomerbyIdQuery()
            { Id = customerId };
            
            try
            {
                var customer = await _mediator.Send(query);

                return Ok(customer);
            }
            catch
            {
                return NotFound();
            }
        }


        [Route("{customerId:Guid}")]
        [HttpPut]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateCustomerAsync([FromBody] UpdateCustomerCommand command)
        {           
            try
            {
                var customer = await _mediator.Send(command);

                return Ok(customer);
            }
            catch
            {
                return NotFound();
            }
        }


        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(List<Customer>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetCustomersAsync([FromQuery] Guid customerId)
        {
            var query = new GetCustomersQuery();

            try
            {
                var customer = await _mediator.Send(query);

                return Ok(customer);
            }
            catch
            {
                return NotFound();
            }
        }


        [Route("customerId:Guid")]
        [HttpDelete]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteCustomerAsync([FromQuery] Guid customerId)
        {
            var command = new DeleteCustomerCommand(customerId);

            try
            {
                return Ok(await _mediator.Send(command));
            }
            catch
            {
                return NotFound();
            }
        }


    }
}
