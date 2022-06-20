using ZofyaApi.Models;
using Microsoft.AspNetCore.Mvc;
using ZofyaApi.ModelValidations;
using Microsoft.EntityFrameworkCore;

namespace ZofyaApi.Controllers;

[ApiController]
[Route("controller")]
public class ClientController : ControllerBase {
  private ZofyaContext dbContext;
  private Log log = new Log();

  public ClientController(ZofyaContext dbContext) {
    this.dbContext = dbContext;
  }

  [HttpPost]
  [Route("/RegisterWishList")]
  public Result RegisterWishList(WishListData wishListData) {
    List<string> errorMessages = new();
    List<string> errorFields = new();

    try
    {
      Result result = new();
      bool isCorrect = true;

      WishListData wishListValidation = new();

      Result nameResult = wishListValidation.ValidateWishlistName(wishListData.Name);
      if (!nameResult.correct) 
      { 
        isCorrect = false;
        result.correct = false;

        if (nameResult.message != null) 
        {
          errorMessages.Add(nameResult.message[0]);
        }

        errorFields.Add("Wishlist Name");
      }

      if (isCorrect)
      {
        WishList wishList = new WishList {
          IDUser = wishListData.IDUser,
          Name = wishListData.Name
        };

        dbContext.WishLists.Add(wishList);
        dbContext.SaveChanges();

        List<String> successMessage = new List<String>();
        successMessage.Add("The Wishlist has been successfully registered");

        result.correct = true;
        result.message = successMessage;
      }
      else 
      {
        result.correct = false;
        result.message = errorMessages;
        result.field = errorFields;
      }

      return result;
    }
    catch (Exception e)
    {
      Result result = new Result();
      result.correct = false;

      errorMessages.Clear();
      log.Add(e.ToString());
      errorMessages.Add("Internal Server Error");

      result.message = errorMessages;
      return result;
    }
  }

  [HttpDelete]
  [Route("/DeleteWishList/{idWishList}")]
  public Result DeleteWishList(int idWishList) {
    Result result =  new();
    List<string> errorMessages = new();

    try
    {
      WishList wishList = dbContext.WishLists.Where(wishlist => wishlist.IDWishList == idWishList).First();

      dbContext.WishLists.Remove(wishList);
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
  [Route("/PostItemWishList")]
  public List<Item_WishList> PostItemWishlist(IDResult idUser)
  {    
    return dbContext.Item_WishLists.ToList();
  }

  [HttpGet]
  [Route("/Items/Wishlist/{idWishList}")]
  public List<AuxiliaryItemWishList> GetItemsWishList(int idWishList) {
    List<AuxiliaryItemWishList> auxiliaryItemWishLists = new();

    List<Item_WishList> item_WishLists = dbContext.Item_WishLists.Where(item =>
      item.IDWishList == idWishList).ToList();

    List<Item> items = dbContext.Items.ToList();

    foreach (Item_WishList item_WishList in item_WishLists)
    {
      foreach (Item item in items)
      {
        if (item_WishList.SKU.Equals(item.SKU)) {
          string mainImage = (dbContext.Item_Images.Where(i => 
            i.SKU.Equals(item.SKU)).ToList())[0].ImageURL;
          string mainColor = (dbContext.Item_Colors.Where(i => 
            i.SKU.Equals(item.SKU)).ToList())[0].Color;

          auxiliaryItemWishLists.Add(new AuxiliaryItemWishList {
            IDItemWishList = item_WishList.IDItemWishList,
            SKU = item.SKU,
            Price = item.Price,
            Name = item.Name,
            ImageURL  = mainImage,
            Color = mainColor    
          });
        }
      }
    }

    return auxiliaryItemWishLists;
  }

  [HttpDelete]
  [Route("/DeleteItemWishList/{idItemWishList}")]
  public Result DeleteItemWishList(int idItemWishList) {
    Result result = new();
    List<string> errorMessages = new();

    try
    {
      Item_WishList item_WishList = dbContext.Item_WishLists.Where(item => item.IDItemWishList == idItemWishList).First();

      dbContext.Item_WishLists.Remove(item_WishList);
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
  [Route("/PostUserOrder")]
  public List<Order> PostUserOrder(IDResult idUser) {
    int id = int.Parse(idUser.ID);

    return dbContext.Orders.Where(order => order.IDUser == id).ToList();
  }
}