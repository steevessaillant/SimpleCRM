using CRMRepository.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace CRMRepository
{
    public class CustomerRepository : IRepository<Customer>, IPersistableFile
    {
        private readonly List<Customer> tempDataStore = new();

        public string Path => Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Customers.json";

        public CustomerRepository()
        {
            tempDataStore.ImportCustomersFromTextFile(this.Path);
        }
        public void Add(Customer entity)
        {
            if(!tempDataStore.Exists(x => x.Id == entity.Id))
            {
                tempDataStore.Add(entity);
            }
        }

        public void Clear()
        {
            tempDataStore.Clear();
            this.Save();
        }

        //for now exclude becuase it is not implemented
        public void Delete(Customer entity)
        {
            tempDataStore.Remove(entity);
            this.Save();
        }

        public List<Customer> FetchAll()
        {
            return tempDataStore;
        }

        public Customer Get(Customer entity)
        {
            if(tempDataStore.Exists(x => x.Id == entity.Id))
            {
                return entity;
            }
            return null;
        }

        public Customer GetById(string Id)
        {
            if (tempDataStore.Exists(x => x.Id == Id))
            {
                return tempDataStore.Find(x => x.Id == Id);
            }
            return null;
        }

        public void Save()
        {
            tempDataStore
                .ExportToTextFile<Customer>(this.Path);
        }

        public void Update(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
