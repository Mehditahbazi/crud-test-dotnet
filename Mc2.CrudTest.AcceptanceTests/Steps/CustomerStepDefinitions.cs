using Mc2.CrudTest.AcceptenceTests.Drivers;
using Xunit;

namespace Mc2.CrudTest.AcceptenceTests.Steps
{
    [Binding]
    public class CustomerSteps
    {
        private readonly CustomerDriver _driver;
        private object _customerDetails;
        private HttpResponseMessage _response;

        public CustomerSteps()
        {
            _driver = new CustomerDriver();
        }

        [Given(@"the following customer details")]
        public void GivenTheFollowingCustomerDetails(Table table)
        {
            var row = table.Rows[0];
            _customerDetails = new
            {
                FirstName = row["FirstName"],
                LastName = row["LastName"],
                DateOfBirth = DateTime.Parse(row["DateOfBirth"]),
                PhoneNumber = row["PhoneNumber"],
                Email = row["Email"],
                BankAccountNumber = row["BankAccountNumber"]
            };
        }

        [When(@"I create the customer")]
        public async Task WhenICreateTheCustomer()
        {
            _response = await _driver.CreateCustomerAsync(_customerDetails);
        }

        [Then(@"the customer should be successfully created")]
        public void ThenTheCustomerShouldBeSuccessfullyCreated()
        {
            Assert.NotNull(_response);
            Assert.True(_response.IsSuccessStatusCode);
        }

        [Given(@"a customer exists with email ""(.*)""")]
        public async Task GivenACustomerExistsWithEmail(string email)
        {
            _customerDetails = new
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = DateTime.Parse("1990-01-01"),
                PhoneNumber = "1234567890",
                Email = email,
                BankAccountNumber = "123-456-789"
            };

            _response = await _driver.CreateCustomerAsync(_customerDetails);
            Assert.True(_response.IsSuccessStatusCode);
        }

        [When(@"I retrieve the customer by email ""(.*)""")]
        public async Task WhenIRetrieveTheCustomerByEmail(string email)
        {
            _response = await _driver.GetCustomerAsync(email);
        }

        [Then(@"the customer details should be returned")]
        public async Task ThenTheCustomerDetailsShouldBeReturned()
        {
            var content = await _response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            Assert.Contains("John", content);
        }
    }
}
