Feature: Customer Management
  As a user
  I want to manage customers
  So that I can perform CRUD operations

  Scenario: Create a new customer
    Given the following customer details
      | FirstName | LastName | DateOfBirth | PhoneNumber   | Email               | BankAccountNumber |
      | John      | Doe      | 1990-01-01  | 1234567890    | john12.doe@Mail.com   | 123-456-789       |
    When I create the customer
    Then the customer should be successfully created

  Scenario: Retrieve a customer
    Given a customer exists with email "john12.doe@yahoo.com"
    When I retrieve the customer by email "john12.doe@yahoo.com"
    Then the customer details should be returned

  Scenario: Successfully retrieving a customer by ID
    Given a customer exists with ID 2
    When I request the customer by ID 2
    Then the response should contain the customer details

  Scenario: Successfully update a customer with a valid timestamp
    Given a customer exists with the following details:
      | FirstName | LastName | DateOfBirth | PhoneNumber | Email | BankAccountNumber |
      | John      | Doe      | 1990-01-01  | 1234567890  | john20@example.com | 123456789 |
    And I retrieve the latest timestamp for the customer
    When I update the customer with:
      | FirstName | LastName | PhoneNumber |
      | Updated   | Doe      | 9876543210  |
    Then the update should be successful

  Scenario: Fail to update a customer due to an outdated timestamp
    Given a customer exists with the following details:
      | FirstName | LastName | DateOfBirth | PhoneNumber | Email | BankAccountNumber |
      | John      | Doe      | 1990-01-01  | 1234567890  | john22@example.com | 123456789 |
    And I retrieve an outdated timestamp for the customer
    When I attempt to update the customer
    Then the update should fail due to concurrency conflict

  Scenario: Successfully delete a customer with a valid timestamp
    Given a customer exists with the following details:
      | FirstName | LastName | DateOfBirth | PhoneNumber | Email | BankAccountNumber |
      | Jane      | Doe      | 1995-05-15  | 1234567890  | jane2@example.com | 987654321 |
    And I retrieve the latest timestamp for the customer
    When I delete the customer
    Then the deletion should be successful

  Scenario: Fail to delete a customer due to an outdated timestamp
    Given a customer exists with the following details:
      | FirstName | LastName | DateOfBirth | PhoneNumber | Email | BankAccountNumber |
      | Jane      | Doe      | 1995-05-15  | 1234567890  | jane3@example.com | 987654321 |
    And I retrieve an outdated timestamp for the customer
    When I attempt to delete the customer
    Then the deletion should fail due to concurrency conflict