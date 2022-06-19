using System;
using System.Collections.Generic;

namespace ZofyaApi.Models
{
    public partial class Item_Color
    {
        public int IDItem_Color { get; set; }
        public string SKU { get; set; } = null!;
        public string Color { get; set; } = null!;

        public virtual Item SKUNavigation { get; set; } = null!;
    }
}
