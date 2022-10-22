Feature: Customer entry form
    
    @smoke
    Scenario: Customer adds himself to the CRM
        Given I want to add a customer
            | Id       | FirstName | LastName | DateOfBirth |
            | CyTestId | CyTestFN  | CyTestLN | 2000-01-01  |
        When I add the customer
        Then the customer should be added as
            | Id       | FirstName | LastName | DateOfBirth |
            | CyTestId | CyTestFN  | CyTestLN | 2000-01-01  |

    @validation
    Scenario: Customer under age of 18 tries to adds himself to the CRM 
        Given I want to add a customer
            | Id       | FirstName | LastName | DateOfBirth |
            | CyTestId | CyTestFN  | CyTestLN | 2100-01-01  |
        When I add the customer
        Then the customer should not be added and an error saying "Must be 18 years of age" is displayed
            


