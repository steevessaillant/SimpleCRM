using CRMRepository;
using CRMRepository.Entities;
using CRMRestApiV2.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xbehave;
using Xunit;

namespace SimpleCRM
{
    [ExcludeFromCodeCoverage]
    public class CustomerInteractionsIntegrationsTest
    {

        #region functional api acceptance tests
        [Scenario]
        public void PostCustomersJohnAndJaneToCRM(CRMCustomerController controller,Customer John,Customer Jane, CustomerRepository customerRepo)
        {

            controller = new(null);

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

            controller = new(null);

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

        [Fact]
        public void ExportToTextFileShouldPersistTestCustomer()
        {
            CRMCustomerController controller = new(null);
            
            Customer testCustomer = new() { Id = "TEST", FirstName = "Tester", LastName = "Testing" };
            controller.Post(testCustomer);
            
           
            controller.Repository
                .As<CustomerRepository>()
                .FetchAll()
                .LoadCustomersFromTextFile<Customer>(controller.Repository.DataSourceFleLocalPath)
                .Should().Contain(testCustomer);
           
            //clean up
            controller.Delete(testCustomer);
                
        }

        [Fact]
        public void CanResetAndCreateAnEmptyList()
        {
            var customerRepo = new CustomerRepository(false);

            customerRepo.FetchAll().Count.Should().Be(0);

        }
        #endregion
    }
}
