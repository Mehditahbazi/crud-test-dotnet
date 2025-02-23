using Mc2.CrudTest.Application.Use_Cases;
using Mc2.CrudTest.Application.Validators;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.Interfaces;
using MediatR;

namespace Mc2.CrudTest.Application.Command_Handlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly ICustomerRepository customerRepository;
        public CreateCustomerCommandHandler(ICustomerRepository _customerRepository)
        {
            customerRepository = _customerRepository;
        }

        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {

            //if (CustomerValidator.IsDuplicateCustomer(await customerRepository.GetAllAsync(), request.FirstName, request.LastName, request.DateOfBirth))
            //    throw new ArgumentException("Customer already exists");

            if (!PhoneNumberValidator.IsValidMobileNumber(request.PhoneNumber))
                throw new ArgumentException("Invalid mobile phone number");

            if (await customerRepository.ExistsAsync(request.FirstName, request.LastName, request.DateOfBirth, request.Email))
            {
                throw new InvalidOperationException("Customer already exists.");
            }

            var customer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                BankAccountNumber = request.BankAccountNumber,
                DateOfBirth = request.DateOfBirth,
                PhoneNumber = request.PhoneNumber
            };

            await customerRepository.AddAsync(customer);

            return customer.Id;
        }
    }
}
