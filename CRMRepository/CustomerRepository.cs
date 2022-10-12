using CRMRepository.Entities;
using System;
using System.Collections.Generic;

namespace CRMRepository
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly List<Customer> customerList = null;

        /// <summary>
        /// Instanciate a Customer datasource
        /// </summary>
        public CustomerRepository()
        {
            customerList = new AzureTableClient().GetAllFromTable();
        }

        /// <summary>
        /// Add Or Updatea Customer
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddOrUpdate(Customer entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var azureTableClient = new AzureTableClient();
            var action = azureTableClient.AddOrUpdateToTable(entity);
            azureTableClient.SaveToTableAsync(new List<Customer> { entity },
                new List<Azure.Data.Tables.TableTransactionAction> { action });
        }

        /// <summary>
        /// Add Or Update multiple Customers
        /// </summary>
        /// <param name="entities"></param>
        public void AddOrUpdateRange(List<Customer> entities)
        {
            entities.ForEach(entity => AddOrUpdate(entity));
        }



        /// <summary>
        /// Delete a Customer
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(Customer entity)
        {
            if (entity == null)
            {
                return false;
            }
            var azureTableClient = new AzureTableClient();
            var action = azureTableClient.DeleteFromTable(entity);
            azureTableClient.SaveToTableAsync(new List<Customer> { entity }, new List<Azure.Data.Tables.TableTransactionAction> { action });
            return this.customerList.Remove(entity);
        }

        /// <summary>
        /// Delete multiple customers
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(List<Customer> entities)
        {
            entities.ForEach(entity => Delete(entity));
        }

        /// <summary>
        /// Get all Customers in datastore
        /// </summary>
        /// <returns></returns>
        public List<Customer> FetchAll()
        {
            return new AzureTableClient().GetAllFromTable();
        }

        /// <summary>
        /// Gets a single customer
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Customer Get(Customer entity)
        {
            if (customerList.Exists(x => x.Id == entity.Id ))
            {
                return customerList.Find(x => x.Id == entity.Id);
            }
          
            return null;
        }

        /// <summary>
        /// Gets a single customer by its Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Customer GetById(string Id)
        {
            if (customerList.Exists(x => x.Id == Id))
            {
                return customerList.Find(x => x.Id == Id);
            }
            else
            {
                return new AzureTableClient().GetById(Id);
            }
        }

        /// <summary>
        /// Maybe TODO
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Update(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
