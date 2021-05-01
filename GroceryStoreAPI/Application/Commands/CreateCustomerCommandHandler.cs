using GroceryStoreAPI.Domain.Models;
using GroceryStoreAPI.Infrastructure.Repositories;
using GroceryStoreAPI.Infrastructure.Services;
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
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;
        public CreateCustomerCommandHandler(
            IMediator mediator,
            ICustomerRepository customerRepository,
            ILogger<CreateCustomerCommandHandler> logger
            )
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<bool> Handle(CreateCustomerCommand message, CancellationToken cancellationToken)
        {
            var customer = new Customer(message.Id, message.Name);
            _customerRepository.Add(customer);

            return await _customerRepository.Save(cancellationToken);
        }
    }

    [DataContract]
    public class CreateCustomerCommand
        : IRequest<bool>
    {
        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public Guid Id { get; private set; }
        public CreateCustomerCommand(string name, Guid userId)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

    }


}
