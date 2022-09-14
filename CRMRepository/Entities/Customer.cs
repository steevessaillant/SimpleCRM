using System;
using System.Collections.Generic;

namespace CRMRepository.Entities
{
    public class Customer : IEquatable<Customer>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Customer);
        }

        public bool Equals(Customer other)
        {
            return other != null &&
                   Id == other.Id &&
                   FirstName == other.FirstName &&
                   LastName == other.LastName;
        }

        public override string ToString()
        {
            return $"{{ Id:{Id} FirstName:{FirstName} LastName:{LastName}}}";
        }

        public static bool operator ==(Customer left, Customer right)
        {
            return EqualityComparer<Customer>.Default.Equals(left, right);
        }

        public static bool operator !=(Customer left, Customer right)
        {
            return !(left == right);
        }
    }
}
