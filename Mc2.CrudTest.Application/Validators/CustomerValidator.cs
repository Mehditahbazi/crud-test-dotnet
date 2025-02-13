using Mc2.CrudTest.Domain.Entities;

namespace Mc2.CrudTest.Application.Validators;

public static class CustomerValidator
{
    public static bool IsDuplicateCustomer(List<Customer> customers, string firstName, string lastName, DateTime dateOfBirth)
    {
        return customers != null && customers.Any(c => c.FirstName == firstName && c.LastName == lastName && c.DateOfBirth == dateOfBirth);
    }
}