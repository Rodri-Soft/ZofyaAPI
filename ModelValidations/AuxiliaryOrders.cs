using System;
using System.Collections.Generic;

namespace ZofyaApi.ModelValidations
{
    public class AuxiliaryOrder
    {
        public int IDOrder { get; set; }
        public DateTime Date { get; set; }
        public DateTime DeliveryDate { get; set; }        
        public string Status { get; set; } = null!;
        public decimal TotalToPay { get; set; }
        public int IDUser { get; set; }
    }
}
