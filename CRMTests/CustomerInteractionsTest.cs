using CRMRepository;
using CRMRepository.Entities;
using CRMRestApi.Controllers;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xbehave;
using Xunit;

namespace SimpleCRM
{
    public class CustomerOperationsFeature
    {
        #region functional api acceptance tests
        [Scenario]
        public void PostCustomerToCRM(CRMCustomerController controller,Customer customer, Mock<IRepository<Customer>> customerRepoMock)
        {

            "Given we have a new customer"
                .x(() =>
                {
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    customer = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" };
                    customerRepoMock.Setup(x => x.Add(customer));
                    customerRepoMock.Setup(x => x.FetchAll()).Returns(new List<Customer> { customer });
                    customerRepoMock.Setup(x => x.Save());
                    controller = new CRMCustomerController(customerRepoMock.Object);
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
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    customer = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe" };
                    customerRepoMock.Setup(x => x.Add(customer));
                    customerRepoMock.Setup(x => x.Get(customer)).Returns(customer);
                    customerRepoMock.Setup(x => x.Delete(customer));
                    customerRepoMock.Setup(x => x.Save());
                    controller = new CRMCustomerController(customerRepoMock.Object);
                });

            "When this customer is deleted"
                .x(() =>
                {
                    controller.Delete(customer);
                });


            "Then the customer is deleted"
                .x(() =>
                {
                    var actualCustomer = customerRepoMock.Object.Get(customer);
                    
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
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    customer = new Customer { Id = "JD2", FirstName = "Jane", LastName = "Doe" };
                    customerRepoMock.Setup(x => x.Get(customer))
                    .Throws<ArgumentOutOfRangeException>(() => new ArgumentOutOfRangeException("The Customer: " + customer.ToString() + " was not found"));
                    customerRepoMock.Setup(x => x.Delete(customer))
                    .Throws<ArgumentOutOfRangeException>(() => new ArgumentOutOfRangeException("The Customer: " + customer.ToString() + " was not found"));

                    controller = new CRMCustomerController(customerRepoMock.Object);
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

        #region technical tests
        [Fact]
        public void Controller_Throws_Argument_Null_Exception_When_Repo_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new CRMCustomerController(null));
        }
        #endregion
    }
}
