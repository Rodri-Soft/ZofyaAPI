public class AuxiliaryItem
{
  public string SKU { get; set; } = null!;
  public string Description { get; set; } = null!;
  public decimal Discount { get; set; }
  public string Name { get; set; } = null!;
  public decimal Price { get; set; }
  public string Category { get; set; } = null!;
  public string Gender { get; set; } = null!;
  public string Status { get; set; } = null!;
  public string Care { get; set; } = null!;
  public int Stock { get; set; }
  public List<string> Sizes { get; set; } = null!;
  public List<string> Colors { get; set; } = null!;
  public List<string> Images { get; set; } = null!;
}