using CRMRepository.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CRMRepository
{
    public class CustomerRepository : IRepository<Customer>
    {
        private List<Customer> tempDataStore = new List<Customer>();
        
        public IQueryable<Customer> Query => throw new NotImplementedException();

        public void Add(Customer entity)
        {
            if(!tempDataStore.Exists(x => x.Id == entity.Id))
            {
                tempDataStore.Add(entity);
            }
        }

        public void Delete(Customer entity)
        {
            throw new NotImplementedException();
        }

        public List<Customer> FetchAll()
        {
                using (StreamReader r = new StreamReader(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)
                + "\\Customers.json"))
                {
                    string json = r.ReadToEnd();
                    tempDataStore = JsonConvert.DeserializeObject<List<Customer>>(json);
                }
            return tempDataStore;
        }

        public Customer Get(Customer entity)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            tempDataStore
                .ExportToTextFile<Customer>(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)
                + "\\Customers.json",',');
        }
    }
}
