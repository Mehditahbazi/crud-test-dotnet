using FluentAssertions;
using Mc2.CrudTest.Application.Command_Handlers;
using Mc2.CrudTest.Application.Use_Cases;
using Mc2.CrudTest.Domain.Interfaces;
using Moq;
using Xunit;

namespace Mc2.CrudTest.Tests;

public class DeleteCustomerCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly DeleteCustomerCommandHandler _handler;

    public DeleteCustomerCommandHandlerTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _handler = new DeleteCustomerCommandHandler(_customerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Delete_Customer_Successfully()
    {
        var command = new DeleteCustomerCommand(2, Convert.ToBase64String(new byte[8]));

        _customerRepositoryMock.Setup(repo => repo.DeleteAsync(command.Id, Convert.FromBase64String(command.Timestamp)))
            .ReturnsAsync(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().Be(true);
        _customerRepositoryMock.Verify(repo => repo.DeleteAsync(command.Id, Convert.FromBase64String(command.Timestamp)), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Customer_Not_Found()
    {
        var command = new DeleteCustomerCommand(1, Convert.ToBase64String(new byte[8]));

        _customerRepositoryMock.Setup(repo => repo.DeleteAsync(command.Id, Convert.FromBase64String(command.Timestamp)))
            .ReturnsAsync(false);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Customer not found.");
    }
}
