using CRMRepository;
using CRMRepository.Entities;
using CRMRestApiV2.Controllers;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xbehave;
using Xunit;

namespace SimpleCRM
{
    public class CustomerInteractionsTest
    {
        private readonly CRMCustomerController controller = new(null);


        #region functional api acceptance tests
        [Scenario]
        public void PostCustomerToCRM(CRMCustomerController controller,Customer customer, Mock<IRepository<Customer>> customerRepoMock)
        {

            "Given we have a new customer"
                .x(() =>
                {
                    controller = this.controller;
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    customer = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" };
                    customerRepoMock.Setup(x => x.Add(customer));
                    customerRepoMock.Setup(x => x.FetchAll()).Returns(new List<Customer> { customer });
                    customerRepoMock.Setup(x => x.Save());
                    controller.Repository = customerRepoMock.Object;
                });

            "When this customer is added"
                .x( () =>
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

                    customerRepoMock.Verify(x => x.Add(customer), Times.Once);
                    customerRepoMock.Verify(x => x.Save(), Times.Once);
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
                    customerRepoMock.Setup(x => x.Add(customer));
                    customerRepoMock.Setup(x => x.Get(customer)).Returns(customer);
                    customerRepoMock.Setup(x => x.Delete(customer));
                    customerRepoMock.Setup(x => x.Save());
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


                    customerRepoMock.Verify(x => x.Delete(customer), Times.Once);
                    customerRepoMock.Verify(x => x.Save(), Times.Once);
                });

        }

        [Scenario]
        public void DeleteCustomerFromCRMFailsWhenNotFound(CRMCustomerController controller, Customer customer, Mock<IRepository<Customer>> customerRepoMock,ArgumentOutOfRangeException exception)
        {

            "Given we have non-existing customer"
                .x(() =>
                {
                    controller = this.controller;
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    controller.Repository = customerRepoMock.Object;
                    customer = new Customer { Id = "JD2", FirstName = "Jane", LastName = "Doe" };
                    customerRepoMock.Setup(x => x.Get(customer))
                    .Throws<ArgumentOutOfRangeException>(() => new ArgumentOutOfRangeException("The Customer: " + customer.ToString() + " was not found"));
                    customerRepoMock.Setup(x => x.Delete(customer))
                    .Throws<ArgumentOutOfRangeException>(() => new ArgumentOutOfRangeException("The Customer: " + customer.ToString() + " was not found"));

                    controller = this.controller;
                });

            "When this customer is deleted"
                .x(() =>
                {
                    try
                    {
                        controller.Delete(customer);
                    }
                    catch(ArgumentOutOfRangeException ex)
                    {
                        exception = ex;
                    }

                });


            "Then the non-existing customer cannot deleted"
                .x(() =>
                {
                    exception.ParamName
                    .Should()
                    .Be("The Customer: " + customer.ToString() + " was not found");
                });

        }
        #endregion

    }
}
