import React from 'react'
import { mount } from '@cypress/react18'
import CustomerForm from '../../src/components/customerForm'
describe('ComponentName.cy.js', () => {
  const age = '[data-cy=age]'
  it("Customer Fform should mount with default values", () => {
    mount(
      <CustomerForm />
    )
    cy.get('div').contains('Add a new customer')
    cy.get(age).should('have.value', '18')
  })
})