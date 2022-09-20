using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using CRMRepository.Entities;
using System.Linq;

namespace CRMRepository
{
    public static class MemoryToFileDatastore
    {
        public static void ExportToTextFile<T>(this IEnumerable<T> data, string fileName)
        {
            using var sw = File.CreateText(fileName);
            sw.AutoFlush = true;
            var Json = JsonConvert.SerializeObject(data);
            sw.WriteLine(Json);
        }

        public static List<Customer> ImportCustomersFromTextFile<Customer>(this List<Customer> data,string fileName)
        {
            var fileContents = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<List<Customer>>(fileContents).ToList<Customer>();
        }

    }
}
