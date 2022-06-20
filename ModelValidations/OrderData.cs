public class OrderData
{
  public int IDOrder { get; set; }
  public string Date { get; set; } = null!;
  public string DeliveryDate { get; set; } = null!;
  public string OrderNumber { get; set; } = null!;
  public string Status { get; set; } = null!;
  public decimal TotalToPay { get; set; }
  public int IDUser { get; set; }
  public int IDAddress { get; set; }
  public int IDShoppingCart { get; set; }
}