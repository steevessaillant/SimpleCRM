using CRMRepository.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CRMRepository
{
    public class CustomerRepository : IRepository<Customer>, IPersistableFile
    {
        private readonly List<Customer> tempDataStore = new();
        public string Path { get => Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Customers.json"; }

        public void Add(Customer entity)
        {
            //if(tempDataStore.)
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
        [ExcludeFromCodeCoverage]
        public void Delete(Customer entity)
        {
            throw new NotImplementedException();
        }

        public List<Customer> FetchAll()
        {
            tempDataStore.ImportCustomersFromTextFile(this.Path);
            return tempDataStore;
        }

        //for now exclude becuase it is not implemented
        [ExcludeFromCodeCoverage]
        public Customer Get(Customer entity)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            tempDataStore
                .ExportToTextFile<Customer>(this.Path);
        }
    }
}
