using GroceryStoreAPI.Domain.Models;
using GroceryStoreAPI.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Application.Commands
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<DeleteCustomerCommandHandler> _logger;
        public DeleteCustomerCommandHandler(
            IMediator mediator,
            ICustomerRepository customerRepository,
            ILogger<DeleteCustomerCommandHandler> logger
            )
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<bool> Handle(DeleteCustomerCommand message, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetAsync(message.Id);

            if(customer == null)
            {
                return false;
            }

            _customerRepository.Delete(customer);

            return true;
        }
    }

    [DataContract]
    public class DeleteCustomerCommand
        : IRequest<bool>
    {
        [DataMember]
        public Guid Id { get; set; }
        public DeleteCustomerCommand(Guid id)
        {
            Id = id;
        }

    }
}
