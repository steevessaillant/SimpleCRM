using Castle.Core.Resource;
using CRMRepository;
using CRMRepository.Entities;
using CRMRestApiV2.Controllers;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using Xbehave;

namespace CRMTests.Integration
{
    [ExcludeFromCodeCoverage]
    public class CustomerInteractionsIntegrationsTest
    {


        /// <summary>
        /// Clear the datasource before each Scenario 
        /// </summary>
        [Background]
        public async static void Setup()
        {
            var customerRepo = new CustomerRepository();
            var actualData = await customerRepo.FetchAllAsync();

            if (actualData.ToList() != null)
            {
                await customerRepo.DeleteRangeAsync(actualData);
            }
        }


        [Scenario]
        [Example("JD1", "John", "Doe", "2000-01-01")]
        [Example("JD2", "Jane", "Doe", "1999-02-02")]
        public void PostCustomersJohnAndJaneToCRM(string id, string firstName, string lastName, string dateOfBirth, CRMCustomerController controller)
        {
            controller = new CRMCustomerController(null);
            Customer actual = null;
            Customer expected = new() { Id = id, FirstName = firstName, LastName = lastName, DateOfBirth = DateTime.Parse(dateOfBirth) };
            "Given we have a these new customers to add to the CRM"
                .x(() =>
                {
                    actual = new() { Id = id, FirstName = firstName, LastName = lastName, DateOfBirth = DateTime.Parse(dateOfBirth) };
                });

            "When these customers are posted"
                .x(async () =>
                {
                    await controller.PostAsync(actual);
                });


            "Then these customer are added and saved"
                .x(async () =>
                {

                    var stored = await controller.GetAsync(actual.Id);

                    actual.Should().BeEquivalentTo(expected);

                })
                .Teardown(async () =>
                 {
                     await controller.DeleteByIdAsync(actual.Id);
                     controller = null;
                 });

        }

        [Scenario]
        [Example("JD1", "John", "Doe", "2000-01-01")]
        public void PostCustomersJohnShouldUpdateJohn(string id, string firstName, string lastName, string dateOfBirth, CRMCustomerController controller)
        {
            controller = new CRMCustomerController(null);
            Customer actual = null;
            "Given we have a this existing customer that is already added to the CRM with age 2"
                .x(async () =>
                {
                    actual = new() { Id = id, FirstName = firstName, LastName = lastName, DateOfBirth = DateTime.Parse(dateOfBirth) };
                    await controller.PostAsync(actual);
                });

            "When this customer is posted with age 22"
                .x(async () =>
                {
                    actual.DateOfBirth = DateTime.Parse("1999-01-01");
                    await controller.PostAsync(actual);
                });


            "Then this customer is just updated and saved"
                .x(async () =>
                {

                    var expected = await controller.GetAsync(actual.Id);

                    actual.Should().BeEquivalentTo(expected);

                })
                .Teardown(async () =>
                {
                    await controller.DeleteByIdAsync(actual.Id);
                    controller = null;
                });


        }

        [Scenario]
        public void GetAllCustomersFromCRM(CRMCustomerController controller)
        {

            List<Customer> expected = null;
            List<Customer> actual = null;

            controller = new CRMCustomerController(null);


            "Given we have a these new customers to add to the CRM"
                     .x(() =>
                     {
                         actual = new()
                            {
                                 new() { Id = "JD1", FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.Parse("2000-01-01") },
                                 new()  { Id = "JD2", FirstName = "Jane", LastName = "Doe" , DateOfBirth = DateTime.Parse("1999-02-02") }
                            };
                     });

            "When these customers are posted"
                .x(async () =>
                {
                    await controller.PostAsync(actual[0]);
                    await controller.PostAsync(actual[1]);
                    expected = await controller.GetAllAsync();
                });



            "Then these customer are added and saved"
                .x(() =>
                {

                    actual.Should().BeEquivalentTo(expected);

                })
                .Teardown(async () =>
                {
                    await controller.DeleteRangeAsync("JD1,JD2");
                    controller = null;
                });

        }

