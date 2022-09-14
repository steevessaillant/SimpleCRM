using CRMRepository;
using CRMRepository.Entities;
using CRMRestApi.Controllers;
using FluentAssertions;
using Xbehave;

namespace SimpleCRM
{
    public class CustomerInteractionsIntegrationsTest
    {
        #region functional api acceptance tests
        [Scenario]
        public void PostCustomersJohnAndJaneToCRM(CRMCustomerController controller,Customer John,Customer Jane, CustomerRepository customerRepo)
        {

            "Given we have a these new customers to add to the CRM"
                .x(() =>
                {
                    customerRepo = new CustomerRepository();
                    John = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" };
                    Jane = new Customer { Id = "JD2", FirstName = "Jane", LastName = "Doe" };
                    controller = new CRMCustomerController(customerRepo);
                });

            "When these customers are posted"
                .x( () =>
                {
                    controller.Post(John);
                    controller.Post(Jane);
                });


            "Then these customer are added and saved"
                .x(() =>
                {
                    customerRepo
                    .FetchAll()
                    .Find(x => x.Id == John.Id)
                    .Should().Be(John);

                    customerRepo
                    .FetchAll()
                    .Find(x => x.Id == Jane.Id)
                    .Should().Be(Jane);
                });

        }
        #endregion
    }
}
