using CRMRepository;
using CRMRepository.Entities;
using CRMRestApiV2.Controllers;
using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using Xbehave;
using Xunit;

namespace SimpleCRM
{
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
            controller.ClearDataSource();

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
            List<Customer> tempDataStore = new() { testCustomer };
            tempDataStore.ExportToTextFile(customerRepo.Path);
            var actual = tempDataStore.ImportCustomersFromTextFile(customerRepo.Path);
            actual.Should().Contain(testCustomer);
            actual.Should().BeAssignableTo<List<Customer>>();
            //clean up
            customerRepo.Clear();
            tempDataStore.ExportToTextFile<Customer>(customerRepo.Path);
        }
        #endregion
    }
}
