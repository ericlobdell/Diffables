using System.Collections.Generic;

namespace Diffables.Tests.Models
{
    public class DiffablePerson : DiffableObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public List<string> Pets { get; set; }
        public Address Address { get; set; }


    }

    public class Address
    {
        public int StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
