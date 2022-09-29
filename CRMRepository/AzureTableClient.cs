using Azure;
using Azure.Data.Tables;
using CRMRepository.Entities;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TableStorage.Abstractions.TableEntityConverters;

namespace CRMRepository
{
    public static class AzureTableClient
    {
        private static readonly string azureTableUrl = "https://simplecrmfilestore.table.core.windows.net/Customers";
        private static readonly TableClient tableClient = new(new Uri(azureTableUrl),
        "Customers", new TableSharedKeyCredential("simplecrmfilestore", "x2WRxcLvZ2K6WFwOb4OUPiJqvI2RBKm6rd7OuMfxz3OBRtASQ/KsTm2rVHxE8apwp2Zfs9s+RnNy+AStIysb/w=="));

            
        internal static void SaveToTableAsync(List<Customer> customers, List<TableTransactionAction> tableTransactionActionList)
        {
            if (customers == null)
            {
                throw new ArgumentNullException(nameof(customers)); 
            }
            if (tableTransactionActionList == null)
            {
                throw new ArgumentNullException(nameof(tableTransactionActionList));
            }
            if (customers.Count != tableTransactionActionList.Count)
            {
                throw new ArgumentException("The number of customers and tableTransactionActionList must be the same");
            }
            if (customers.Count == 0)
            {
                return;
            }

            tableClient.SubmitTransaction(tableTransactionActionList);
           
        }

        internal static List<Customer> GetAllFromTable()
        {
            var customers = new List<Customer>();
    
            var tableEntities = tableClient.Query<TableEntity>();
            if(tableEntities == null)
            {
                return customers;
            }

            foreach (var entity in tableEntities.ToList())
            {
                customers.Add(entity.FromTableEntity<Customer>());
            }
            return customers;

        }

        internal static Customer GetById(string Id)
        {
            var customers = new List<Customer>();

            var entity = tableClient.Query<TableEntity>(x => x.PartitionKey == "Default"
            && x.RowKey == Id).FirstOrDefault();

            return entity.FromTableEntity<Customer>();

        }

        internal static List<TableTransactionAction> DeleteRangeFromTableAsync(List<Customer> customers)
        {
            List<TableTransactionAction> transactionActions = new();
            transactionActions.AddRange(customers.Select(x => new TableTransactionAction(TableTransactionActionType.Delete, x.ToAzureTableEntity())));
            return transactionActions;
        }

        internal static TableTransactionAction DeleteFromTable(Customer customer)
        {
            if (customer is null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            return new TableTransactionAction(TableTransactionActionType.Delete, customer.ToAzureTableEntity());

        }

        internal static TableTransactionAction AddOrUpdateToTable(Customer customer)
        {
            if (customer is null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            return new TableTransactionAction(TableTransactionActionType.UpsertMerge, customer.ToAzureTableEntity());
        }
       
    }
}