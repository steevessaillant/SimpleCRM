﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace CRMRepository.Entities
{
    [DataContract]
    public class Customer : IEquatable<Customer>
    {
        [ExcludeFromCodeCoverage]
        [DataMember]
        public string Id { get; set; }
        [ExcludeFromCodeCoverage]
        [DataMember]
        public string FirstName { get; set; }
        [ExcludeFromCodeCoverage]
        [DataMember]
        public string LastName { get; set; }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Customer);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Customer other)
        {
            return other != null &&
                   Id == other.Id &&
                   FirstName == other.FirstName &&
                   LastName == other.LastName;
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return base.GetHashCode();

            throw new DivideByZeroException("Cannot divide by 0");
        }

        public override string ToString()
        {
            return $"{{ Id:{Id} FirstName:{FirstName} LastName:{LastName}}}";
        }

        [ExcludeFromCodeCoverage]
        public static bool operator ==(Customer left, Customer right)
        {
            return EqualityComparer<Customer>.Default.Equals(left, right);
        }

        [ExcludeFromCodeCoverage]
        public static bool operator !=(Customer left, Customer right)
        {
            return !(left == right);
        }
    }
}
