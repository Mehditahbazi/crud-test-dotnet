using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.Interfaces;
using Moq;
using Xunit;

namespace Mc2.CrudTest.Tests;

public class CustomerRepositoryTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;

    public CustomerRepositoryTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
    }

    [Fact]
    public async Task AddAsync_Should_Add_Customer()
    {
        var customer = new Customer
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            BankAccountNumber = "123456789",
            Email = "john.doe@email.com",
            PhoneNumber = "123456789012",
            TimeStamp = new byte[8]
        };

        _customerRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Customer>()))
            .ReturnsAsync(1)
            .Verifiable();

        await _customerRepositoryMock.Object.AddAsync(customer);

        _customerRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Customer>(c =>
            c.FirstName == "John" &&
            c.LastName == "Doe" &&
            c.DateOfBirth == new DateTime(1990, 1, 1) &&
            c.BankAccountNumber == "123456789" &&
            c.Email == "john.doe@email.com" &&
            c.PhoneNumber == "123456789012"
        )), Times.Once);
    }
}
