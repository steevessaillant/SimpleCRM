using CRMRepository.Entities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CRMRepository
{
    public static class MemoryToFileDatastore
    {
        public static void ExportToTextFile<T>(this IEnumerable<T> data, string fileName)
        {
            WriteEnumerableToFile(data, fileName);
        }

        private static void WriteEnumerableToFile<T>(IEnumerable<T> data, string fileName)
        {
            var serializer = new DataContractJsonSerializer(data.GetType());
            using var stream = new FileStream(fileName, FileMode.OpenOrCreate);
            if(data.ToList().Count == 0)
            {
                stream.SetLength(0);
                stream.Close();
                return;
            }
            serializer.WriteObject(stream, data);
            stream.Close();
        }

        public static void ExportToTextFile<T>(this List<T> data, string fileName)
        {
            WriteEnumerableToFile(data.AsEnumerable(), fileName);
        }

        public static List<Entities.Customer> LoadCustomersFromTextFile<Customer>(this List<Entities.Customer> data, string fileName)
        {
            if (File.Exists(fileName))
            {
                using var sr = File.OpenText(fileName);
                var json = sr.ReadToEnd();
                List<Entities.Customer> persistedList = JsonConvert.DeserializeObject<List<Entities.Customer>>(json);
                return persistedList;
            }
            else
            {
                data.ExportToTextFile(fileName);
                return data;
            }
        }

        private static void CreateIfFileNotFoundAndReadAll<Customer>(List<Customer> data, string fileName)
        {
            if (File.Exists(fileName))
            {
                using var sr = File.OpenText(fileName);
                var json = sr.ReadToEnd();
                data.AddRange(JsonConvert.DeserializeObject<List<Customer>>(json));
                sr.Close();
            }
            else
            {


            }

        }

        public static byte[] ToByteArray(this string @this)
        {
            Encoding encoding = Activator.CreateInstance<ASCIIEncoding>();
            return encoding.GetBytes(@this);
        }
    }
    
    public class CustomerComparer : IEqualityComparer<Customer>
    {
        public int GetHashCode(Customer customer)
        {
            if (customer == null)
            {
                return 0;
            }
            return customer.Id.GetHashCode();
        }

        public bool Equals(Customer x1, Customer x2)
        {
            if (object.ReferenceEquals(x1, x2))
            {
                return true;
            }
            if (object.ReferenceEquals(x1, null) ||
                object.ReferenceEquals(x2, null))
            {
                return false;
            }
            return x1.Id == x2.Id;
        }
    }
}
