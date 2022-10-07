using CRMRepository;
using CRMRepository.Entities;
using CRMRestApiV2.Controllers;
using FluentAssertions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using Xbehave;

namespace SimpleCRM
{
    [ExcludeFromCodeCoverage]
    public class CustomerInteractionsIntegrationsTest
    {

        #region functional api acceptance tests
        [Scenario]
        [Example("JD1", "John", "Doe")]
        [Example("JD2", "Jane", "Doe")]
        public void PostCustomersJohnAndJaneToCRM(string id, string firstName, string lastName, CRMCustomerController controller, CustomerRepository customerRepo)
        {
            controller = new CRMCustomerController(null);
            Customer actual = null;
            Customer expected = new() { Id = id, FirstName = firstName, LastName = lastName };
            "Given we have a these new customers to add to the CRM"
                .x(() =>
                {
                    actual = new() { Id = id, FirstName = firstName, LastName = lastName };
                });

            "When these customers are posted"
                .x(() =>
                {
                    controller.Post(actual);
                });


            "Then these customer are added and saved"
                .x(() =>
                {

                    var stored = controller.Get(actual.Id);

                    actual.Should().BeEquivalentTo(expected);

                })
                .Teardown(() =>
                 {
                     controller.DeleteById(actual.Id);
                     controller = null;
                 });

        }

        [Scenario]
        public void GetAllCustomersFromCRM(CRMCustomerController controller, CustomerRepository customerRepo)
        {
            controller = new CRMCustomerController(null);
            List<Customer> expected = new()
            {
                new() { Id = "JD1", FirstName = "John", LastName = "Doe" },
                new()  { Id = "JD2", FirstName = "Jane", LastName = "Doe" }
            };

            List<Customer> actual = null;


            "Given we have a these new customers to add to the CRM"
                     .x(() =>
                     {
                         actual = new()
                            {
                                new() { Id = "JD1", FirstName = "John", LastName = "Doe" },
                                new()  { Id = "JD2", FirstName = "Jane", LastName = "Doe" }
                            };
                     });

            "When these customers are posted"
                .x(() =>
                {
                    controller.Post(actual[0]);
                    controller.Post(actual[1]);
                });
        


            "Then these customer are added and saved"
                .x(() =>
                {

                    actual.Should().BeEquivalentTo(expected);

                })
                .Teardown(() =>
                {
                    controller.DeleteRange("JD1,JD2");
                    controller = null;
                });

        }

        [Scenario]
        public void GetCustomersJohnFromCRM(CRMCustomerController controller, Customer John, CustomerRepository customerRepo)
        {
            Customer actual = null;
            Customer expected = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" };
            controller = new(null);

            "Given we have John Doe a new customer that has been added to the CRM"
                .x(() =>
                {
                    controller.Post(expected);
                });

            "When the customer John Doe is requested"
                .x(() =>
                {
                    actual = controller.Get(expected.Id);
                });


            "Then the customer John Doe is returned"
                .x(() =>
                {
                    actual.Should().BeEquivalentTo(expected);

                }).Teardown(() =>
                {
                    controller.Delete(actual);
                    controller = null;
                });
           
        }

        [Scenario]
        public void TryDeleteNonExistingCustomersFromCRMShouldReturnHTTPNOTFOUND(CRMCustomerController controller, string nonExistingId ,CustomerRepository customerRepo)
        {
            HttpStatusCode actual = HttpStatusCode.Unused;
            HttpStatusCode expected = HttpStatusCode.NotFound;

            nonExistingId = "NOTHERE";
            controller = new(null);

            "When the non exiting customer BadId is requested to be deleted"
                .x(() =>
                {
                    actual = controller.DeleteById(nonExistingId);
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

        #region technical data infrastucture tests

        [Scenario]
        public void TryDeleteNullCustomersFromCRMShouldReturnHTTPNOTFOUND(CRMCustomerController controller, string nonExistingIds, CustomerRepository customerRepo)
        {
            HttpStatusCode actual = HttpStatusCode.Unused;
            HttpStatusCode expected = HttpStatusCode.BadRequest;

            nonExistingIds = null;
            controller = new(null);

            "When the non exiting customer BadId is requested to be deleted"
                .x(() =>
                {
                    actual = controller.DeleteRange(nonExistingIds);
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
