using CRMRepository.Entities;
using System;
using System.Collections.Generic;

namespace CRMRepository
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly List<Customer> customerList = null;


        public CustomerRepository()
        {
            customerList = new AzureTableClient().GetAllFromTable();
        }
       
        public  void AddOrUpdate(Customer entity)
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

        public void AddOrUpdateRange(List<Customer> entities)
        {
            entities.ForEach(entity => AddOrUpdate(entity));
        }


        public void Clear()
        {
            var azureTableClient = new AzureTableClient();
            var actions  = azureTableClient.DeleteRangeFromTableAsync(customerList);
            azureTableClient.SaveToTableAsync(customerList, actions);
        }

        //for now exclude becuase it is not implemented
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

        public void DeleteRange(List<Customer> entities)
        {
            entities.ForEach(entity => Delete(entity));
        }

        public List<Customer> FetchAll()
        {
            return new AzureTableClient().GetAllFromTable();
        }

        public Customer Get(Customer entity)
        {
            if (customerList.Exists(x => x.Id == entity.Id ))
            {
                return customerList.Find(x => x.Id == entity.Id);
            }
          
            return null;
        }


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


        public void Update(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
