using CRMRepository;
using CRMRepository.Entities;
using CRMRepository.Validators;
using CRMRestApiV2.Controllers;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using Xbehave;

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
                    customer = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe", Age = 18 };
                    customerRepoMock.Setup(x => x.AddOrUpdateAsync(customer));
                    customerRepoMock.Setup(x => x.GetByIdAsync(customer.Id)).Returns(Task.FromResult(customer));
                });

            "When this customer is added"
                .x(async () =>
                {
                    await controller.PostAsync(customer);
                });


            "Then the customer is added"
                .x(async () =>
                {
                    var result = await controller.GetAsync(customer.Id);
                    result.Should().Be(customer);

                    customerRepoMock.Verify(x => x.AddOrUpdateAsync(customer), Times.Once);
                    customerRepoMock.Verify(x => x.GetByIdAsync(customer.Id), Times.Once);

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
                .x(async () =>
                {
                    customerRepoMock.Setup(x => x.AddOrUpdateAsync(customer));
                    customerRepoMock.Setup(x => x.GetAsync(customer)).Returns(Task.FromResult(customer));
                    customerRepoMock.Setup(x => x.DeleteAsync(customer));
                    await controller.DeleteAsync(customer);
                });


            "Then the customer is deleted"
                .x(async () =>
                {
                    var actualCustomer = await controller.Repository.GetAsync(customer);

                    actualCustomer
                    .Should()
                    .Be(customer);


                    customerRepoMock.Verify(x => x.GetAsync(customer), Times.Once);
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
                    customerRepoMock.Setup(x => x.AddOrUpdateRangeAsync(new List<Customer> { john, jane }));
                    customerRepoMock.Setup(x => x.FetchAllAsync()).Returns(Task.FromResult(new List<Customer> { john, jane }));
                });


            "Then these same customer are returned"
                .x(() =>
                {
                    var actualCustomers = controller.Repository.FetchAllAsync();

                    actualCustomers.Result
                    .Should()
                    .Contain(new List<Customer> { john, jane });


                    customerRepoMock.Verify(x => x.FetchAllAsync(), Times.Once);
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
                .x(async () =>
                {
                    customerRepoMock.Setup(x => x.AddOrUpdateAsync(customer));
                    customerRepoMock.Setup(x => x.GetByIdAsync(Id)).Returns(Task.FromResult(customer));
                    customerRepoMock.Setup(x => x.DeleteAsync(customer));
                    await controller.DeleteAsync(customer);
                });


            "Then the customer is deleted"
                .x(async () =>
                {
                    var actualCustomer = await controller.Repository.GetAsync(customer);

                    actualCustomer
                    .Should()
                    .Be(null);


                    customerRepoMock.Verify(x => x.GetAsync(customer), Times.Once);
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
                    customerRepoMock.Setup(x => x.GetAsync(customer))
                    .Throws(() => new ArgumentOutOfRangeException("The Customer: " + customer.ToString() + " was not found"));
                    customerRepoMock.Setup(x => x.DeleteAsync(customer))
                    .Throws(() => new ArgumentOutOfRangeException("The Customer: " + customer.ToString() + " was not found"));
                });

            "When this customer is attempted to be deleted"
                .x(async () =>
                {
                    actual = await controller.DeleteAsync(customer);

                });


            "Then the non-existing customer cannot be deleted"
                .x(() =>
                {
                    expected.Should().Be(actual);
                });

        }

        #endregion
        #region functional tests for business rule Customer Age must be 18 yrs old or more


        [Scenario]
        public void CustomerMustBeAnAdult(CRMCustomerController controller, Customer customer, Mock<IRepository<Customer>> customerRepoMock, CustomerValidator validator, ValidationFailure validationFailure, Task task)
        {
            "Given we have a new customer that is 17 yrs of age"
                .x(() =>
                {
                    customerRepoMock = new Mock<IRepository<Customer>>();
                    controller = new CRMCustomerController(null, customerRepoMock.Object);
                    customer = new Customer { Id = "JD1", FirstName = "John", LastName = "Doe", Age = 17 };
                    customerRepoMock.Setup(x => x.AddOrUpdateAsync(customer));

                });
            
             "When it is attempted to be added to the CRM"
                        .x(async () =>
                        {
                            try {
                                await controller.PostAsync(customer);
                            }
                            catch (ValidationException ex)
                            {
                                validationFailure = new ValidationFailure("Age", ex.Message);
                            }
                           
                        });


            "Then the customer is not added to the CRM"
                .x(() =>
                {
                    customerRepoMock.Verify(x => x.AddOrUpdateAsync(customer), Times.Never);
                    validationFailure.Should().NotBeNull();
                    validationFailure.ErrorMessage.Should().Contain("Age must be 18 or older");
                });
        }
        #endregion
    }
}
