using System;
using System.Collections.Generic;

namespace ZofyaApi.ModelValidations
{
  public class AuxiliaryItemWishList
  {
    public int IDItemWishList { get; set; }
    public string SKU { get; set; } = null!;
    public decimal Price { get; set; }
    public string Name { get; set; } = null!;
    public string ImageURL { get; set; } = null!;
    public string Color { get; set; } = null!;
  }
}
