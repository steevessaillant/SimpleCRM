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
    public class AzureTableClient
    {
        private const string ConnectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;\r\nAccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;\r\nBlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;\r\nQueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;\r\nTableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
        private const string TableName = "Customers";

       
       
        private readonly TableClient tableClient = new(ConnectionString, TableName);

        /// <summary>
        /// Start client
        /// </summary>
        /// <exception cref="ApplicationException"></exception>
        public AzureTableClient()
        {
            try
            {
                tableClient.CreateIfNotExists();
            }
            catch
            {
                throw new ApplicationException("Unable to connect to Azure Storage");
            }
        }

        /// <summary>
        /// Save 
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="tableTransactionActionList"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        internal async Task SaveToTableAsync(List<Customer> customers, List<TableTransactionAction> tableTransactionActionList)
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
            
            await tableClient.SubmitTransactionAsync(tableTransactionActionList);
           
        }

        /// <summary>
        /// Get all Customers
        /// </summary>
        /// <returns></returns>
        internal  List<Customer>? GetAllFromTable()
        {
            var customers = new List<Customer>();
    
            var tableEntities = tableClient.Query<TableEntity>();
            if(tableEntities == null)
            {
                return null;
            }

            foreach (var entity in tableEntities.ToList())
            {
                customers.Add(entity.FromTableEntity<Customer>());
            }
            return customers;

        }

        /// <summary>
        /// Get customer by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal Customer? GetById(string Id)
        {
            var customers = new List<Customer>();

            var entity = tableClient.Query<TableEntity>(x => x.PartitionKey == "Default"
            && x.RowKey == Id).FirstOrDefault();

            if (entity == null)
            {
                return null;
            }
            return entity.FromTableEntity<Customer>();

        }

        /// <summary>
        /// DeleteRangeFromTableAsync 
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        internal List<TableTransactionAction> DeleteRangeFromTableAsync(List<Customer> customers)
        {
            List<TableTransactionAction> transactionActions = new();
            transactionActions.AddRange(customers.Select(x => new TableTransactionAction(TableTransactionActionType.Delete, x.ToAzureTableEntity())));
            return transactionActions;
        }

        internal TableTransactionAction DeleteFromTable(Customer customer)
        {
            if (customer is null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            return new TableTransactionAction(TableTransactionActionType.Delete, customer.ToAzureTableEntity());

        }

        /// <summary>
        ///  AddOrUpdateToTable
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal TableTransactionAction AddOrUpdateToTable(Customer customer)
        {
            if (customer is null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            return new TableTransactionAction(TableTransactionActionType.UpsertMerge, customer.ToAzureTableEntity());
        }
       
    }
}