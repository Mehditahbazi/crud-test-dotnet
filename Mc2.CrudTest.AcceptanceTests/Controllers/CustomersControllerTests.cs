using Mc2.CrudTest.Presentation.Server.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Mc2.CrudTest.AcceptanceTests.Controllers
{
    public class CustomersControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public CustomersControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

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
    }
}
