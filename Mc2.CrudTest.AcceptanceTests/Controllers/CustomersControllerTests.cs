using CrudTest.Presentation.Server.Controllers;
using Mc2.CrudTest.Application.Use_Cases;
using Mc2.CrudTest.Presentation.Server.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Mc2.CrudTest.AcceptanceTests.Controllers
{
    public class CustomersControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CustomersController _controller;
        public CustomersControllerTests(WebApplicationFactory<Program> factory, Mock<IMediator> mediatorMock)
        {
            _factory = factory;
            _mediatorMock = mediatorMock;
        }

        #region Create Customer Tests
        [Fact]
        public async Task CreateCustomer_ValidInput_ReturnsCreatedCustomer()
        {
            var client = _factory.CreateClient();
            var newCustomer = new Customer
            {
                FirstName = "Test",
                LastName = "Customer",
                Email = "test.customer@example.com"
            };

            var response = await client.PostAsJsonAsync("/api/customers", newCustomer);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdCustomer = await response.Content.ReadFromJsonAsync<Customer>();
            Assert.NotNull(createdCustomer);
            Assert.Equal(newCustomer.FirstName, createdCustomer.FirstName);
            Assert.Equal(newCustomer.LastName, createdCustomer.LastName);
            Assert.Equal(newCustomer.Email, createdCustomer.Email);
            Assert.NotEqual(0, createdCustomer.Id);
        }


        [Fact]
        public async Task CreateCustomer_InvalidInput_ReturnsBadRequest()
        {

            var client = _factory.CreateClient();
            var invalidCustomer = new Customer
            {

                LastName = "Customer",
                Email = "invalid email"
            };

            var response = await client.PostAsJsonAsync("/api/customers", invalidCustomer);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateCustomer_DuplicateEmail_ReturnsConflict()
        {
            var client = _factory.CreateClient();
            var customer1 = new Customer
            {
                FirstName = "Test",
                LastName = "Customer",
                Email = "duplicate@example.com"
            };
            var customer2 = new Customer
            {
                FirstName = "Test2",
                LastName = "Customer2",
                Email = "duplicate@example.com"
            };

            await client.PostAsJsonAsync("/api/customers", customer1);
            var response2 = await client.PostAsJsonAsync("/api/customers", customer2);

            Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);
        }
        #endregion

        #region Get Customer By ID Tests
        [Fact]
        public async Task GetCustomer_ExistingId_ReturnsOkResultWithCustomer()
        {
            int customerId = 1;
            var expectedCustomer = new Customer { Id = customerId, FirstName = "Test", LastName = "Customer" }; // Example Customer

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedCustomer);

            var result = await _controller.GetCustomerById(customerId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualCustomer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(expectedCustomer.Id, actualCustomer.Id);
            Assert.Equal(expectedCustomer.FirstName, actualCustomer.FirstName);
            Assert.Equal(expectedCustomer.LastName, actualCustomer.LastName);
            _mediatorMock.Verify(m => m.Send(It.Is<GetCustomerByIdQuery>(q => q.Id == customerId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCustomer_NonExistingId_ReturnsNotFoundResult()
        {
            int customerId = 1;

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Customer)null);

            var result = await _controller.GetCustomerById(customerId);

            Assert.IsType<NotFoundResult>(result);
            _mediatorMock.Verify(m => m.Send(It.Is<GetCustomerByIdQuery>(q => q.Id == customerId), It.IsAny<CancellationToken>()), Times.Once);

        }
    }
    #endregion
}

