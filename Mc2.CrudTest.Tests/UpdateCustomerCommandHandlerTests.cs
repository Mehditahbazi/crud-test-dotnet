using FluentAssertions;
using Mc2.CrudTest.Application.Command_Handlers;
using Mc2.CrudTest.Application.Use_Cases;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Mc2.CrudTest.Tests;

public class UpdateCustomerCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly UpdateCustomerCommandHandler _handler;

    public UpdateCustomerCommandHandlerTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_Customer_Successfully()
    {
        var existingCustomer = new Customer
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            PhoneNumber = "123456789",
            Email = "john.doe@email.com",
            BankAccountNumber = "123456789012",
            TimeStamp = new byte[] { 1, 2, 3 }
        };

        var command = new UpdateCustomerCommand(
            existingCustomer.Id,
            "UpdatedFirstName",
            "UpdatedLastName",
           new DateTime(1990, 1, 1),
            "987654321098",
            "updated.email@email.com",
            "60379985859367",
            Convert.ToBase64String(existingCustomer.TimeStamp)
        );

        _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(existingCustomer.Id))
            .ReturnsAsync(existingCustomer);

        _customerRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Customer>(), existingCustomer.TimeStamp))
            .ReturnsAsync(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        _customerRepositoryMock.Verify(repo => repo.GetByIdAsync(existingCustomer.Id), Times.Once);
        _customerRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Customer>(), existingCustomer.TimeStamp), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Customer_Does_Not_Exist()
    {
        var command = new UpdateCustomerCommand(
            999,
            "UpdatedFirstName",
            "UpdatedLastName",
            new DateTime(1990, 1, 1),
            "987654321098",
            "updated.email@email.com",
            "60379985859367",
            Convert.ToBase64String(new byte[8])
        );

        _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync((Customer)null);


        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);


        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Customer not found.");
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Timestamp_Mismatch()
    {
        var existingCustomer = new Customer
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            PhoneNumber = "123456789",
            Email = "john.doe@email.com",
            BankAccountNumber = "123456789012",
            TimeStamp = new byte[] { 1, 2, 3 }
        };

        var command = new UpdateCustomerCommand(
            existingCustomer.Id,
            "UpdatedFirstName",
            "UpdatedLastName",
            new DateTime(1990, 1, 1),
            "987654321098",
            "updated.email@email.com",
        "60379985859367",
            Convert.ToBase64String(new byte[] { 9, 9, 9 })
        );

        _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(existingCustomer.Id))
            .ReturnsAsync(existingCustomer);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DbUpdateConcurrencyException>().WithMessage("The record has been modified by another process.");
    }
}
