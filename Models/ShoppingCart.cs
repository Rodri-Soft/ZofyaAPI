using System;
using System.Collections.Generic;

namespace ZofyaApi.Models
{
    public partial class ShoppingCart
    {
        public int IDShoppingCart { get; set; }
        public bool? IsEmpty { get; set; }
        public decimal? TotalBalance { get; set; }
        public int? IDUser { get; set; }

        public virtual Customer? IDUserNavigation { get; set; }
    }
}
