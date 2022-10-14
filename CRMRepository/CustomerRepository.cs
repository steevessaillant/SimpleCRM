using CRMRepository.Entities;
using CRMRepository.Validators;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CRMRepository
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly List<Customer>? customerList = null;

        /// <summary>
        /// Instanciate a Customer datasource
        /// </summary>
        public CustomerRepository()
        {
            customerList = new AzureTableClient().GetAllFromTable();
        }

        /// <summary>
        /// Add Or Update a Customer
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task AddOrUpdateAsync(Customer entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            
            var azureTableClient = new AzureTableClient();
            var action = azureTableClient.AddOrUpdateToTable(entity);
            await azureTableClient.SaveToTableAsync(new List<Customer> { entity },
                new List<Azure.Data.Tables.TableTransactionAction> { action });
        }

        /// <summary>
        /// Add Or Update multiple Customers
        /// </summary>
        /// <param name="entities"></param>
        public async Task AddOrUpdateRangeAsync(List<Customer> entities)
        {
            entities.ForEach(async entity => await Task.FromResult(AddOrUpdateAsync(entity)));
            await Task.CompletedTask;
        }



        /// <summary>
        /// Delete a Customer
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Customer entity)
        {
            if (this.customerList == null)
            {
                return await Task.FromResult(false);
            }
            if (entity == null)
            {
                return await Task.FromResult(false);
            }
            var azureTableClient = new AzureTableClient();
            var action = azureTableClient.DeleteFromTable(entity);
            await azureTableClient.SaveToTableAsync(new List<Customer> { entity }, new List<Azure.Data.Tables.TableTransactionAction> { action });
            
            return await Task.FromResult(this.customerList.Remove(entity));
        }

        /// <summary>
        /// Delete multiple customers
        /// </summary>
        /// <param name="entities"></param>
        public async Task DeleteRangeAsync(List<Customer> entities)
        {
            entities.ForEach(async entity => await DeleteAsync(entity));
            await Task.CompletedTask;
        }

        /// <summary>
        /// Get all Customers in datastore
        /// </summary>
        /// <returns></returns>
        public async Task<List<Customer>?> FetchAllAsync()
        {
            return await Task.FromResult(new AzureTableClient().GetAllFromTable());
        }

        /// <summary>
        /// Gets a single customer
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Customer?> GetAsync(Customer entity)
        {
            if (this.customerList == null)
            {
                return null;
            }

            if (customerList.Exists(x => x.Id == entity.Id ))
            {
                return await Task.FromResult(customerList.Find(x => x.Id == entity.Id));
            }

            return null;
          
        }

        /// <summary>
        /// Gets a single customer by its Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Customer?> GetByIdAsync(string Id)
        {
            if (this.customerList == null)
            {
                return null;
            }
            if (customerList.Exists(x => x.Id == Id))
            {
                return await Task.FromResult(customerList.Find(x => x.Id == Id));
            }
            else
            {
                return new AzureTableClient().GetById(Id);
            }
        }
        
        public async Task<ValidationResult> ValidateEntity(Customer entity)
        {
            return await new CustomerValidator().ValidateAsync(entity);
        }


    }
}
