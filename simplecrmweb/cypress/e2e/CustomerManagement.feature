Feature: Manage our customers

  Scenario: Add a new customer
    Given I want to add a customer
    When I add the customer
      | Id  | FirstName | LastName | Age |
      | JD9 | John      | Doe      | 35  |
    Then the customer should be added as 
      | Id  | FirstName | LastName | Age |
      | JD9 | John      | Doe      | 35  |
