using System;
using System.Collections.Generic;

namespace ZofyaApi.Models
{
    public partial class Item
    {
        public Item()
        {
            ItemShoppingCarts = new HashSet<ItemShoppingCart>();
            Item_Colors = new HashSet<Item_Color>();
            Item_Images = new HashSet<Item_Image>();
            Item_Sizes = new HashSet<Item_Size>();
            Item_WishLists = new HashSet<Item_WishList>();
        }

        public string SKU { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? Discount { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Category { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int Stock { get; set; }
        public string Care { get; set; } = null!;

        public virtual ICollection<ItemShoppingCart> ItemShoppingCarts { get; set; }
        public virtual ICollection<Item_Color> Item_Colors { get; set; }
        public virtual ICollection<Item_Image> Item_Images { get; set; }
        public virtual ICollection<Item_Size> Item_Sizes { get; set; }
        public virtual ICollection<Item_WishList> Item_WishLists { get; set; }
    }
}
