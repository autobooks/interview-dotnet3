using GroceryStoreAPI.Domain.Models;
using GroceryStoreAPI.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Application.Queries
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerbyIdQuery, Customer>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<GetCustomerByIdQueryHandler> _logger;
        public GetCustomerByIdQueryHandler(
            ICustomerRepository customerRepository,
            ILogger<GetCustomerByIdQueryHandler> logger
            )
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository)); 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Customer> Handle(GetCustomerbyIdQuery message, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetAsync(message.Id);
            return customer;
        }
    }

    public class GetCustomerbyIdQuery : IRequest<Customer>
    {
        public Guid Id{ get; set; }
    }

}
