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

        [HttpGet]
        public IEnumerable<Customer> Get() => Repository.FetchAll().ToArray();

        [Route("/{id?}")]
        [HttpGet]
        public Customer Get(string Id)
        {
            return Repository.GetById(Id);
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
                this.Repository.Delete(customer);
                this.Repository.Save();
            }
            catch
            {
                throw;
            }
        }

        protected void ClearDataSource()
        {
            this.Repository.Clear();
        }

    }
}
