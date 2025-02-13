using Mc2.CrudTest.Application.Use_Cases;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Infrastructure.Persistence;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Application.Query_Handlers
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
    {
        private readonly CustomerDbContext _context;

        public GetCustomerByIdQueryHandler(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            return customer;
        }
    }
}
