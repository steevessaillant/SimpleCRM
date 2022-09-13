using CRMRepository;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CRMRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRMCustomerController : ControllerBase
    {
        private readonly IRepository<Customer> repository;

        public CRMCustomerController(IRepository<Customer> repository)
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            this.repository = repository;
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
