import React from 'react'
import { mount } from '@cypress/react18'
import CustomerForm from '../../src/components/customerForm'
describe('ComponentName.cy.js', () => {
    const id = '[data-cy=id]'
    const firstName = '[data-cy=firstName]'
    const lastName = '[data-cy=lastName]'
    const dob = '[data-cy=dob]'

    beforeEach(() => {
        cy.intercept('POST', 'http://localhost:5222/api/CRMCustomer', (request) => {
            request.reply({
                statusCode : 200
            })
        }).as("postedCustomer")
    })

    it("Customer form should mount with default values", () => {
        mount(
            <CustomerForm />
        )
        cy.get('div').contains('Add new customer')
    })
    it("Customer form should post data with default values (smoke test)", () => {

        mount(
            <CustomerForm />
        )
            .then(() => {
          
                cy.get(firstName).type("CyTestFName")
                cy.get(lastName).type("CyTestLName")
                cy.get(dob).clear()
                cy.get(dob).type('2000-01-01')
                cy.get("form")
                    .submit()
                    .then(() => {
                        cy.wait('@postedCustomer').then((interception) => {
                            expect(interception.response.statusCode).to.eq(200)
                        })
                    })
            })

    })
})