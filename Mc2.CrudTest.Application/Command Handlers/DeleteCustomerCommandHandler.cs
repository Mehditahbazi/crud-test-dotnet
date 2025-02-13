using Mc2.CrudTest.Application.Use_Cases;
using Mc2.CrudTest.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Application.Command_Handlers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ICustomerRepository customerRepository;

        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetByIdAsync(request.Id);

            if (customer == null)
            {
                throw new InvalidOperationException("Customer not found.");
            }
            var ts = Convert.FromBase64String(request.Timestamp);
            if (!customer.TimeStamp.SequenceEqual(ts))
            {
                throw new DbUpdateConcurrencyException("The record has been modified by another process.");
            }

            await customerRepository.DeleteAsync(request.Id, ts);
            return true;
        }
    }
}
