using System;
using System.Collections.Generic;

namespace ZofyaApi.Models
{
    public partial class WishList
    {
        public WishList()
        {
            Item_WishLists = new HashSet<Item_WishList>();
        }

        public int IDWishList { get; set; }
        public string Name { get; set; } = null!;
        public int IDUser { get; set; }

        public virtual Customer IDUserNavigation { get; set; } = null!;
        public virtual ICollection<Item_WishList> Item_WishLists { get; set; }
    }
}
