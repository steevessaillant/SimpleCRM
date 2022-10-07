using CRMRepository;
using CRMRepository.Entities;
using CRMRestApiV2.Controllers;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Xbehave;
using Xunit;

namespace CRMTests
{
    [ExcludeFromCodeCoverage]
    public class CustomerInteractionsTest
    {
        private readonly CRMCustomerController controller = new(null);


        #region functional api acceptance tests

        [Scenario]
        public void PostCustomerToCRM(CRMCustomerController controller, Customer customer, Mock<IRepository<Customer>> customerRepoMock)
        {

            "Given we have a new customer"
                .x(() =>
                {
                    controller = this.controller;
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    customer = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" };
                    customerRepoMock.Setup(x => x.AddOrUpdate(customer));
                    customerRepoMock.Setup(x => x.FetchAll()).Returns(new List<Customer> { customer });
                    controller.Repository = customerRepoMock.Object;
                });

            "When this customer is added"
                .x(() =>
                {
                    controller.Post(customer);
                });


            "Then the customer is added"
                .x(() =>
                {
                    customerRepoMock.Object
                    .FetchAll()
                    .Find(x => x.Id == customer.Id)
                    .Should().Be(customer);

                    customerRepoMock.Verify(x => x.AddOrUpdate(customer), Times.Once);
                });

        }

        [Scenario]
        public void DeleteCustomerFromCRM(CRMCustomerController controller, Customer customer, Mock<IRepository<Customer>> customerRepoMock)
        {

            "Given we have an existing customer"
                .x(() =>
                {
                    controller = this.controller;
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    controller.Repository = customerRepoMock.Object;
                    customer = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" };

                });

            "When this customer is deleted"
                .x(() =>
                {
                    customerRepoMock.Setup(x => x.AddOrUpdate(customer));
                    customerRepoMock.Setup(x => x.Get(customer)).Returns(customer);
                    customerRepoMock.Setup(x => x.Delete(customer));
                    controller = this.controller;
                    controller.Delete(customer);
                });


            "Then the customer is deleted"
                .x(() =>
                {
                    var actualCustomer = controller.Repository.Get(customer);

                    actualCustomer
                    .Should()
                    .Be(customer);


                    customerRepoMock.Verify(x => x.Get(customer), Times.Once);
                });

        }

        [Scenario]
        public void GetAllCustomerFromCRM(CRMCustomerController controller, Customer john, Customer jane, Mock<IRepository<Customer>> customerRepoMock)
        {

            "Given we all these customer"
                .x(() =>
                {
                    controller = this.controller;
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    controller.Repository = customerRepoMock.Object;
                    john = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" };
                    jane = new Customer { Id = "JD2", FirstName = "Jane", LastName = "Doe" };

                });

            "When these customers are added"
                .x(() =>
                {
                    customerRepoMock.Setup(x => x.AddOrUpdateRange(new List<Customer> { john, jane }));
                    customerRepoMock.Setup(x => x.FetchAll()).Returns(new List<Customer> { john, jane });
                    controller = this.controller;
                });


            "Then these same customer are returned"
                .x(() =>
                {
                    var actualCustomers = controller.Repository.FetchAll();

                    actualCustomers
                    .Should()
                    .Contain(new List<Customer> { john, jane });


                    customerRepoMock.Verify(x => x.FetchAll(), Times.Once);
                });

        }

        [Scenario]
        public void DeleteCustomerByIdFromCRM(CRMCustomerController controller, string Id, Customer customer, Mock<IRepository<Customer>> customerRepoMock)
        {

            "Given we have an existing customer"
                .x(() =>
                {
                    controller = this.controller;
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    controller.Repository = customerRepoMock.Object;
                    customer = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" };

                });

            "When this customer is deleted"
                .x(() =>
                {
                    customerRepoMock.Setup(x => x.AddOrUpdate(customer));
                    customerRepoMock.Setup(x => x.GetById(Id)).Returns(customer);
                    customerRepoMock.Setup(x => x.Delete(customer));
                    controller = this.controller;
                    controller.Delete(customer);
                });


            "Then the customer is deleted"
                .x(() =>
                {
                    var actualCustomer = controller.Repository.Get(customer);

                    actualCustomer
                    .Should()
                    .Be(null);


                    customerRepoMock.Verify(x => x.Get(customer), Times.Once);
                });

        }

        [Scenario]
        public void DeleteCustomerFromCRMFailsWhenNotFound(CRMCustomerController controller, Customer customer, Mock<IRepository<Customer>> customerRepoMock, ArgumentOutOfRangeException exception)
        {
            HttpStatusCode expected = HttpStatusCode.NotFound;
            HttpStatusCode actual = HttpStatusCode.Unused;

            "Given we have non-existing customer"
                .x(() =>
                {
                    controller = this.controller;
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    controller.Repository = customerRepoMock.Object;
                    customer = new Customer { Id = "JD2", FirstName = "Jane", LastName = "Doe" };
                    customerRepoMock.Setup(x => x.Get(customer))
                    .Throws(() => new ArgumentOutOfRangeException("The Customer: " + customer.ToString() + " was not found"));
                    customerRepoMock.Setup(x => x.Delete(customer))
                    .Throws(() => new ArgumentOutOfRangeException("The Customer: " + customer.ToString() + " was not found"));

                    controller = this.controller;
                });

            "When this customer is deleted"
                .x(() =>
                {
                    actual = controller.Delete(customer);

                });


            "Then the non-existing customer cannot deleted"
                .x(() =>
                {
                    expected.Should().Be(actual);
                });

        }


        #endregion

    }
}
