using CRMRepository;
using CRMRepository.Entities;
using CRMRestApiV2.Controllers;
using FluentAssertions;
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


        #region functional api acceptance tests

        /// <summary>
        /// Clear the datasource before each Scenario 
        /// </summary>
        [Background]
        public static void Setup()
        {
            var customerRepo = new CustomerRepository();
            var actualData = customerRepo.FetchAll().ToList();
            
            if(actualData != null)
            {
                customerRepo.DeleteRange(actualData);
            }
        }
        
        
        [Scenario]
        [Example("JD1", "John", "Doe",21)]
        [Example("JD2", "Jane", "Doe",20)]
        public void PostCustomersJohnAndJaneToCRM(string id, string firstName, string lastName,int age, CRMCustomerController controller)
        {
            controller = new CRMCustomerController(null);
            Customer actual = null;
            Customer expected = new() { Id = id, FirstName = firstName, LastName = lastName, Age = age };
            "Given we have a these new customers to add to the CRM"
                .x(() =>
                {
                    actual = new() { Id = id, FirstName = firstName, LastName = lastName, Age = age };
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
        [Example("JD1", "John", "Doe", 21)]
        public void PostCustomersJohnShouldUpdateJohn(string id, string firstName, string lastName, int age, CRMCustomerController controller)
        {
            controller = new CRMCustomerController(null);
            Customer actual = null;
            "Given we have a this existing customer that is already added to the CRM"
                .x(() =>
                {
                    actual = new() { Id = id, FirstName = firstName, LastName = lastName, Age = age };
                    controller.Post(actual);
                });

            "When these customers are posted"
                .x(() =>
                {
                    actual.Age = 22;
                    controller.Post(actual);
                });


            "Then these customer are added and saved"
                .x(() =>
                {

                    var expected = controller.Get(actual.Id);

                    actual.Should().BeEquivalentTo(expected);

                })
                .Teardown(() =>
                {
                    controller.DeleteById(actual.Id);
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
                                 new() { Id = "JD1", FirstName = "John", LastName = "Doe", Age = 21 },
                                 new()  { Id = "JD2", FirstName = "Jane", LastName = "Doe" , Age = 20}
                            };
                     });

            "When these customers are posted"
                .x(() =>
                {
                    controller.Post(actual[0]);
                    controller.Post(actual[1]);
                    expected = controller.GetAll();
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
            Customer expected = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" , Age = 21};
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
        public void TryDeleteNonExistingCustomersFromCRMShouldReturnHTTPNOTFOUND(CRMCustomerController controller, string nonExistingId, CustomerRepository customerRepo)
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
