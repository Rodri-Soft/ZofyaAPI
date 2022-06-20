using System;
using System.Collections.Generic;

namespace ZofyaApi.ModelValidations
{
  public class AuxiliaryItemShoppingCart
  {
    public int IDItemShoppingCart { get; set; }
    public int IDShoppingCart { get; set; }
    public string SKU { get; set; } = null!;
    public int QuantityOfItems { get; set; }
    public decimal TotalItem { get; set; }
    public string SizeSelected { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ImageURL { get; set; } = null!;
    public string Color { get; set; } = null!;
  }
}
