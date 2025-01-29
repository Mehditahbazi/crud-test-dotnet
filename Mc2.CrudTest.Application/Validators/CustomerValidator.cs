using Mc2.CrudTest.Infrastructure.Persistence;

namespace Mc2.CrudTest.Application.Validators;

public static class CustomerValidator
{
    public static bool IsDuplicateCustomer(CustomerDbContext dbContext, string firstName, string lastName, DateTime dateOfBirth)
    {
        return dbContext.Customers.Any(c =>
            c.FirstName == firstName &&
            c.LastName == lastName &&
            c.DateOfBirth == dateOfBirth);
    }
}