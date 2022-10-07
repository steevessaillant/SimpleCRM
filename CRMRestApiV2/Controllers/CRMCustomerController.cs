using CRMRepository;
using CRMRepository.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        private IRepository<Customer>? repository = null;

        private readonly ILogger<CRMCustomerController> _logger;
      
        /// <summary>
        /// Customer repository
        /// </summary>
        public IRepository<Customer> Repository { get => repository; set => repository = value; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repository"></param>
        public CRMCustomerController(ILogger<CRMCustomerController> logger, IRepository<Customer>? repository = null)
        {
            _logger = logger;
            this.Repository = repository ?? new CustomerRepository();

        }

        /// <summary>
        /// Get customer by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
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
            this.Repository.AddOrUpdate(customer);
        }

        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="customer"></param>
        [HttpDelete]
        public HttpStatusCode Delete(Customer customer)
        {
            return DeleteFromRepository(null, customer);
        }

        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="Id"></param>       
        [HttpDelete("{Id}")]
        public HttpStatusCode DeleteById(string? Id)
        {
            return DeleteFromRepository(Id);
        }

        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="Ids"></param> 
        [HttpDelete("deleterange/{Ids}")]
        public HttpStatusCode DeleteRange(string? Ids)
        {
            try
            {
                if (Ids == null)
                {
                    return HttpStatusCode.BadRequest;
                }
                var ids = Ids.Split(',');
                var entities = new List<Customer>();
                foreach (var id in ids)
                {
                    entities.Add(Repository.GetById(id));
                }

                Repository.DeleteRange(entities);
            }
            catch
            {
                return HttpStatusCode.InternalServerError;
            }
            return HttpStatusCode.OK;
        }

        private HttpStatusCode DeleteFromRepository(string? Id = null,Customer? customer = null)
        {
            string? id = Id;
            if (customer != null)
            {
                id = customer.Id;
            }
            if (id != null)
            {
                customer = Repository.GetById(id);
                if (Repository.Delete(customer))
                {
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.NotFound;
                }
            }
            else
            {
                return HttpStatusCode.BadRequest;
            }
        }

    }
}
