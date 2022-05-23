using System;
using System.Collections.Generic;

namespace ZofyaApi.Models
{
    public partial class Item_Image
    {
        public string SKU { get; set; } = null!;
        public string ImageURL { get; set; } = null!;

        public virtual Item SKUNavigation { get; set; } = null!;
    }
}
