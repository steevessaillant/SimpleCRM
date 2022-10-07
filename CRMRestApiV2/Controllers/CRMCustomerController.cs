using CRMRepository;
using CRMRepository.Entities;
using Microsoft.AspNetCore.Mvc;
using Namotion.Reflection;
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
        /// Customer repository
        /// </summary>
        public IRepository<Customer>? Repository { get; set; } = null;

        /// <summary>
        /// Logger
        /// </summary>
        public ILogger<CRMCustomerController> Logger { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repository"></param>
        public CRMCustomerController(ILogger<CRMCustomerController> logger, IRepository<Customer>? repository = null)
        {
            Logger = logger;
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
            if (Repository == null)
            {
                throw new Exception("Repository is null");
            }
            return Repository.GetById(Id);
        }
        /// <summary>
        /// Post customer
        /// </summary>
        /// <param name="customer"></param>
        [HttpPost]
        public void Post(Customer customer)
        {
            if (Repository == null)
            {
                throw new Exception("Repository is null");
            }
            this.Repository.AddOrUpdate(customer);
        }

        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="customer"></param>
        [HttpDelete]
        public HttpStatusCode Delete(Customer customer)
        {
            return DeleteFromRepository(customer.Id);
        }

        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="Id"></param>       
        [HttpDelete("{Id}")]
        public HttpStatusCode DeleteById(string Id)
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
            if (Repository == null)
            {
                throw new Exception("Repository is null");
            }
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

        private HttpStatusCode DeleteFromRepository(string Id)
        {
            if (Repository == null)
            {
                throw new Exception("Repository is null");
            }
            try
            {
                if (Id == null)
                {
                    return HttpStatusCode.BadRequest;
                }
                var entity = Repository.GetById(Id);
                if (!Repository.Delete(entity))
                {
                    {
                        return HttpStatusCode.NotFound;
                    }
                };
            }
            catch
            {
                return HttpStatusCode.InternalServerError;
            }
            return HttpStatusCode.OK;
        }

    }
}
