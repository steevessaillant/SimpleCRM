# SimpleCRM
BDD/TDD implementation with XUnit (GWT plugin)

This is BDD / TDD double loop with full build pipeline (GitHUbActions)

[![codecov](https://codecov.io/gh/steevessaillant/SimpleCRM/branch/main/graph/badge.svg?token=W8ZU6MV7D2)](https://codecov.io/gh/steevessaillant/SimpleCRM)
![](https://raw.githubusercontent.com/steevessaillant/SimpleCRM/main/badge-simple-crm-main.svg)

Test String for PR.

The work on this is in progress, some files need to be ignore by git , I need to adrees this issue ASAP.
There are End-to-End tests yet , but e CRUD logic is covered.

CRUD: Definition, Create-Read-Update-Delete

For CRUD Update is Missing for now.


The scenarios in this project are:

Given we have a these new customers to add to the CRM
| Id    | First Name | Last Name | 
| JD1   | John       | Doe       |
| JD2   | Jane       | Doe       |
When these customers are posted
Then these customer are added and saved

Given we have a new customer
| Id    | First Name | Last Name |
| JD1   | John       | Doe       |
When this customer is added
Then the customer is added

Given we have an existing customer
| Id    | First Name | Last Name |
| JD1   | John       | Doe       |
When this customer is deleted
Then the customer is deleted

Given we have non-existing customer
| Id    | First Name | Last Name |
| JD2   | Jane       | Doe       |
When this customer is attempted to be deleted
Then the non-existing customer cannot be deleted

When we have a new customer that is 17 yrs of age, it cannot be instanciated thus not added
| Id    | First Name | Last Name | Age |
| JD1   | John       | Doe       | 17  |







TODO : 
1. Add real CRM business logic (other that REST CRUD stuff)
2. Add a React NodeJS UI and theyre Cypress Cucumber Specs.

