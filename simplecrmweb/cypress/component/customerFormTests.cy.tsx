import React from 'react'
import { mount } from '@cypress/react18'
// @ts-ignore:next-line
import { CustomerForm } from '../../src/components/CustomerForm.tsx'
import moment from "moment"
import { getActiveElement } from 'formik'


describe('CustomerForm', () => {
    const id = '[data-cy=id]'
    const firstName = '[data-cy=firstName]'
    const lastName = '[data-cy=lastName]'
    const dateOfBirth = '[name=dateOfBirth]'
    const form = '[data-cy=form]'
    const submit = '[data-cy=submit]'
    const alphaNumericOnlyErrorMessage = 'must be alpha-numeric';
    const alphaOnlyErrorMessage = 'must be alpha';
    const mustBeAnAdultErrorMessage = 'Must be 18 years of age';
    const requiredErrorMessage = 'Required';


    beforeEach(() => {
        cy.intercept('POST', 'http://localhost:5222/api/CRMCustomer', (request) => {
            const yearsAgo = moment().diff(request.body.dateOfBirth, 'years', true); //with presion = true like 1.01
            const minimumAge = 18;
            let isOK = false
            yearsAgo < minimumAge ? isOK = false : isOK = true;
            if (isOK === true) {
                request.reply({
                    statusCode: 200
                })
            } else {
                request.reply({
                    statusCode: 500,
                    error: "Customer must be 18 years of age"
                })
            }

        }).as("postedCustomer")
    })

    it("should mount with default values", () => {
        mount(<CustomerForm />)
        cy.get(submit).contains('Create / Update Customer')
    })

    it("should post data with valid values (smoke test)", () => {
        const now = new Date()
        const nowString = now.toISOString().split('T')[0];
        const yearsAgo = moment().diff(nowString, 'years', true); //with precision = true like 1.01
        const minimumAge = 18;
        mount(
            <CustomerForm />
        )
            .then(() => {
                cy.get(id).type("CyTestId")
                    .get(firstName).type("CyTestFName")
                    .get(lastName).type("CyTestLName")
                    .get(dateOfBirth)
                    .clear()
                    .type(moment().subtract(19, 'year').format('YYYY-MM-DD'))
                    .get("[data-cy='submit']")
                    .click()
                    .then(() => {
                        cy.wait('@postedCustomer').then((interception) => {
                            if (interception.response !== undefined)
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
                    .get("[data-cy='submit']")
                    .click()
                    .then(() => {
                        cy.get('[data-cy="errorForId"]').contains(requiredErrorMessage);
                        cy.get('[data-cy="errorForFirstName"]').contains(requiredErrorMessage);
                        cy.get('[data-cy="errorForLastName"]').contains(requiredErrorMessage);
                        cy.get('[data-cy="errorForDateOfBirth"]').contains(requiredErrorMessage);
                    })

            })
    })

    it("should not post data with a customer agednunder 18 and invalid strings for names", () => {

        mount(
            <CustomerForm />
        )
            .then(() => {
                cy.get(id).type("_")
                    .get(firstName).type("1")
                    .get(lastName).type("1")
                    .get(dateOfBirth)
                    .clear()
                    .type(moment().format('YYYY-MM-DD'))
                    .get("[data-cy='submit']")
                    .click()
                    .then(() => {
                        cy.get('[data-cy="errorForId"]').contains(alphaNumericOnlyErrorMessage);
                        cy.get('[data-cy="errorForFirstName"]').contains(alphaOnlyErrorMessage);
                        cy.get('[data-cy="errorForLastName"]').contains(alphaOnlyErrorMessage);
                        cy.get('[data-cy="errorForDateOfBirth"]').contains(mustBeAnAdultErrorMessage);
                    })
            })

    })
})