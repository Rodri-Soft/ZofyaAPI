using ZofyaApi.Models;
using Microsoft.AspNetCore.Mvc;
using ZofyaApi.ModelValidations;
using Microsoft.EntityFrameworkCore;

namespace ZofyaApi.Controllers;

[ApiController]
[Route("controller")]
public class CartController : ControllerBase {
  private ZofyaContext dbContext;
  private Log log = new Log();

  public CartController(ZofyaContext dbContext) {
    this.dbContext = dbContext;
  }

  [HttpPost]
  [Route("/PostUserItemShoppingCart")]
  public List<AuxiliaryItemShoppingCart> PostUserItemShoppingCart(IDResult idUser) {    
    int id = int.Parse(idUser.ID);
    int idShoppingCart = (dbContext.ShoppingCarts.Where(cart => 
      cart.IDUser == id).First()).IDShoppingCart;

    List<AuxiliaryItemShoppingCart> auxiliaryItemShoppingCarts = new();
    List<ItemShoppingCart> itemShoppingCarts = dbContext.ItemShoppingCarts.Where(item =>
      item.IDShoppingCart == idShoppingCart && !item.isOnOrder).ToList();    

    foreach (ItemShoppingCart itemShoppingCart in itemShoppingCarts)
    {      
      string name = (dbContext.Items.Where(item => item.SKU.Equals(itemShoppingCart.SKU)).First()).Name;
      string mainImage = (dbContext.Item_Images.Where(item => 
        item.SKU.Equals(itemShoppingCart.SKU)).ToList())[0].ImageURL;
      string mainColor = (dbContext.Item_Colors.Where(item => 
        item.SKU.Equals(itemShoppingCart.SKU)).ToList())[0].Color;

      auxiliaryItemShoppingCarts.Add(new AuxiliaryItemShoppingCart
      {
        IDItemShoppingCart = itemShoppingCart.IDItemShoppingCart,
        IDShoppingCart = itemShoppingCart.IDShoppingCart,
        SKU = itemShoppingCart.SKU,
        QuantityOfItems = itemShoppingCart.QuantityOfItems,
        TotalItem = itemShoppingCart.TotalItem,
        SizeSelected = itemShoppingCart.SizeSelected,
        Name = name,
        ImageURL = mainImage,
        Color = mainColor
      });
    }

    return auxiliaryItemShoppingCarts;
  }

