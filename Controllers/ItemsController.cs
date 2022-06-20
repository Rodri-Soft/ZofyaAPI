using ZofyaApi.Models;
using Microsoft.AspNetCore.Mvc;
using ZofyaApi.ModelValidations;
using Microsoft.EntityFrameworkCore;

namespace ZofyaApi.Controllers;

[ApiController]
[Route("controller")]
public class ItemsController : ControllerBase {
  private ZofyaContext dbContext;
  private Log log = new Log();

  public ItemsController(ZofyaContext dbContext) {
    this.dbContext = dbContext;
  }

  protected List<string> GetItemSizes(string sku) {
    List<string> sizes = new();
    List<Item_Size> item_Sizes = dbContext.Item_Sizes.Where(size => size.SKU.Equals(sku)).ToList();

    foreach (var size in item_Sizes)
    {
      sizes.Add(size.Size);
    }

    return sizes;
  }

  protected List<string> GetItemColors(string sku) {
    List<string> colors = new();
    List<Item_Color> item_Colors = dbContext.Item_Colors.Where(color => color.SKU.Equals(sku)).ToList();

    foreach (var color in item_Colors)
    {
      colors.Add(color.Color);
    }

    return colors;
  }

  protected List<string> GetItemImages(string sku) {
    List<string> images = new();
    List<Item_Image> item_Images = dbContext.Item_Images.Where(image => image.SKU.Equals(sku)).ToList();

    foreach (var image in item_Images)
    {
      images.Add(image.ImageURL);
    }

    return images;
  }

  [HttpGet]
  [Route("/Items/Woman/Blouses")]
  public List<AuxiliaryItem> GetWomanBlouses()
  {
    List<AuxiliaryItem> auxiliaryItems = new();
    List<Item> items = dbContext.Items.Where(item => 
      item.Gender.Equals("Feminine") && 
      item.Status.Equals("Available") && 
      item.Category.Equals("Blouse")
    ).ToList();

    foreach (var item in items)
    {
      auxiliaryItems.Add(new AuxiliaryItem 
      {
        SKU = item.SKU,
        Description = item.Description,
        Discount = item.Discount, 
        Name = item.Name,
        Price = item.Price,
        Category = item.Category,
        Gender = item.Gender,
        Status = item.Status,
        Stock = item.Stock,
        Care = item.Care,
        Sizes = new List<string>(),
        Colors = new List<string>(),
        Images = new List<string>()
      });             
    }
    
    foreach (var item in auxiliaryItems)
    {
      item.Sizes = GetItemSizes(item.SKU);
      item.Colors = GetItemColors(item.SKU);
      item.Images = GetItemImages(item.SKU);
    }
    
    return auxiliaryItems;
  }

  [HttpGet]
  [Route("/Item/Woman/{sku}")]
  public AuxiliaryItem GetItemInformation(string sku) {
    Item? item = dbContext.Items.Find(sku);
    AuxiliaryItem auxiliaryItem = new();

    if (item != null) {
      auxiliaryItem.SKU = item.SKU;
      auxiliaryItem.Description = item.Description;
      auxiliaryItem.Discount = item.Discount;
      auxiliaryItem.Name = item.Name;
      auxiliaryItem.Price = item.Price;
      auxiliaryItem.Category = item.Category;
      auxiliaryItem.Gender = item.Gender;
      auxiliaryItem.Status = item.Status;
      auxiliaryItem.Stock = item.Stock;
      auxiliaryItem.Care = item.Care;
      auxiliaryItem.Sizes = GetItemSizes(item.SKU);
      auxiliaryItem.Colors = GetItemColors(item.SKU);
      auxiliaryItem.Images = GetItemImages(item.SKU);
    }

    return auxiliaryItem;
  }

  [HttpPost]
  [Route("/PostUserWishesList")]
  public List<WishList> PostUserWishesList(IDResult idUser) {    
    int id = int.Parse(idUser.ID);

    return dbContext.WishLists.Where(wishlist => 
      wishlist.IDUser.Equals(id)).ToList();
  }

