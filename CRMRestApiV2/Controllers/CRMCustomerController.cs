using CRMRepository;
using CRMRepository.Entities;
using CRMRepository.Validators;
using FluentValidation;
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
        public async Task<Customer?> GetAsync(string Id)
        {
            if (Repository == null)
            {
                await Task.FromException(new NullReferenceException("Repository is null"));
                return null;
            }

            var customer = await Repository.GetByIdAsync(Id);
            return customer;
        }
        /// <summary>
        /// Post customer
        /// </summary>
        /// <param name="customer"></param>
        [HttpPost]
        public async Task PostAsync(Customer customer)
        {
            var validator = new CustomerValidator();
            var validationResult = await validator.ValidateAsync(customer);
            if (!validationResult.IsValid)
            {
                await Task.FromException(new ValidationException(validationResult.Errors[0].ErrorMessage));
            }
            try
            {
                if (Repository == null)
                {
                    await Task.FromException(new NullReferenceException("Repository is null"));
                    return;
                }
                await this.Repository.AddOrUpdateAsync(customer);
            }
            catch (ArgumentException ex)
            {
                {
                    Logger.LogError(ex, "Error in PostAsync: " + ex.Message);
                    await Task.FromException(ex);
                }
            }
        }

        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="customer"></param>
        [HttpDelete]
        public async Task<HttpStatusCode> DeleteAsync(Customer customer)
        {
            return await DeleteFromRepositoryAsync(customer.Id);
        }

        /// <summary>
        /// Delete customer by Id
        /// </summary>
        /// <param name="Id"></param>       
        [HttpDelete("{Id}")]
        public async Task<HttpStatusCode> DeleteByIdAsync(string Id)
        {
            return await DeleteFromRepositoryAsync(Id);
        }

        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="Ids"></param> 
        [HttpDelete("deleterange/{Ids}")]
        public async Task<HttpStatusCode> DeleteRangeAsync(string? Ids)
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
                    var entity = await Repository.GetByIdAsync(id);
                    if (entity != null)
                    {
                        entities.Add(entity);
                    }
                }

                await Repository.DeleteRangeAsync(entities);
            }
            catch
            {
                return HttpStatusCode.InternalServerError;
            }
            return HttpStatusCode.OK;
        }

        private async Task<HttpStatusCode> DeleteFromRepositoryAsync(string Id)
        {
            if (Repository == null)
            {
                await Task.FromException(new NullReferenceException("Repository is null"));
                return HttpStatusCode.InternalServerError;
            }
            try
            {
                if (Id == null)
                {
                    return HttpStatusCode.BadRequest;
                }
                var entity = await Repository.GetByIdAsync(Id);
                if (entity != null)
                {
                    await Repository.DeleteAsync(entity);
                }
                else
                {
                    return HttpStatusCode.NotFound;
                }
            }
            catch
            {
                return HttpStatusCode.InternalServerError;
            }
            return HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets all the Customers
        /// </summary>
        /// <returns>List[Customer]</returns>
        [HttpGet()]
        public async Task<List<Customer>?> GetAllAsync()
        {
            if (Repository == null)
            {
                await Task.FromException(new NullReferenceException("Repository is null"));
                return null;
            }
            try
            {
                return await Repository.FetchAllAsync();
            }
            catch
            {
                throw;
            }

        }
    }
}
