Feature: Customer Manager

 Scenario: Get all customers
    Given customers exist in the system
    When I send a GET request to the /api/customers endpoint
    Then a list of customers is returned successfully
    And the HTTP status code is 200 OK

  Scenario: Get a customer by ID
    Given a customer exists with ID 1
    When I send a GET request to the /api/customers/1 endpoint
    Then the customer details are returned successfully
    And the HTTP status code is 200 OK

  Scenario: Update an existing customer
    Given a customer exists with ID 1
    And an updated customer request
    When I send a PUT request to the /api/customers/1 endpoint
    Then the customer is updated successfully
    And the HTTP status code is 204 No Content

  Scenario: Delete an existing customer
    Given a customer exists with ID 1
    When I send a DELETE request to the /api/customers/1 endpoint
    Then the customer is deleted successfully
    And the HTTP status code is 204 No Content

  Scenario: Try to get a non-existent customer
    When I send a GET request to the /api/customers/999 endpoint
    Then the response status code is 404 Not Found

  Scenario: Try to update a non-existent customer
    Given an updated customer request
    When I send a PUT request to the /api/customers/999 endpoint
    Then the response status code is 404 Not Found

  Scenario: Try to delete a non-existent customer
    When I send a DELETE request to the /api/customers/999 endpoint
    Then the response status code is 404 Not Found
