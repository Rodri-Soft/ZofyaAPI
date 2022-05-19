using System;
using System.Collections.Generic;

namespace ZofyaApi.Models
{
    public partial class Customer_Address
    {
        public int IDCustomerAddress { get; set; }
        public int IDUser { get; set; }
        public int IDAddress { get; set; }

        public virtual Address IDAddressNavigation { get; set; } = null!;
        public virtual Customer IDUserNavigation { get; set; } = null!;
    }
}
