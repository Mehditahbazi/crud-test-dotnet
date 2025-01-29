using Mc2.CrudTest.Presentation;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;

namespace Mc2.CrudTest.AcceptenceTests.Drivers
{
    public class CustomerDriver
    {

        private readonly HttpClient _httpClient;

        public CustomerDriver()
        {
            var factory = new WebApplicationFactory<Program>();
            _httpClient = factory.CreateClient();
            _httpClient.BaseAddress = new System.Uri("https://localhost:7045/api/");

        }

        public async Task<HttpResponseMessage> CreateCustomerAsync(object customer)
        {
            var content = new StringContent(JsonSerializer.Serialize(customer), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("customers", content);
        }

        public async Task<HttpResponseMessage> GetCustomerAsync(string email)
        {
            return await _httpClient.GetAsync($"customers/{email}");
        }

        public async Task<HttpResponseMessage> UpdateCustomerAsync(string email, object customer)
        {
            var content = new StringContent(JsonSerializer.Serialize(customer), Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync($"customers/{email}", content);
        }

        public async Task<HttpResponseMessage> DeleteCustomerAsync(string email)
        {
            return await _httpClient.DeleteAsync($"customers/{email}");
        }
    }
}
