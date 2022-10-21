import { Given, Then, When } from "@badeball/cypress-cucumber-preprocessor";

let actualCustomer = null
let expectedCustomer = null
let id = '[data-cy=id]'
let firstName = '[data-cy=firstName]'
let lastName = '[data-cy=lastName]'
let dateOfBirth = '[name=dateOfBirth]'
let form = '[data-cy=form]'
let submit = '[data-cy=submit]'
let message = '[data-cy=message]'

Given("I want to add a customer", (customers) => {
    customers.hashes().forEach((row) => {
      actualCustomer = row
     })
  })
  
  When("I add the customer", () => {
    cy.visit("/" )
    .then(() => {
      cy.get(id).type(actualCustomer.Id)
          .get(firstName).type(actualCustomer.FirstName)
          .get(lastName).type(actualCustomer.LastName)
          .get(dateOfBirth).clear()
          .get(dateOfBirth).type(actualCustomer.DateOfBirth)
          .get(form)
          .get(submit)
          .click()
  
    })
  })
  
  Then("the customer should be added as", (customers) => {
    customers.hashes().forEach((row) => {
      expectedCustomer = row
    });
    cy.request({
      url: 'http://localhost:5222/api/CRMCustomer/' + expectedCustomer.Id
     }).then((response) => {
      expect(response.body.dateOfBirth).to.contain(expectedCustomer.DateOfBirth)
     })
    
})
