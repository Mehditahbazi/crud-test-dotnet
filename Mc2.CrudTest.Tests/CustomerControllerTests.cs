using Mc2.CrudTest.Application.Use_Cases;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.Interfaces;
using Mc2.CrudTest.Presentation.Server.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Mc2.CrudTest.Tests
{
    public class CustomerControllerTests
    {
        private readonly CustomersController _controller;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;

        public CustomerControllerTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _mediatorMock = new Mock<IMediator>();

            _controller = new CustomersController(_mediatorMock.Object);
        }

        [Fact]
        public async Task CreateCustomer_DuplicateCustomer_ReturnsBadRequest()
        {
            var customerCommand = new CreateCustomerCommand
            (
                "John",
                "Doe",
                new DateTime(1980, 1, 1),
                "+1234567891",
                "john.doe@test.com",
                "87654321"
            );

            _customerRepositoryMock
                .Setup(repo => repo.ExistsAsync("John", "Doe", new DateTime(1980, 1, 1)))
                .ReturnsAsync(true);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), default))
                .ThrowsAsync(new ArgumentException("Customer already exists."));

            var result = await _controller.CreateCustomerAsync(customerCommand);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Customer already exists.", badRequestResult.Value);

            _customerRepositoryMock
                .Setup(repo => repo.ExistsAsync(
                    It.Is<string>(fn => fn == "John"),
                    It.Is<string>(ln => ln == "Doe"),
                    It.Is<DateTime>(dob => dob.Date == new DateTime(1980, 1, 1).Date)
                ))
                .ReturnsAsync(true);
        }

        [Fact]
        public async Task CreateCustomer_ValidCustomer_ReturnsCreatedAtAction()
        {
            var customerCommand = new CreateCustomerCommand
            (
                "John",
                "Doe",
                new DateTime(1980, 1, 1),
                "+1234567890",
                "john.doe@test.com",
                "12345678"
            );


            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), default))
                .ReturnsAsync(0);

            var result = await _controller.CreateCustomerAsync(customerCommand);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(0, createdAtActionResult.Value);
        }

        [Fact]
        public async Task GetCustomer_ExistingCustomerId_ReturnsOkObjectResult()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1980, 1, 1),
                PhoneNumber = "+1234567890",
                Email = "john.doe@test.com",
                BankAccountNumber = "12345678"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetCustomerByIdQuery>(), default))
                .ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomerByIdAsync(0);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomer = Assert.IsType<Customer>(okObjectResult.Value);
            Assert.Equal(customer.Id, returnedCustomer.Id);
        }

        [Fact]
        public async Task DeleteCustomer_ExistingCustomerId_ReturnsNoContentResult()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var deleteCommand = new DeleteCustomerCommand { Id = 1 };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteCustomerCommand>(), default))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteCustomerAsync(deleteCommand);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
