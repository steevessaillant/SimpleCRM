import React from 'react'
import { mount } from '@cypress/react18'
import CustomerForm from '../../src/components/customerForm'
import dateFormat, { masks } from "dateformat";
const now = new Date();

describe('CustomerForm', () => {
    const id = '[data-cy=id]'
    const firstName = '[data-cy=firstName]'
    const lastName = '[data-cy=lastName]'
    const dob = '[data-cy=dob]'
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

    it("should post data with default values (smoke test)", () => {

        mount(
            <CustomerForm />
        )
            .then(() => {
                cy.get(id).type("CyTestId")
                    .get(firstName).type("CyTestFName")
                    .get(lastName).type("CyTestLName")
                    .get(dob).clear()
                    .get(dob).type('2000-01-01')
                    .get("form")
                    .submit()
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
                cy.get("form")
                    .submit()
                    .then((e) => {
                        cy.get(message).contains('Please fix the errors in the form')
                    })
            })

    })

    //TODO : installer momentjs pour validerr les dates
    // it("should not post data if the customer is not 18 years old", () => {

    //     var now = dateFormat(now, "isoDate");
    //     mount(
    //         <CustomerForm />
    //     )
    //         .then(() => {
    //             cy.get(id).type("CyTestId")
    //                 .get(firstName).type("CyTestFName")
    //                 .get(lastName).type("CyTestLName")
    //                 .get(dob).clear()
    //                 .get(dob).type(now)
    //                 .get("form")
    //                 .submit()
    //                 .then((e) => {
    //                     cy.get(message).contains('Please fix the errors in the form')
    //                 })
    //         })

    // })
})