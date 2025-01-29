using Mc2.CrudTest.Application.Use_Cases;
using Mc2.CrudTest.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Application.Command_Handlers;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly CustomerDbContext _context;
    public UpdateCustomerCommandHandler(CustomerDbContext context)
    {
        _context = context;
    }
    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
             .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (customer == null)
        {
            throw new KeyNotFoundException("Customer not found.");
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


        await _context.SaveChangesAsync(cancellationToken);
    }
}