  [HttpDelete]
  [Route("/DeleteItemShoppingCart/{idItemShoppingCart}")]
  public Result DeleteItemShoppingCart(int idItemShoppingCart) {
    Result result = new();
    List<string> errorMessages = new();

    try
    {
      ItemShoppingCart itemShoppingCart = dbContext.ItemShoppingCarts.Where(item =>
        (item.IDItemShoppingCart == idItemShoppingCart) && !item.isOnOrder).First();
    
      ShoppingCart shoppingCart = dbContext.ShoppingCarts.Where(cart =>
        cart.IDShoppingCart == itemShoppingCart.IDShoppingCart).First();

      bool IsEmpty = dbContext.ItemShoppingCarts.Where(item => 
        item.IDShoppingCart == idItemShoppingCart).ToList().Count == 0;

      if (IsEmpty) shoppingCart.IsEmpty = true;
      
      shoppingCart.TotalBalance -= itemShoppingCart.TotalItem;
      dbContext.ItemShoppingCarts.Remove(itemShoppingCart);
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

  [HttpPatch]
  [Route("/ChangeItemQuantityById/{idItemShoppingCart}/{isIncreasing}")]
  public Result ChangeItemQuantityById(int idItemShoppingCart, bool isIncreasing) {
    List<String> errorMessages = new();
    Result result = new();

    try
    {
      ItemShoppingCart itemShoppingCart = dbContext.ItemShoppingCarts.Where(item =>
        (item.IDItemShoppingCart == idItemShoppingCart) && !item.isOnOrder).First();

      decimal itemPrice = (dbContext.Items.Where(item => 
        item.SKU.Equals(itemShoppingCart.SKU)).FirstOrDefault()).Price;
      
      if (itemShoppingCart != null)
      {
        ShoppingCart shoppingCart = dbContext.ShoppingCarts.Where(cart => 
          cart.IDShoppingCart == itemShoppingCart.IDShoppingCart).First();

        if (isIncreasing)
        {
          itemShoppingCart.QuantityOfItems += 1;
          itemShoppingCart.TotalItem += itemPrice;        
          shoppingCart.TotalBalance += itemPrice;
        }
        else 
        {
          if (itemShoppingCart.QuantityOfItems == 1)
          {
            dbContext.ItemShoppingCarts.Remove(itemShoppingCart);
            shoppingCart.IsEmpty = true;
          } 
          else 
          {
            itemShoppingCart.QuantityOfItems -= 1;
            itemShoppingCart.TotalItem -= itemPrice;    
            dbContext.Entry(itemShoppingCart).State = EntityState.Modified;
          }          
          shoppingCart.TotalBalance -= itemPrice;
        }

        dbContext.SaveChanges();
        result.correct = true;        
      }
      else
      {
        errorMessages.Add("ItemShoppingCart Not Found");
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

  [HttpPost]
  [Route("/RegisterCustomerAddress")]
  public Result RegisterCustomerAddress(AddressData addressData) {
    List<string> errorMessages = new();
    List<string> errorFields = new();

    try
    {
      Result result = new();
      bool isCorrect = true;
      AddressData addressDataValidation = new();

      Result streetNameResult = addressDataValidation.ValidateStreetName(addressData.StreetName); 
      if (!streetNameResult.correct) 
      { 
        isCorrect = false;
        result.correct = false;

        if (streetNameResult.message != null) 
        {
          errorMessages.Add(streetNameResult.message[0]);
        }

        errorFields.Add("Street Name");
      }

      Result colonyResult = addressDataValidation.ValidateColony(addressData.Colony); 
      if (!colonyResult.correct) 
      { 
        isCorrect = false;
        result.correct = false;

        if (colonyResult.message != null) 
        {
          errorMessages.Add(colonyResult.message[0]);
        }

        errorFields.Add("Colony");
      } 

      Result outSideResult = addressDataValidation.ValidateOutSideNumber(addressData.OutSideNumber); 
      if (!outSideResult.correct) 
      { 
        isCorrect = false;
        result.correct = false;

        if (outSideResult.message != null) 
        {
          errorMessages.Add(outSideResult.message[0]);
        }

        errorFields.Add("Out Side Number");
      }
       
      Result insideResult = addressDataValidation.ValidateOutSideNumber(addressData.InsideNumber); 
      if (!insideResult.correct) 
      { 
        isCorrect = false;
        result.correct = false;

        if (insideResult.message != null) 
        {
          errorMessages.Add(insideResult.message[0]);
        }

        errorFields.Add("Insider Number");
      }

      Result postalCodeResult = addressDataValidation.ValidatePostalCode(addressData.PostalCode); 
      if (!postalCodeResult.correct) 
      { 
        isCorrect = false;
        result.correct = false;

        if (postalCodeResult.message != null) 
        {
          errorMessages.Add(postalCodeResult.message[0]);
        }

        errorFields.Add("Postal Code");
      }

      Result cityResult = addressDataValidation.ValidateCity(addressData.City); 
      if (!cityResult.correct) 
      { 
        isCorrect = false;
        result.correct = false;

        if (cityResult.message != null) 
        {
          errorMessages.Add(cityResult.message[0]);
        }

        errorFields.Add("City");
      }

      if (isCorrect)
      {
        Address address = new Address {
          StreetName = addressData.StreetName,
          Colony = addressData.Colony,
          OutSideNumber = addressData.OutSideNumber,
          InsideNumber = addressData.InsideNumber,
          PostalCode = addressData.PostalCode,
          City = addressData.City
        };

        dbContext.Addresses.Add(address);
        dbContext.SaveChanges();

        List<String> successMessage = new List<String>();
        successMessage.Add("The Address has been successfully registered");

        int idAddess = dbContext.Addresses.Where(a => 
          a.StreetName.Equals(address.StreetName)).Max(a => 
            address.IDAddress);

        Customer_Address customer_Address = new Customer_Address {          
          IDUser = addressData.IDCustomer,
          IDAddress = idAddess
        };

        dbContext.Customer_Addresses.Add(customer_Address);
        dbContext.SaveChanges();

        result.correct = true;
        result.message = successMessage;
      }
      else {
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

  [HttpPost]
  [Route("/PostCustomerAddress")]
  public List<Address> PostCustomerAddress(IDResult idUser) {
    int id = int.Parse(idUser.ID);

    List<Address> allAddresses = dbContext.Addresses.ToList();
    List<Address> addresses = new();

    List<Customer_Address> customer_Addresses = dbContext.Customer_Addresses.Where(addressCustomer =>
    addressCustomer.IDUser == id).ToList();

    foreach (Address address in allAddresses)
    {
      foreach (Customer_Address customer_Address in customer_Addresses)
      {
        if ((customer_Address.IDAddress == address.IDAddress) && (customer_Address.IDUser == id)) {
          addresses.Add(address);
        }
      }
    }

    return  addresses;
  }

  [HttpPost]
  [Route("/PostCustomerOrder")]
  public Result PostCustomerOrder(OrderData orderData) {
    List<String> errorMessages = new();
    Result result = new();

    try
    {
      Order order = new Order {
        Date = orderData.Date,
        DeliveryDate = orderData.DeliveryDate,
        OrderNumber = orderData.OrderNumber,
        Status = "In Process",
        TotalToPay = orderData.TotalToPay,
        IDUser = orderData.IDUser,
        IDAddress = orderData.IDAddress
      };

      dbContext.Orders.Add(order);

      ShoppingCart shoppingCart = dbContext.ShoppingCarts.Where(shoppingCart =>
        shoppingCart.IDShoppingCart == orderData.IDShoppingCart).First();
      
      List<ItemShoppingCart> itemShoppingCarts = dbContext.ItemShoppingCarts.Where(item =>
        item.IDShoppingCart == orderData.IDShoppingCart).ToList();

      foreach (ItemShoppingCart item in itemShoppingCarts)
      {
        item.isOnOrder = true;
      }

      shoppingCart.IsEmpty = true;
      shoppingCart.TotalBalance = 0;
      result.correct = true;
      
      dbContext.SaveChanges();
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