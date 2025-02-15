using Mc2.CrudTest.Presentation.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Mc2.CrudTest.AcceptanceTests.Drivers
{
    public class CustomerDriver
    {

        private readonly HttpClient _httpClient;
        private HttpResponseMessage _response;
        private string _latestTimestamp;
        private int _customerId;
        public CustomerDriver()
        {
            var factory = new WebApplicationFactory<Program>();
            _httpClient = factory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7045/api/");

        }

        public async Task<HttpResponseMessage> CreateCustomerAsync(object customer)
        {
            var content = new StringContent(JsonSerializer.Serialize(customer), Encoding.UTF8, "application/json");
            _response = await _httpClient.PostAsync("customers", content);
            if (_response.StatusCode == System.Net.HttpStatusCode.OK)
                _customerId = JsonSerializer.Deserialize<int>(await _response.Content.ReadAsStringAsync());
            return _response;
        }

        public async Task<HttpResponseMessage> GetCustomerAsync(string email)
        {
            return await _httpClient.GetAsync($"customers/GetByEmail/{email}");
        }

        public async Task<HttpResponseMessage> GetCustomerById(int id)
        {
            return await _httpClient.GetAsync($"customers/{id}");
        }

        public async Task RetrieveLatestTimestampAsync()
        {
            var response = await _httpClient.GetAsync($"customers/{_customerId}");
            var json = await response.Content.ReadAsStringAsync();
            var customer = JObject.Parse(json);
            _latestTimestamp = customer["timeStamp"].ToObject<string>();
        }

        public async Task RetrieveOutdatedTimestampAsync()
        {
            _latestTimestamp = Convert.ToBase64String(new byte[] { 1, 2, 3, 4 });
        }

        public async Task UpdateCustomerAsync(object updateData)
        {
            var requestBody = updateData.GetType().GetProperties()
       .ToDictionary(prop => prop.Name, prop => prop.GetValue(updateData));

            requestBody["Id"] = _customerId;
            requestBody["TimeStamp"] = _latestTimestamp;
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _response = await _httpClient.PutAsync($"customers", content);
        }

        public async Task DeleteCustomerAsync()
        {
            var requestBody = new { id = _customerId, timestamp = _latestTimestamp };
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "customers") { Content = content });
        }

        public void AssertSuccess()
        {
            Assert.True(_response.IsSuccessStatusCode, $"Expected success but got {_response.StatusCode}");
        }

        public void AssertConflict()
        {
            Assert.Equal(System.Net.HttpStatusCode.Conflict, _response.StatusCode);
        }
    }
}
