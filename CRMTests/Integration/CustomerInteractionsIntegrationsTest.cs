using CRMRepository;
using CRMRepository.Entities;
using CRMRestApiV2.Controllers;
using FluentAssertions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xbehave;
using Xunit;

namespace SimpleCRM
{
    [ExcludeFromCodeCoverage]
    public class CustomerInteractionsIntegrationsTest
    {
        private readonly CRMCustomerController controller = new CRMCustomerController(null);

        #region functional api acceptance tests
        [Scenario]
        public void PostCustomersJohnAndJaneToCRM(CRMCustomerController controller,Customer John,Customer Jane, CustomerRepository customerRepo)
        {

            controller = this.controller;            

            "Given we have a these new customers to add to the CRM"
                .x(() =>
                {
                    John = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" };
                    Jane = new Customer { Id = "JD2", FirstName = "Jane", LastName = "Doe" };
                    
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
                    controller.Get()
                    .Should()
                    .Contain(John);

                    controller.Get()
                    .Should()
                    .Contain(Jane); ;
                });

            //cleanup
            controller.Delete(John);
            controller.Delete(Jane);

        }

        [Scenario]
        public void GetCustomersJohnFromCRM(CRMCustomerController controller, Customer John, CustomerRepository customerRepo)
        {

            controller = this.controller;

            "Given we have John Doe a new customer that has been added to the CRM"
                .x(() =>
                {
                    John = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" };
                    controller.Post(John);
                });

            "When the customer John Doe is requested"
                .x(() =>
                {
                    controller.Get(John.Id);
                });


            "Then the customer John Doe is returned"
                .x(() =>
                {
                    controller.Get()
                    .Should()
                    .Contain(John);
                });

            //cleanup
            controller.Delete(John);

        }
        #endregion

        #region technical data infrastucture tests

        private readonly string dataFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Customers.json";

        [Fact]
        public void MemoryToFileIOShouldPersist()
        {
            var customerRepo = this.controller.Repository;
            List<Customer> tempDataStore = new();
            tempDataStore.ExportToTextFile(customerRepo.Path);

            File.Exists(customerRepo.Path).Should().BeTrue();

        }

        [Fact]
        public void ImportFromTextFileShouldReturnNotNUllEmptyListList()
        {
            var customerRepo = this.controller.Repository;
            Customer testCustomer = new() { Id = "TEST", FirstName = "Tester", LastName = "Testing" };
            List<Customer> actualDataStore = new() { testCustomer };
            actualDataStore.ExportToTextFile(customerRepo.Path);
            actualDataStore.ImportCustomersFromTextFile(customerRepo.Path);
            actualDataStore.Should().Contain(testCustomer);
            actualDataStore.Should().BeAssignableTo<List<Customer>>();
            //clean up
            customerRepo.Clear();
            actualDataStore.ExportToTextFile<Customer>(customerRepo.Path);
        }
        #endregion
    }
}
