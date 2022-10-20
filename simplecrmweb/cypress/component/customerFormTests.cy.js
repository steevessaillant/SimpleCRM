import React from 'react'
import { mount } from '@cypress/react18'
import CustomerForm from '../../src/components/customerForm'


describe('CustomerForm', () => {
    const id = '[data-cy=id]'
    const firstName = '[data-cy=firstName]'
    const lastName = '[data-cy=lastName]'
    const dateOfBirth = '[name=dateOfBirth]'
    const form = '[data-cy=form]'
    const submit = '[data-cy=submit]'
    const message = '[data-cy=message]'

    beforeEach(() => {
        cy.intercept('POST', 'http://localhost:5222/api/CRMCustomer', (request) => {
            request.reply({
                statusCode: 200
            })
        }).as("postedCustomer")
    })

    it("should mount with default values", () => {
        mount(
            <CustomerForm />
        )
        cy.get('div').contains('Add new customer')
    })

    it("should post data with valid values (smoke test)", () => {

        mount(
            <CustomerForm />
        )
            .then(() => {
                cy.get(id).type("CyTestId")
                    .get(firstName).type("CyTestFName")
                    .get(lastName).type("CyTestLName")
                    .get(dateOfBirth).clear()
                    .get('.react-datepicker__day.react-datepicker__day--009').click()
                    .get(form)
                    .get(submit).click()
                    .then(() => {
                        cy.wait('@postedCustomer').then((interception) => {
                            expect(interception.response.statusCode).to.eq(200)
                        })
                    })
            })

     })

    it("should not post data with empty fields", () => {

        mount(
            <CustomerForm />
        )
            .then(() => {
                cy.get(form)
                    .get(submit).click()
                    .then((e) => {
                        cy.get(message).contains('Please fix the errors in the form')
                    })
            })

    })

})