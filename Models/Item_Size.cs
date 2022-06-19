using System;
using System.Collections.Generic;

namespace ZofyaApi.Models
{
    public partial class Item_Size
    {
        public int IDItem_Sizes { get; set; }
        public string SKU { get; set; } = null!;
        public string Size { get; set; } = null!;

        public virtual Item SKUNavigation { get; set; } = null!;
    }
}
