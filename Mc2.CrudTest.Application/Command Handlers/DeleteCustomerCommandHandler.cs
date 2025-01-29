using Mc2.CrudTest.Application.Use_Cases;
using Mc2.CrudTest.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Application.Command_Handlers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly CustomerDbContext _context;

        public DeleteCustomerCommandHandler(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }
            var ts = Convert.FromBase64String(request.Timestamp);
            if (!customer.TimeStamp.SequenceEqual(ts))
            {
                throw new DbUpdateConcurrencyException("The record has been modified by another process.");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
