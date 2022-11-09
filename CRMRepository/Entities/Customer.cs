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
        [NotNull]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Id { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [DataMember(IsRequired = true)]
        [NotNull]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string FirstName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [DataMember(IsRequired = true)]
        [NotNull]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string LastName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [DataMember(IsRequired = true)]
        [NotNull]
        public DateTimeOffset DateOfBirth { get; set; }


        public TableEntity ToAzureTableEntity(string partitionKey = "Default")
        {
            return this.ToTableEntity(partitionKey, this.Id, null);
        }

    }
}