  [HttpPost]
  [Route("/RegisterItemShoppingCart")]
  public Result RegisterItemShoppingCart(ItemShoppingCartData itemShoppingCartData) {
    List<String> errorMessages = new();
    Result result = new();

    try
    {
      bool existsInCart = dbContext.ItemShoppingCarts.Where(item => 
        (item.IDShoppingCart == itemShoppingCartData.IDShoppingCart) && 
        (item.SKU.Equals(itemShoppingCartData.SKU)) &&
        (item.SizeSelected.Equals(itemShoppingCartData.SizeSelected)) && !item.isOnOrder).Count() > 0;

      if (!existsInCart)
      {
        ItemShoppingCart itemShoppingCart = new ItemShoppingCart {
          IDShoppingCart = itemShoppingCartData.IDShoppingCart,
          SKU = itemShoppingCartData.SKU,
          QuantityOfItems = itemShoppingCartData.QuantityOfItems,
          TotalItem = itemShoppingCartData.TotalItem,
          SizeSelected = itemShoppingCartData.SizeSelected,
          isOnOrder = false
        };

        ShoppingCart shoppingCart = dbContext.ShoppingCarts.Where(shoppingCart =>
          shoppingCart.IDShoppingCart == itemShoppingCartData.IDShoppingCart).First();
        shoppingCart.IsEmpty = false;
        shoppingCart.TotalBalance += itemShoppingCart.TotalItem;

        dbContext.ItemShoppingCarts.Add(itemShoppingCart);
        dbContext.SaveChanges();
        result.correct = true;
      } else {
        errorMessages.Add("Repeated Item");
        result.field = errorMessages;
      }
    }
    catch (Exception e)
    {
      result.correct = false;
      log.Add(e.ToString());
      errorMessages.Add("Internal Server Error");
      result.field = errorMessages;
    } 
    
    return result;
  }

  [HttpPatch]
  [Route("/UpdateCartProductQuantity")]
  public Result UpdateCartProductQuantity(CartProductQuantityData cartProductQuantityData) {
    List<String> errorMessages = new();
    Result result = new();

    try
    {
      ItemShoppingCart itemShoppingCart = dbContext.ItemShoppingCarts.Where(item =>
        item.SKU.Equals(cartProductQuantityData.SKU) &&
        (item.IDShoppingCart == cartProductQuantityData.ShoppingCartID) && 
        (item.SizeSelected.Equals(cartProductQuantityData.Size)) && !item.isOnOrder).First();

      decimal price = dbContext.Items.Where(item => 
        item.SKU.Equals(cartProductQuantityData.SKU)).First().Price;
      itemShoppingCart.QuantityOfItems += 1;
      itemShoppingCart.TotalItem = itemShoppingCart.QuantityOfItems * price;

      ShoppingCart shoppingCart = dbContext.ShoppingCarts.Where(shoppingCart =>
      shoppingCart.IDShoppingCart == cartProductQuantityData.ShoppingCartID).First();
      shoppingCart.TotalBalance += price;

      dbContext.Entry(itemShoppingCart).State = EntityState.Modified;
      dbContext.SaveChanges();
      result.correct = true;      
    }
    catch (Exception e)
    {
      result.correct = false;
      log.Add(e.ToString());
      errorMessages.Add("Internal Server Error");      
      result.field = errorMessages;
    }

    return result;
  }

  [HttpPost]
  [Route("/RegisterItemInWishList")]
  public Result RegisterItemInWishList(ItemWishListData itemWishListData) {
    List<String> errorMessages = new();
    Result result = new();

    try
    {
      bool existsInWishList = dbContext.Item_WishLists.Where(item =>
        (item.IDWishList == itemWishListData.IDWishList) && 
        (item.SKU.Equals(itemWishListData.SKU))).Count() > 0;

      if (!existsInWishList)
      {
        Item_WishList itemWishList = new Item_WishList {
          IDWishList = itemWishListData.IDWishList,
          SKU =  itemWishListData.SKU
        };

        dbContext.Item_WishLists.Add(itemWishList);
        dbContext.SaveChanges();
        result.correct = true;
      } else {
        errorMessages.Add("Repeated Item");
        result.field = errorMessages;
      }
    }
    catch (Exception e)
    {
      result.correct = false;
      log.Add(e.ToString());
      errorMessages.Add("Internal Server Error");
      result.field = errorMessages;
    }

    return result;
  }
}
