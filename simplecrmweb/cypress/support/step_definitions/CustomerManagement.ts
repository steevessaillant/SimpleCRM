import  {Given, Then, When, After}  from "@badeball/cypress-cucumber-preprocessor";

let actualCustomer: { Id: string; FirstName: string; LastName: string; DateOfBirth: string; }
let expectedCustomer: { Id: string; FirstName: string; LastName: string; DateOfBirth: string; }
let id = '[data-testid=id]'
let firstName = '[data-testid=firstName]'
let lastName = '[data-testid=lastName]'
let dateOfBirth = '[name=dateOfBirth]'
let form = '[data-testid=form]'
let submit = '[data-testid=submit]'
let message = '[data-testid=message]'

Given("I want to add a customer", (customers: any) => {
  customers.hashes().forEach((row: any) => {
    actualCustomer = row
  })
})

When("I add the customer", () => {
  cy.visit("/")
    .then(() => {
      cy.get(id).type(actualCustomer.Id)
        .get(firstName).type(actualCustomer.FirstName)
        .get(lastName).type(actualCustomer.LastName)
        .get(dateOfBirth).clear()
        .get(dateOfBirth).type(actualCustomer.DateOfBirth)
        .get(form)
        .get(submit)
        .click()
        .then(() => {
          debugger;
        })

    })
})

Then("the customer should be added as", (customers: any) => {
  customers.hashes().forEach((row: any) => {
    expectedCustomer = row
  });
  cy.request({
    url: 'http://localhost:5000/api/CRMCustomer/' + actualCustomer.Id
  }).then((response) => {
    expect(response.body.dateOfBirth).to.contain(expectedCustomer.DateOfBirth)
  })

})

Then("the customer should not be added and an error saying {string} is displayed", (errorMessage: string) => {
  cy.get('[data-testid="errorForDateOfBirth"]').contains(errorMessage);
})

After(() =>{
  cy.request({
    method: 'DELETE',
    url: 'http://localhost:5000/api/CRMCustomer/' + actualCustomer.Id
  })
})
