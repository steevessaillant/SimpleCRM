using Azure;
using Azure.Data.Tables;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using TableStorage.Abstractions.TableEntityConverters;
using TableEntity = Azure.Data.Tables.TableEntity;

namespace CRMRepository.Entities
{
    /// <summary>
    /// Represents a Customer Account
    /// </summary>
    [DataContract]
    public class Customer
    {
        private int _age;

        /// <summary>
        /// Id of the customer
        /// </summary>
        [ExcludeFromCodeCoverage]
        [DataMember]
        public string Id { get; set; }

        [ExcludeFromCodeCoverage]
        [DataMember]
        public string FirstName { get; set; }
        [ExcludeFromCodeCoverage]
        [DataMember]
        public string LastName { get; set; }
        [DataMember(IsRequired = true)]
        public int Age
        {

            get => _age;
            set
            {
                {
                    if (value < 18)
                    {
                        throw new ArgumentException("Age must be 18 or older");
                    }
                    else
                    {
                        _age = value;
                    }
                }
            }
        }

        public TableEntity ToAzureTableEntity(string partitionKey = "Default")
        {
            return this.ToTableEntity(partitionKey,this.Id,null);
        }
        
    }
}
