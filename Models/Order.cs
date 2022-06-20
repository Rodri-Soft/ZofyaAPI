using System;
using System.Collections.Generic;

namespace ZofyaApi.Models
{
    public partial class Order
    {
        public int IDOrder { get; set; }
        public string Date { get; set; } = null!;
        public string DeliveryDate { get; set; } = null!;
        public string OrderNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
        public decimal TotalToPay { get; set; }
        public int IDUser { get; set; }
        public int IDAddress { get; set; }

        public virtual Address IDAddressNavigation { get; set; } = null!;
        public virtual Customer IDUserNavigation { get; set; } = null!;
    }
}
