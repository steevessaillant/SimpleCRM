Feature: Customer entry form

    Scenario: Customer adds himself to the CRM
        Given I want to add a customer
            | Id       | FirstName | LastName | DateOfBirth |
            | CyTestId | CyTestFN  | CyTestLN | 01-01-2000  |
        When I add the customer
        Then the customer should be added as
            | Id       | FirstName | LastName | DateOfBirth |
            | CyTestId | CyTestFN  | CyTestLN | 2000-01-01  |

