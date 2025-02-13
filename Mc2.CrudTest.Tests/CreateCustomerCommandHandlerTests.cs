using FluentAssertions;
using Mc2.CrudTest.Application.Command_Handlers;
using Mc2.CrudTest.Application.Use_Cases;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.Interfaces;
using Moq;
using Xunit;

namespace Mc2.CrudTest.Tests;

public class CreateCustomerCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerCommandHandlerTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Customer_Successfully()
    {
        var command = new CreateCustomerCommand("John", "Doe", DateTime.Parse("1990-01-01"), "+14185438090", "john.doe@email.com", "123456789012");

        _customerRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Customer>()))
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().Be(0);
        _customerRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Customer_Exists()
    {
        var command = new CreateCustomerCommand("John", "Doe", DateTime.Parse("1990-01-01"), "+14185438090", "john.doe@email.com", "123456789012");

        _customerRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(true);

        await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Customer already exists.");
    }
}
