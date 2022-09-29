using CRMRepository.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace CRMRepository
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly List<Customer> customerList = null;


        public CustomerRepository()
        {
            customerList = AzureTableClient.GetAllFromTable();
        }
       
        public  void AddOrUpdate(Customer entity)
        {
            var action = AzureTableClient.AddOrUpdateToTable(entity);
            AzureTableClient.SaveToTableAsync(new List<Customer> { entity },
                new List<Azure.Data.Tables.TableTransactionAction> { action });
        }

        public void AddOrUpdateRange(List<Customer> entities)
        {
            entities.ForEach(entity => AddOrUpdate(entity));
        }


        public void Clear()
        {
            var actions  = AzureTableClient.DeleteRangeFromTableAsync(customerList);
            AzureTableClient.SaveToTableAsync(customerList, actions);
        }

        //for now exclude becuase it is not implemented
        public void Delete(Customer entity)
        {
            var action = AzureTableClient.DeleteFromTable(entity);
            AzureTableClient.SaveToTableAsync(new List<Customer> { entity }, new List<Azure.Data.Tables.TableTransactionAction> { action });
            this.customerList.Remove(entity);
        }

        public void DeleteRange(List<Customer> entities)
        {
            entities.ForEach(entity => Delete(entity));
        }

        public List<Customer> FetchAll()
        {
            return AzureTableClient.GetAllFromTable();
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
                return AzureTableClient.GetById(Id);
            }
        }


        public void Update(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
