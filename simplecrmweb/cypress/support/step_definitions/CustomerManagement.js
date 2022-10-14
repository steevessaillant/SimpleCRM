// cypress/e2e/duckduckgo.ts
import { Given } from "@badeball/cypress-cucumber-preprocessor";

Given('I want to add a customer', () => {
    cy.request('http://localhost:5222/api/CRMCustomer')
    .should((response) => {
        expect(response.status).to.eq(200)
        console.log(response)
      })
})