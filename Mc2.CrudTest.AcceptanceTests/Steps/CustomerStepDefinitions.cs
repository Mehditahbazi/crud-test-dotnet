using FluentAssertions;
using Mc2.CrudTest.AcceptenceTests.Drivers;
using System.Net;
using System.Text.Json;
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
                BankAccountNumber = row["BankAccountNumber"],
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

        [Given(@"a customer exists with ID (.*)")]
        public async Task GivenACustomerExistsWithID(int id)
        {
            _response = await _driver.GetCustomerById(id);
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [When(@"I request the customer by ID (.*)")]
        public async Task WhenIRequestTheCustomerByID(int id)
        {
            _response = await _driver.GetCustomerById(id);
        }

        [Then(@"the response should contain the customer details")]
        public async Task ThenTheResponseShouldContainTheCustomerDetails()
        {
            var content = await _response.Content.ReadAsStringAsync();
            var customer = JsonSerializer.Deserialize<dynamic>(content);
            Assert.NotNull(customer);
        }

        [Given(@"a customer exists with the following details:")]
        public async Task GivenACustomerExistsWithTheFollowingDetails(Table table)
        {
            var customer = new
            {
                FirstName = table.Rows[0]["FirstName"],
                LastName = table.Rows[0]["LastName"],
                DateOfBirth = table.Rows[0]["DateOfBirth"],
                PhoneNumber = table.Rows[0]["PhoneNumber"],
                Email = table.Rows[0]["Email"],
                BankAccountNumber = table.Rows[0]["BankAccountNumber"]
            };

            await _driver.CreateCustomerAsync(customer);
        }

        [Given(@"I retrieve the latest timestamp for the customer")]
        public async Task GivenIRetrieveTheLatestTimestampForTheCustomer()
        {
            await _driver.RetrieveLatestTimestampAsync();
        }

        [Given(@"I retrieve an outdated timestamp for the customer")]
        public async Task GivenIRetrieveAnOutdatedTimestampForTheCustomer()
        {
            await _driver.RetrieveOutdatedTimestampAsync();
        }

        [When(@"I update the customer with:")]
        public async Task WhenIUpdateTheCustomerWith(Table table)
        {
            var updateData = new
            {
                FirstName = table.Rows[0]["FirstName"],
                LastName = table.Rows[0]["LastName"],
                PhoneNumber = table.Rows[0]["PhoneNumber"]
            };

            await _driver.UpdateCustomerAsync(updateData);
        }

        [When(@"I attempt to update the customer")]
        public async Task WhenIAttemptToUpdateTheCustomer()
        {
            await _driver.UpdateCustomerAsync(new { FirstName = "NewName" });
        }

        [When(@"I delete the customer")]
        public async Task WhenIDeleteTheCustomer()
        {
            await _driver.DeleteCustomerAsync();
        }

        [When(@"I attempt to delete the customer")]
        public async Task WhenIAttemptToDeleteTheCustomer()
        {
            await _driver.DeleteCustomerAsync();
        }

        [Then(@"the update should be successful")]
        public void ThenTheUpdateShouldBeSuccessful()
        {
            _driver.AssertSuccess();
        }

        [Then(@"the update should fail due to concurrency conflict")]
        public void ThenTheUpdateShouldFailDueToConcurrencyConflict()
        {
            _driver.AssertConflict();
        }

        [Then(@"the deletion should be successful")]
        public void ThenTheDeletionShouldBeSuccessful()
        {
            _driver.AssertSuccess();
        }

        [Then(@"the deletion should fail due to concurrency conflict")]
        public void ThenTheDeletionShouldFailDueToConcurrencyConflict()
        {
            _driver.AssertConflict();
        }
    }
}
