using CRMRepository;
using CRMRepository.Entities;
using CRMRestApiV2.Controllers;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
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
                     controller.Delete(actual.Id);
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
                });
           
        }

        #endregion

        #region technical data infrastucture tests




        #endregion
    }
}
