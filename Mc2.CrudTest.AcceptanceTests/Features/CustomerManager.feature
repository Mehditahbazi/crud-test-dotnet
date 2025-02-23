Feature: Customer Management
  As a user
  I want to manage customers
  So that I can perform CRUD operations

  Scenario: Create a new customer
    Given the following customer details
      | FirstName | LastName | DateOfBirth | PhoneNumber        | Email               | BankAccountNumber |
      | John      | Doe      | 1990-01-01  | +1-329-420-1792    | john.doe@Mail.com   | 123-456-789       |
    When I create the customer
    Then the customer should be successfully created

  Scenario: Retrieve a customer
    Given a customer exists with email "john.doe@rayankar.com"
    When I retrieve the customer by email "john.doe@rayankar.com"
    Then the customer details should be returned

  Scenario: Successfully retrieving a customer by ID
    Given a customer exists with ID 101
    When I request the customer by ID 102
    Then the response should contain the customer details

  Scenario: Successfully update a customer with a valid timestamp
    Given a customer exists with the following details:
      | FirstName | LastName | DateOfBirth | PhoneNumber | Email | BankAccountNumber |
      | John      | Doe      | 1990-01-01  | +1-329-420-1792  | john20@example.com | 123456789 |
    And I retrieve the latest timestamp for the customer
    When I update the customer with:
      | FirstName | LastName | PhoneNumber |
      | Updated   | Doe      | 9876543210  |
    Then the update should be successful

  Scenario: Fail to update a customer due to an outdated timestamp
    Given a customer exists with the following details:
      | FirstName | LastName | DateOfBirth | PhoneNumber | Email | BankAccountNumber |
      | John      | Doe      | 1990-01-01  | +1-329-420-1792  | john22@example.com | 123456789 |
    And I retrieve an outdated timestamp for the customer
    When I attempt to update the customer
    Then the update should fail due to concurrency conflict

  Scenario: Successfully delete a customer with a valid timestamp
    Given a customer exists with the following details:
      | FirstName | LastName | DateOfBirth | PhoneNumber | Email | BankAccountNumber |
      | Jane      | Doe      | 1995-05-15  | +1-329-420-1792  | jane2@example.com | 987654321 |
    And I retrieve the latest timestamp for the customer
    When I delete the customer
    Then the deletion should be successful

  Scenario: Fail to delete a customer due to an outdated timestamp
    Given a customer exists with the following details:
      | FirstName | LastName | DateOfBirth | PhoneNumber | Email | BankAccountNumber |
      | Jane1      | Doe1      | 1995-05-15  | +1-329-420-1792  | jane1@example.com | 987654321 |
    And I retrieve an outdated timestamp for the customer
    When I attempt to delete the customer
    Then the deletion should fail due to concurrency conflict

  Scenario: Create a customer with an invalid phone number
    Given I have a new customer with an invalid phone number
    When I create the customer
    Then the response should contain an error message "Invalid mobile phone number"

  Scenario: Create a duplicate customer
    Given I have an existing customer
    And I try to create another customer with the same FirstName, LastName, and DateOfBirth
    When I create the customer
    Then the response should contain an error message "Customer already exists"