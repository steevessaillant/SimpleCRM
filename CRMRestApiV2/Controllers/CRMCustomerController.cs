using CRMRepository;
using CRMRepository.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CRMRestApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRMCustomerController : ControllerBase
    {
        private IRepository<Customer> repository = new CustomerRepository();

        private readonly ILogger<CRMCustomerController> _logger;

        public IRepository<Customer> Repository { get => repository; set => repository = value; }

        public CRMCustomerController(ILogger<CRMCustomerController> logger)
        {
            _logger = logger;

        }

        [HttpGet(Name = "GetCustomers")]
        public IEnumerable<Customer> Get()
        {
            return Repository.FetchAll().ToArray();
        }

        [HttpPost]
        public void Post(Customer customer)
        {
            this.Repository.Add(customer);
            this.Repository.Save();
        }

        [HttpDelete]
        public void Delete(Customer customer)
        {
            try
            {
                this.Repository.Delete(this.Repository.Get(customer));
                this.Repository.Save();
            }
            catch
            {
                throw;
            }
        }

        public void ClearDataSource()
        {
            this.Repository.Clear();
        }

    }
}
