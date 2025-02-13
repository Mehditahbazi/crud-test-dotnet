using Mc2.CrudTest.Application.Use_Cases;
using Mc2.CrudTest.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Application.Command_Handlers;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
{
    private readonly ICustomerRepository customerRepository;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        this.customerRepository = customerRepository;
    }

    public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(request.Id);

        if (customer == null)
        {
            throw new InvalidOperationException("Customer not found.");
        }

        var ts = Convert.FromBase64String(request.TimeStamp);
        if (!customer.TimeStamp.SequenceEqual(ts))
        {
            throw new DbUpdateConcurrencyException("The record has been modified by another process.");
        }

        var requestProperties = request.GetType().GetProperties();

        if (request.FirstName != null)
            customer.FirstName = request.FirstName;

        if (request.LastName != null)
            customer.LastName = request.LastName;

        if (request.Email != null)
            request.Email = request.Email;

        if (request.PhoneNumber != null)
            customer.PhoneNumber = request.PhoneNumber;

        if (request.DateOfBirth != null)
            customer.DateOfBirth = request.DateOfBirth ?? DateTime.MinValue;

        if (request.BankAccountNumber != null)
            customer.BankAccountNumber = request.BankAccountNumber;


        await customerRepository.UpdateAsync(customer, ts);
        return true;
    }
}
