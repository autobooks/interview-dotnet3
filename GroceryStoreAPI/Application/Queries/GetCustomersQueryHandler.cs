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
    public class GetCustomersQueryHandler : IRequestHandler< GetCustomersQuery, List<Customer>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<GetCustomerByIdQueryHandler> _logger;
        public GetCustomersQueryHandler(
            ICustomerRepository customerRepository,
            ILogger<GetCustomerByIdQueryHandler> logger
            )
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository)); 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<Customer>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers;
        }

    }
    public class GetCustomersQuery : BaseQueryRequest<List<Customer>>
    {

    }
}
