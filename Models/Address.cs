using System;
using System.Collections.Generic;

namespace ZofyaApi.Models
{
    public partial class Address
    {
        public Address()
        {
            Customer_Addresses = new HashSet<Customer_Address>();
            Orders = new HashSet<Order>();
        }

        public int IDAddress { get; set; }
        public string City { get; set; } = null!;
        public string Colony { get; set; } = null!;
        public string InsideNumber { get; set; } = null!;
        public string OutSideNumber { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string StreetName { get; set; } = null!;

        public virtual ICollection<Customer_Address> Customer_Addresses { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
