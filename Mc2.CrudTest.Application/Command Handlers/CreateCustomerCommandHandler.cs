using Mc2.CrudTest.Application.Use_Cases;
using Mc2.CrudTest.Application.Validators;
using Mc2.CrudTest.Infrastructure.Persistence;
using Mc2.CrudTest.Presentation.Server.Models;
using MediatR;

namespace Mc2.CrudTest.Application.Command_Handlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly CustomerDbContext _context;
        public CreateCustomerCommandHandler(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            if (!PhoneNumberValidator.IsValidMobileNumber(request.PhoneNumber))
                throw new ArgumentException("Invalid mobile phone number");

            var customer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                BankAccountNumber = request.BankAccountNumber,
                DateOfBirth = request.DateOfBirth,
                PhoneNumber = request.PhoneNumber
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);

            return customer.Id;
        }
    }
}
