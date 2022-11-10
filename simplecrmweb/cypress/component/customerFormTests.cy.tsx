import { mount } from '@cypress/react18'
// @ts-ignore:next-line
import { CustomerForm } from '../../src/components/CustomerForm.tsx'
import * as moment from "moment"
import * as React from 'react';

let fixtureData = { 
    "id": "",
"firstName": "",
"lastName": "",
"dateOfBirth": ""
};

describe('CustomerForm', () => {
    const id = '[data-testid=id]'
    const firstName = '[data-testid=firstName]'
    const lastName = '[data-testid=lastName]'
    const dateOfBirth = '[name=dateOfBirth]'
    const form = '[data-testid=form]'
    const submit = '[data-testid=submit]'
    const alphaNumericOnlyErrorMessage = 'must be alpha-numeric';
    const alphaOnlyErrorMessage = 'must be alpha';
    const mustBeAnAdultErrorMessage = 'Must be 18 years of age';
    const requiredErrorMessage = 'Required';


    beforeEach(() => {
        cy.fixture('customer').then(function (data) {
            fixtureData = data;
          })

        cy.intercept('POST', 'http://localhost:5000/api/CRMCustomer', (request) => {
            const yearsAgo = moment().diff(request.body.dateOfBirth, 'years', true); //with precision = true like 1.01
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

    it("should post data with valid values (smoke test)", () => {;
        mount(
            <CustomerForm />
        )
            .then(() => {
                
                cy.get(id).type(fixtureData.id)
                    .get(firstName).type(fixtureData.firstName)
                    .get(lastName).type(fixtureData.lastName)
                    .get(dateOfBirth).type(fixtureData.dateOfBirth)
                    .get("[data-testid='submit']")
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
                    .get("[data-testid='submit']")
                    .click()
                    .then(() => {
                        cy.get('[data-testid="errorForId"]').contains(requiredErrorMessage);
                        cy.get('[data-testid="errorForFirstName"]').contains(requiredErrorMessage);
                        cy.get('[data-testid="errorForLastName"]').contains(requiredErrorMessage);
                        cy.get('[data-testid="errorForDateOfBirth"]').contains(requiredErrorMessage);
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
                    .type(moment().format('YYYY-MM-DD')) //this is dynamic and will last for ages, this is the actual today date at runtime
                    .get("[data-testid='submit']")
                    .click()
                    .then(() => {
                        cy.get('[data-testid="errorForId"]').contains(alphaNumericOnlyErrorMessage);
                        cy.get('[data-testid="errorForFirstName"]').contains(alphaOnlyErrorMessage);
                        cy.get('[data-testid="errorForLastName"]').contains(alphaOnlyErrorMessage);
                        cy.get('[data-testid="errorForDateOfBirth"]').contains(mustBeAnAdultErrorMessage);
                    })
            })

    })
})