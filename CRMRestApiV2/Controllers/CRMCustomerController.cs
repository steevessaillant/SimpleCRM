using CRMRepository;
using CRMRepository.Entities;
using CRMRestApiV2;
using CRMRestApiV2.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CRMRestApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRMCustomerController : ControllerBase
    {
        private readonly IRepository<Customer> repository = new CustomerRepository();

        private readonly ILogger<CRMCustomerController> _logger;

        public CRMCustomerController(ILogger<CRMCustomerController> logger)
        {
            _logger = logger;

        }

        [HttpGet(Name = "GetCustomers")]
        public IEnumerable<Customer> Get()
        {
            return repository.FetchAll().ToArray();
        }

        [HttpPost]
        public void Post(Customer customer)
        {
            this.repository.Add(customer);
            this.repository.Save();
        }

        [HttpDelete]
        public void Delete(Customer customer)
        {
            try
            {
                this.repository.Delete(this.repository.Get(customer));
                this.repository.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
