
using System;
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

        /// <summary>
        /// Id of the customer
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Id { get; set; }

        [DataMember(IsRequired = true)]
        public string FirstName { get; set; }
        [DataMember(IsRequired = true)]
        public string LastName { get; set; }
        [DataMember(IsRequired = true)]
        public DateTimeOffset DateOfBirth { get; set; }
        

        public TableEntity ToAzureTableEntity(string partitionKey = "Default")
        {
            return this.ToTableEntity(partitionKey, this.Id, null);
        }

    }
}