        [Scenario]
        public void GetCustomersJohnFromCRM(CRMCustomerController controller, Customer John, CustomerRepository customerRepo)
        {
            Customer actual = null;
            Customer expected = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.Parse("2000-01-01") };
            controller = new(null);

            "Given we have John Doe a new customer that has been added to the CRM"
                .x(async () =>
                {
                    await controller.PostAsync(expected);
                });

            "When the customer John Doe is requested"
                .x(async () =>
                {
                    actual = await controller.GetAsync(expected.Id);
                });


            "Then the customer John Doe is returned"
                .x(() =>
                {
                    actual.Should().BeEquivalentTo(expected);

                }).Teardown(async () =>
                {
                    await controller.DeleteAsync(actual);
                    controller = null;
                });

        }

        [Scenario]
        public void TryDeleteNonExistingCustomersFromCRMShouldReturnHTTPNOTFOUND(CRMCustomerController controller, string nonExistingId, CustomerRepository customerRepo)
        {
            HttpStatusCode actual = HttpStatusCode.Unused;
            HttpStatusCode expected = HttpStatusCode.NotFound;

            nonExistingId = "NOTHERE";
            controller = new(null);

            "When the non exiting customer BadId is requested to be deleted"
                .x(async () =>
                {
                    actual = await controller.DeleteByIdAsync(nonExistingId);
                });


            "Then the customer John Doe is returned"
                .x(() =>
                {
                    actual.Should().Be(expected);

                }).Teardown(() =>
                {
                    controller = null;
                });

        }

        # region functional tests for the Customer business rules
        [Scenario]
        public void PostANonAdultCustomerMustReturnError(CRMCustomerController controller, Customer actual, ValidationException validationException)
        {

            controller = new(null);

            "Given we have John Doe a new customer that is 0 years of age"
                .x(() =>
                {
                    actual = new() { Id = "JD1", FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.Now }; ;
                });

            "When the customer John Doe is posted"
                .x(async () =>
                {
                    try
                    {
                        await controller.PostAsync(new Customer { Id = "JD1", FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.Now});
                    }
                    catch (ValidationException ex)
                    {
                        validationException = ex;
                    }
                });
            "Then the validation fails and returns the following message"
                .x(() =>
                {
                    validationException.Errors.First().ErrorMessage.Should().Be("Age must be 18 or older");
                });

        }

        [Scenario]
        public void PostAnAdultCustomerButOtherRequiredFieldsAreEmptyMustReturnErrors(CRMCustomerController controller, Customer actual, List<ValidationFailure> validationFailures)
        {

            controller = new(null);

            "Given we have John Doe a new customer that is 18 years of age"
                .x(() =>
                {
                    actual = new() { Id = string.Empty, FirstName = string.Empty, LastName = string.Empty, DateOfBirth = DateTime.Now.AddYears(-18) }; ;
                    validationFailures = new List<ValidationFailure>();
                });

            "When the customer John Doe is posted"
                .x(async () =>
                {
                    try
                    {
                        await controller.PostAsync(actual);
                    }
                    catch (ValidationException ex)
                    {
                        ex.Errors.ToList().ForEach(x => validationFailures.Add(new ValidationFailure(x.PropertyName, x.ErrorMessage)));
                    }

                });
            "Then the validation fails and returns the following message"
                .x(() =>
                {
                    validationFailures.ToList().ForEach(failure => failure.ErrorMessage.Contains("is required").Should().BeTrue());
                });

        }


        #endregion

        #region technical data infrastucture tests

        [Scenario]
        public void TryDeleteNullCustomersFromCRMShouldReturnHTTPNOTFOUND(CRMCustomerController controller, string nonExistingIds, CustomerRepository customerRepo)
        {
            HttpStatusCode actual = HttpStatusCode.Unused;
            HttpStatusCode expected = HttpStatusCode.BadRequest;

            nonExistingIds = null;
            controller = new(null);

            "When the non exiting customer BadId is requested to be deleted"
                .x(async () =>
                {
                    actual = await controller.DeleteRangeAsync(nonExistingIds);
                });


            "Then the customer John Doe is returned"
                .x(() =>
                {
                    actual.Should().Be(expected);

                }).Teardown(() =>
                {
                    controller = null;
                });

        }

       

        #endregion
    }
}
