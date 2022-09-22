using CRMRepository;
using CRMRepository.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CRMRestApiV2.Controllers
{
    /// <summary>
    /// CRMCustomerController form rest api/CRMCustomers
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CRMCustomerController : ControllerBase
    {
        /// <summary>
        /// IRepository Interface
        /// </summary>
        private IRepository<Customer> repository = new CustomerRepository();

        private readonly ILogger<CRMCustomerController> _logger;
      
        /// <summary>
        /// Customer repository
        /// </summary>
        public IRepository<Customer> Repository { get => repository; set => repository = value; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        public CRMCustomerController(ILogger<CRMCustomerController> logger)
        {
            _logger = logger;

        }
        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Customer> Get() => Repository.FetchAll().ToArray();

        /// <summary>
        /// Get customer by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Route("/{id?}")]
        [HttpGet]
        public Customer Get(string Id)
        {
            return Repository.GetById(Id);
        }
        /// <summary>
        /// Post customer
        /// </summary>
        /// <param name="customer"></param>
        [HttpPost]
        public void Post(Customer customer)
        {
            this.Repository.Add(customer);
            this.Repository.Save();
        }
        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="customer"></param>
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
        /// <summary>
        /// Remove all data form the file datasource leaving just an empty array --> []
        /// </summary>
        [NonAction]
        protected void ClearDataSource()
        {
            this.Repository.Clear();
        }

    }
}
