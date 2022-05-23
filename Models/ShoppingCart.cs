using System;
using System.Collections.Generic;

namespace ZofyaApi.Models
{
    public partial class ShoppingCart
    {
        public ShoppingCart()
        {
            Customers = new HashSet<Customer>();
            ItemShoppingCarts = new HashSet<ItemShoppingCart>();
        }

        public int IDShoppingCart { get; set; }
        public bool? IsEmpty { get; set; }
        public decimal? TotalBalance { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<ItemShoppingCart> ItemShoppingCarts { get; set; }
    }
}
