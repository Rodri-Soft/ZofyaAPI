using System.Text.RegularExpressions;
using ZofyaApi.ModelValidations;

public class ItemWishListData {
  public int IDItemWishList { get; set; }
  public int IDWishList { get; set; }
  public string SKU { get; set; } = null!;
}

