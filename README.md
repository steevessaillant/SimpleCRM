# SimpleCRM
BDD/TDD implementation with XUnit (GWT plugin) NO Jest version 1.0.0 of web.

This is BDD / TDD double loop with full build pipeline (GitHUbActions)

![Code Coverage](https://img.shields.io/badge/Code%20Coverage-68%25-yellow?style=flat)

[![.NET](https://github.com/steevessaillant/SimpleCRM/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/steevessaillant/SimpleCRM/actions/workflows/dotnet.yml)

Test String for PR.

The work on this is in progress, some files need to be ignore by git , I need to adrees this issue ASAP.
There are End-to-End tests yet , but e CRUD logic is covered.

CRUD: Definition, Create-Read-Update-Delete

For CRUD Update is Missing for now.


The scenarios in this project are:

CRUD Scenarios
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

Business Rules Scenarios

Given we have a new customer that is 17 yrs of age
| Id    | First Name | Last Name | DateOfBirth						   |
| JD1   | John       | Doe       | DateTime.Now.AddYears(-17)          |
When it is attempted to be added to the CRM
Then the customer is not added to the CRM and an error message is returned as : [Age must be 18 or older]

Given we have a new customer with ommited required fields
| Id    | First Name | Last Name | DateOfBirth						   |
|       |            |           | DateTime.Now.AddYears(-17)          |
When it is attempted to be added to the CRM
Then the customer is not added to the CRM and the following error messages are returned as : [FirstName is required, LastName is required, DateOfBirth is required]"

##DateTime.Now.AddYears(-17)  means thats the customer is 17 yrs of age from Now.

Edit pour video








TODO : 

2. Add a React NodeJS UI and theyre Cypress Cucumber Specs.




