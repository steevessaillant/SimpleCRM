using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CRMRepository
{
    public static class MemoryToFileDatastore
    {
        public static void ExportToTextFile<T>(this IEnumerable<T> data, string FileName, char ColumnSeperator)
        {
            using var sw = File.CreateText(FileName);
            sw.AutoFlush = true;
            var Json = JsonConvert.SerializeObject(data);
            sw.WriteLine(Json);
        }
    }
}
