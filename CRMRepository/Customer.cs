using System;

namespace CRMRepository
{
    public class Customer
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return "{ CustomerID: " + Id + " FirstName: " + FirstName + " LastName: " + LastName + "}";
        }
    }
}
