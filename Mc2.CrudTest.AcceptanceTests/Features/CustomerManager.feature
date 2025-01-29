Feature: Customer Management
  As a user
  I want to manage customers
  So that I can perform CRUD operations

  Scenario: Create a new customer
    Given the following customer details
      | FirstName | LastName | DateOfBirth | PhoneNumber   | Email               | BankAccountNumber |
      | John      | Doe      | 1990-01-01  | 1234567890    | john.doe@test.com   | 123-456-789       |
    When I create the customer
    Then the customer should be successfully created

  Scenario: Retrieve a customer
    Given a customer exists with email "john.doe@test.com"
    When I retrieve the customer by email "john.doe@test.com"
    Then the customer details should be returned
