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
    public class CustomerRepository : IRepository<Customer>
    {
        private List<Customer> tempDataStore = new List<Customer>();

        public void Add(Customer entity)
        {
            if(!tempDataStore.Exists(x => x.Id == entity.Id))
            {
                tempDataStore.Add(entity);
            }
        }

        //for now exclude becuase it is not implemented
        [ExcludeFromCodeCoverage]
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

        //for now exclude becuase it is not implemented
        [ExcludeFromCodeCoverage]
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
