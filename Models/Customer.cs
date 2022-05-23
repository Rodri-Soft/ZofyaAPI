using System;
using System.Collections.Generic;

namespace ZofyaApi.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Customer_Addresses = new HashSet<Customer_Address>();
            Orders = new HashSet<Order>();
            WishLists = new HashSet<WishList>();
        }

        public int IDUser { get; set; }
        public DateTime? DateOfBith { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int? IDShoppingCart { get; set; }

        public virtual ShoppingCart? IDShoppingCartNavigation { get; set; }
        public virtual ICollection<Customer_Address> Customer_Addresses { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
    }
}
