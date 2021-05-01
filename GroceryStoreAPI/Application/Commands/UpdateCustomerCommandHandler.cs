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
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateCustomerCommandHandler> _logger;
        public UpdateCustomerCommandHandler(
            IMediator mediator,
            ICustomerRepository customerRepository,
            ILogger<UpdateCustomerCommandHandler> logger
            )
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<bool> Handle(UpdateCustomerCommand message, CancellationToken cancellationToken)
        {
            var customer = new Customer(message.Id, message.Name);
            _customerRepository.Update(customer);

            return await _customerRepository.Save(cancellationToken);
        }
    }

    [DataContract]
    public class UpdateCustomerCommand
        : IRequest<bool>
    {
        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public Guid Id { get; private set; }
        public UpdateCustomerCommand(string name, Guid id)
        {
            Id = id;
            Name = name;
        }

    }
}
