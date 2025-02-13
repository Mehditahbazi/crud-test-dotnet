using FluentAssertions;
using Mc2.CrudTest.Application.Query_Handlers;
using Mc2.CrudTest.Application.Use_Cases;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Mc2.CrudTest.Tests;
public class GetCustomerByIdQueryHandlerTests
{
    private readonly CustomerDbContext _dbContext;
    private readonly GetCustomerByIdQueryHandler _handler;

    public GetCustomerByIdQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new CustomerDbContext(options);
        _handler = new GetCustomerByIdQueryHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ShouldReturnCustomer_WhenCustomerExists()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            PhoneNumber = "1234567890",
            Email = "john@example.com",
            BankAccountNumber = "123456789",
            TimeStamp = BitConverter.GetBytes(new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds())
        };

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync();

        var query = new GetCustomerByIdQuery(customer.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(customer.Id);
        result.FirstName.Should().Be(customer.FirstName);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenCustomerDoesNotExist()
    {

        var query = new GetCustomerByIdQuery(99); // Non-existent ID


        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }
}
