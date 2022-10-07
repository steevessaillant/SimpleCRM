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



        #region functional api acceptance tests (CRUD)

        [Scenario]
        public void AddCustomerToRepository(CRMCustomerController controller, Customer customer, Mock<IRepository<Customer>> customerRepoMock)
        {

            "Given we have a new customer"
                .x(() =>
                {
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    controller = new CRMCustomerController(null, customerRepoMock.Object);
                    customer = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" };
                    customerRepoMock.Setup(x => x.AddOrUpdate(customer));
                    customerRepoMock.Setup(x => x.GetById(customer.Id)).Returns(customer);
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
                    .GetById(customer.Id)
                    .Should().Be(customer);

                    customerRepoMock.Verify(x => x.AddOrUpdate(customer), Times.Once);
                    customerRepoMock.Verify(x => x.GetById(customer.Id), Times.Once);

                });

        }

        [Scenario]
        public void DeleteCustomerFromCRM(CRMCustomerController controller, Customer customer, Mock<IRepository<Customer>> customerRepoMock)
        {

            "Given we have an existing customer"
                .x(() =>
                {
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    controller = new CRMCustomerController(null, customerRepoMock.Object);
                    customer = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" };

                });

            "When this customer is deleted"
                .x(() =>
                {
                    customerRepoMock.Setup(x => x.AddOrUpdate(customer));
                    customerRepoMock.Setup(x => x.Get(customer)).Returns(customer);
                    customerRepoMock.Setup(x => x.Delete(customer));
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

            "Given we have these customer"
                .x(() =>
                {
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    controller = new CRMCustomerController(null, customerRepoMock.Object);
                    john = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" };
                    jane = new Customer { Id = "JD2", FirstName = "Jane", LastName = "Doe" };

                });

            "When these customers are added"
                .x(() =>
                {
                    customerRepoMock.Setup(x => x.AddOrUpdateRange(new List<Customer> { john, jane }));
                    customerRepoMock.Setup(x => x.FetchAll()).Returns(new List<Customer> { john, jane });
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
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    controller = new CRMCustomerController(null, customerRepoMock.Object);
                    customer = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" };

                });

            "When this customer is deleted"
                .x(() =>
                {
                    customerRepoMock.Setup(x => x.AddOrUpdate(customer));
                    customerRepoMock.Setup(x => x.GetById(Id)).Returns(customer);
                    customerRepoMock.Setup(x => x.Delete(customer));
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
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    controller = new CRMCustomerController(null, customerRepoMock.Object);
                    customer = new Customer { Id = "JD2", FirstName = "Jane", LastName = "Doe" };
                    customerRepoMock.Setup(x => x.Get(customer))
                    .Throws(() => new ArgumentOutOfRangeException("The Customer: " + customer.ToString() + " was not found"));
                    customerRepoMock.Setup(x => x.Delete(customer))
                    .Throws(() => new ArgumentOutOfRangeException("The Customer: " + customer.ToString() + " was not found"));
                });

            "When this customer is attempted to be deleted"
                .x(() =>
                {
                    actual = controller.Delete(customer);

                });


            "Then the non-existing customer cannot be deleted"
                .x(() =>
                {
                    expected.Should().Be(actual);
                });

        }

        #endregion
        #region functional tests for business rule Customer Age must be 10 yrs old or more
    

        [Scenario]
        public void CustomerMustBeAnAdult(CRMCustomerController controller, Customer customer, Mock<IRepository<Customer>> customerRepoMock)
        {

            "Given we have a new customer that is 17 yrs of age,it cannot be instanciated thus not added"
                .x(() =>
                {
                    try
                    {
                        customerRepoMock = new Mock<IRepository<Customer>>();
                        controller = new CRMCustomerController(null, customerRepoMock.Object);
                        customer = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe", Age = 17 };
                    }
                    catch (ArgumentException ex)
                    {
                        ex.Message.Should().Be("Age must be 18 or older");
                    }
                });

        }


        #endregion

    }
}
