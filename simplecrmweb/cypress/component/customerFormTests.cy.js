import React from 'react'
import { mount } from '@cypress/react18'
import CustomerForm from '../../src/components/customerForm'
describe('ComponentName.cy.js', () => {
  const id = '[data-cy=id]'
  const firstName = '[data-cy=firstName]'
  const lastName = '[data-cy=lastName]'
  const age = '[data-cy=age]'

  it("Customer form should mount with default values", () => {
    mount(
      <CustomerForm minimumAge={18} />
    )
    cy.get('div').contains('Add new customer')
    cy.get(age).should('have.value', '18')
  })

  it("Customer form should post data with default values (smoke test)", () => {
    mount(
      <CustomerForm minimumAge={18} />
    )
    .then(() => {
      cy.get(id).type("CyTestId")
      cy.get(firstName).type("CyTestFName")
      cy.get(lastName).type("CyTestLName")
      cy.get(age).clear()
      cy.get(age).type(44)
      cy.get("form")
        .submit()
    
      
    })
   
    
  })
})