using Mc2.CrudTest.Application.Use_Cases;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Application.Query_Handlers
{
    public class GetCustomerByEmailQueryHandler : IRequestHandler<GetCustomerByEmailQuery, Customer>
    {
        private readonly CustomerDbContext _context;

        public GetCustomerByEmailQueryHandler(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email == request.email, cancellationToken);

            return customer;
        }
    }
}
