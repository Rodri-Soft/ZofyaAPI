using Microsoft.AspNetCore.Mvc;
using ZofyaApi.Models;
using ZofyaApi.ModelValidations;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ZofyaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagementController : ControllerBase
    {

        private ZofyaContext dbContext;
        private Log log = new Log();

        public ManagementController(ZofyaContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("/PostFindStaff")]
        public staff? PostFindStaff(AuxiliaryUser auxiliaryUser)
        {
            return dbContext.staff.Where(s => s.Email == auxiliaryUser.Email &&
                                         s.Password == auxiliaryUser.Password).FirstOrDefault();
        }    

        [HttpPost]
        [Route("/PostFindStaffEmail")]
        public staff? PostFindStaffEmail(IDResult idEmail)
        {
            return dbContext.staff.Where(s => s.Email == idEmail.ID).FirstOrDefault();
        }     

        [HttpPost]
        [Route("/PostFindItemSKU")]
        public AuxiliaryAdministrationAddItem? PostFindItemSKU(IDResult sku)
        {

            var item = dbContext.Items.Where(i => i.SKU == sku.ID).FirstOrDefault();

            AuxiliaryAdministrationAddItem auxiliaryItem = new AuxiliaryAdministrationAddItem();
            auxiliaryItem.SKU = item.SKU;
            auxiliaryItem.Description = item.Description;
            auxiliaryItem.Name = item.Name;
            auxiliaryItem.Price = item.Price;
            auxiliaryItem.Category = item.Category;
            auxiliaryItem.Status = item.Status;
            auxiliaryItem.Stock = item.Stock;
            auxiliaryItem.Gender = item.Gender;
            auxiliaryItem.Care = item.Care;

            var colorsList = dbContext.Item_Colors.Where(i => i.SKU == sku.ID).ToList();
            List<string> colors = new List<string>();
            foreach (var color in colorsList)
            {
                colors.Add(color.Color);                
            }

            var imagesList = dbContext.Item_Images.Where(i => i.SKU == sku.ID).ToList();
            List<string> images = new List<string>();
            foreach (var image in imagesList)
            {
                images.Add(image.ImageURL);                
            }

            var sizesList = dbContext.Item_Sizes.Where(i => i.SKU == sku.ID).ToList();
            List<string> sizes = new List<string>();
            foreach (var size in sizesList)
            {
                sizes.Add(size.Size);                
            }

            auxiliaryItem.Colors = colors;
            auxiliaryItem.Images = images;
            auxiliaryItem.Sizes = sizes;

            return auxiliaryItem;
        }           

        [HttpPut]
        [Route("/UpdateAdministrator")]
        public Result UpdateAdministrator(AuxiliaryStaff auxiliaryStaff)
        {

            List<String> errorMessages = new List<String>();            
            Result result = new Result();
            List<String> errorFields = new List<String>();
            StaffData staffData = new StaffData();            
            
            switch (auxiliaryStaff.Field)
            {

                case "rfc":                                                   
                                        
                    
                    Result rfcResult = staffData.validateRFC(auxiliaryStaff.Value);

                    if (!rfcResult.correct)
                    {
                        
                        result.correct = false;

                        if (rfcResult.message != null)
                        {
                            errorMessages.Add(rfcResult.message[0]);
                        }

                        errorFields.Add("rfc");

                        break;
                    }

                    try
                    {
                        bool exist = dbContext.staff.Where(st => st.RFC.Equals(auxiliaryStaff.Value)).Count() > 0;

                        if (exist)
                        {
                            
                            result.correct = false;
                            errorMessages.Add("There is already an account registered with that RFC");
                            errorFields.Add("rfc");

                            break;
                        }                                        
                                            
                        var staffDB = dbContext.staff.FirstOrDefault(
                            s => s.RFC.Equals(auxiliaryStaff.PrimaryKeyRFC));

                        if (staffDB != null)
                        {

                            dbContext.Remove(staffDB);
                            dbContext.SaveChanges();
                            
                            staffDB.RFC = auxiliaryStaff.Value;                           

                            dbContext.staff.Add(staffDB);                            
                            dbContext.SaveChanges();

                            List<String> successMessage = new List<String>();
                            successMessage.Add("The RFC has been successfully updated");

                            result.correct = true;
                            result.message = successMessage;
                            return result;

                        }
                        else
                        {
                            result.correct = false;
                            errorMessages.Add("No admin found with that RFC");
                            errorFields.Add("rfc");
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Result exceptionResult = new Result();
                        exceptionResult.correct = false;

                        errorMessages.Clear();
                        log.Add(e.ToString());
                        errorMessages.Add("Internal Server Error");                        

                        exceptionResult.message = errorMessages;
                        return exceptionResult;
                    }
                    
                
                case "curp":             

                    
                    Result curpResult = staffData.validateCURP(auxiliaryStaff.Value);

                    if (!curpResult.correct)
                    {
                        
                        result.correct = false;

                        if (curpResult.message != null)
                        {
                            errorMessages.Add(curpResult.message[0]);
                        }

                        errorFields.Add("curp");

                        break;
                    }

                    try
                    {                                                          
                                            
                        var staffDB = dbContext.staff.FirstOrDefault(
                            s => s.RFC.Equals(auxiliaryStaff.PrimaryKeyRFC));

                        if (staffDB != null)
                        {                            
                            
                            staffDB.CURP = auxiliaryStaff.Value;                           
                            
                            dbContext.staff.Update(staffDB);
                            dbContext.SaveChanges();

                            List<String> successMessage = new List<String>();
                            successMessage.Add("The CURP has been successfully updated");

                            result.correct = true;
                            result.message = successMessage;
                            return result;

                        }
                        else
                        {
                            result.correct = false;
                            errorMessages.Add("No admin found with that RFC");
                            errorFields.Add("rfc");
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Result exceptionResult = new Result();
                        exceptionResult.correct = false;

                        errorMessages.Clear();
                        log.Add(e.ToString());
                        errorMessages.Add("Internal Server Error");                        

                        exceptionResult.message = errorMessages;
                        return exceptionResult;
                    }

                case "email":             

                    
                    Result emailResult = staffData.validateEmail(auxiliaryStaff.Value);

                    if (!emailResult.correct)
                    {
                        
                        result.correct = false;

                        if (emailResult.message != null)
                        {
                            errorMessages.Add(emailResult.message[0]);
                        }

                        errorFields.Add("email");

                        break;
                    }

                    try
                    {                  
                        bool exist = dbContext.staff.Where(st => st.Email.Equals(auxiliaryStaff.Value)).Count() > 0;

                        if (exist)
                        {
                            
                            result.correct = false;
                            errorMessages.Add("There is already an account registered with that Email");
                            errorFields.Add("email");

                            break;
                        }                                                 
                                            
                        var staffDB = dbContext.staff.FirstOrDefault(
                            s => s.RFC.Equals(auxiliaryStaff.PrimaryKeyRFC));

                        if (staffDB != null)
                        {                            
                            
                            staffDB.Email = auxiliaryStaff.Value;                           
                            
                            dbContext.staff.Update(staffDB);
                            dbContext.SaveChanges();

                            List<String> successMessage = new List<String>();
                            successMessage.Add("The Email has been successfully updated");

                            result.correct = true;
                            result.message = successMessage;
                            return result;

                        }
                        else
                        {
                            result.correct = false;
                            errorMessages.Add("No admin found with that RFC");
                            errorFields.Add("rfc");
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Result exceptionResult = new Result();
                        exceptionResult.correct = false;

                        errorMessages.Clear();
                        log.Add(e.ToString());
                        errorMessages.Add("Internal Server Error");                        

                        exceptionResult.message = errorMessages;
                        return exceptionResult;
                    }

                
                case "fullname":             

                    
                    Result fullnameResult = staffData.validateFullname(auxiliaryStaff.Value);

                    if (!fullnameResult.correct)
                    {
                        
                        result.correct = false;

                        if (fullnameResult.message != null)
                        {
                            errorMessages.Add(fullnameResult.message[0]);
                        }

                        errorFields.Add("fullname");

                        break;
                    }

                    try
                    {                                                          
                                            
                        var staffDB = dbContext.staff.FirstOrDefault(
                            s => s.RFC.Equals(auxiliaryStaff.PrimaryKeyRFC));

                        if (staffDB != null)
                        {                            
                            
                            staffDB.FullName = auxiliaryStaff.Value;                           
                            
                            dbContext.staff.Update(staffDB);
                            dbContext.SaveChanges();

                            List<String> successMessage = new List<String>();
                            successMessage.Add("The Fullname has been successfully updated");

                            result.correct = true;
                            result.message = successMessage;
                            return result;

                        }
                        else
                        {
                            result.correct = false;
                            errorMessages.Add("No admin found with that RFC");
                            errorFields.Add("rfc");
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Result exceptionResult = new Result();
                        exceptionResult.correct = false;

                        errorMessages.Clear();
                        log.Add(e.ToString());
                        errorMessages.Add("Internal Server Error");                        

                        exceptionResult.message = errorMessages;
                        return exceptionResult;
                    }

                case "phone":             

                    
                    Result phoneResult = staffData.validatePhone(auxiliaryStaff.Value);

                    if (!phoneResult.correct)
                    {
                        
                        result.correct = false;

                        if (phoneResult.message != null)
                        {
                            errorMessages.Add(phoneResult.message[0]);
                        }

                        errorFields.Add("phone");

                        break;
                    }

                    try
                    {                                                          
                                            
                        var staffDB = dbContext.staff.FirstOrDefault(
                            s => s.RFC.Equals(auxiliaryStaff.PrimaryKeyRFC));

                        if (staffDB != null)
                        {                            
                            
                            staffDB.Phone = auxiliaryStaff.Value;                           
                            
                            dbContext.staff.Update(staffDB);
                            dbContext.SaveChanges();

                            List<String> successMessage = new List<String>();
                            successMessage.Add("The Phone has been successfully updated");

                            result.correct = true;
                            result.message = successMessage;
                            return result;

                        }
                        else
                        {
                            result.correct = false;
                            errorMessages.Add("No admin found with that RFC");
                            errorFields.Add("rfc");
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Result exceptionResult = new Result();
                        exceptionResult.correct = false;

                        errorMessages.Clear();
                        log.Add(e.ToString());
                        errorMessages.Add("Internal Server Error");                        

                        exceptionResult.message = errorMessages;
                        return exceptionResult;
                    }

                default:
                    break;
            }

            result.message = errorMessages;
            result.field = errorFields;

            return result;
          
        }


        [HttpPut]
        [Route("/UpdateAdministratorPassword")]
        public Result UpdateAdministratorPassword(AuxiliaryStaffPassword auxiliaryStaffPassword)
        {

            List<String> errorMessages = new List<String>();
            Result result = new Result();
            List<String> errorFields = new List<String>();
            StaffData staffData = new StaffData();     

            try
            {

                var staffDB = dbContext.staff.FirstOrDefault(
                        s => s.RFC.Equals(auxiliaryStaffPassword.PrimaryKeyRFC));

                if (staffDB == null)
                {
                    
                    result.correct = false;
                    errorMessages.Add("No admin found with that RFC");
                    errorFields.Add("rfc");
                    
                    result.message = errorMessages;
                    result.field = errorFields;

                    return result;

                }

                string currentPassword = Encrypt.GetSHA256(auxiliaryStaffPassword.CurrentValue);
                
                if (!(currentPassword.Equals(staffDB.Password)))
                {

                    result.correct = false;
                    errorMessages.Add("Wrong current password");
                    errorFields.Add("password");
                    
                    result.message = errorMessages;
                    result.field = errorFields;

                    return result;

                }
                
                Result passwordResult = staffData.validatePassword(auxiliaryStaffPassword.NewValue);

                if (!passwordResult.correct)
                {

                    result.correct = false;

                    if (passwordResult.message != null)
                    {
                        errorMessages.Add(passwordResult.message[0]);
                    }

                    errorFields.Add("password");

                    result.message = errorMessages;
                    result.field = errorFields;

                    return result;
                }

                string newPassword = Encrypt.GetSHA256(auxiliaryStaffPassword.NewValue);
                staffDB.Password = newPassword;

                dbContext.staff.Update(staffDB);
                dbContext.SaveChanges();

                List<String> successMessage = new List<String>();
                successMessage.Add("The Password has been successfully updated");

                result.correct = true;
                result.message = successMessage;
                return result;
                                
            }
            catch (Exception e)
            {
                Result exceptionResult = new Result();
                exceptionResult.correct = false;

                errorMessages.Clear();
                log.Add(e.ToString());
                errorMessages.Add("Internal Server Error");

                exceptionResult.message = errorMessages;
                return exceptionResult;
            }


        }

        [HttpPost]
        [Route("/CustomersData")]
        public List<AuxiliaryCustomer> GetCustomersData() {
            
            return dbContext.Customers.Select(c => new AuxiliaryCustomer(){IDUser = c.IDUser,
                                                                FullName = c.FullName,
                                                                Email = c.Email,
                                                                Phone = c.Phone})
                                                                .ToList();                                                                            
        }

        [HttpPost]
        [Route("/ItemsData")]
        public List<AuxiliaryAdministrationItem> GetItemsData() {
            
            return dbContext.Items.Select(i => new AuxiliaryAdministrationItem(){SKU = i.SKU,
                                                                Description = i.Description,
                                                                Name = i.Name,
                                                                Price = i.Price,
                                                                Category = i.Category,
                                                                Status = i.Status,
                                                                Stock = i.Stock,
                                                                Gender = i.Gender,
                                                                Care = i.Care})
                                                                .ToList();                                                                            
        }

        [HttpDelete("/CustomerDelete/{email}")]
        public Result DeleteCustomer(String email)
        {
            Result result = new Result();
            List<String> errorMessages = new List<String>();
            try
            {
                var customerBD = dbContext.Customers.FirstOrDefault(c => c.Email.Equals(email));

                if (customerBD != null)
                {
                    dbContext.Remove(customerBD);
                    dbContext.SaveChanges();

                    List<String> successMessage = new List<String>();
                    successMessage.Add("The customer has been successfully deleted");

                    result.correct = true;
                    result.message = successMessage;

                    return result;
                }
                else
                {
                    result.correct = false;                    
                    errorMessages.Add("The customer does not exist in the DB");
                    result.message = errorMessages;

                    return result;
                }
            }
            catch (Exception e)
            {
                Result exceptionResult = new Result();
                exceptionResult.correct = false;

                errorMessages.Clear();
                log.Add(e.ToString());
                errorMessages.Add("Internal Server Error");

                exceptionResult.message = errorMessages;
                return exceptionResult;
            }
        }

        [HttpDelete("/ItemDelete/{sku}")]
        public Result DeleteItem(String sku)
        {
            Result result = new Result();
            List<String> errorMessages = new List<String>();
            try
            {
                var itemBD = dbContext.Items.FirstOrDefault(i => i.SKU.Equals(sku));

                if (itemBD != null)
                {
                    dbContext.Remove(itemBD);
                    dbContext.SaveChanges();

                    List<String> successMessage = new List<String>();
                    successMessage.Add("The item has been successfully deleted");

                    result.correct = true;
                    result.message = successMessage;

                    return result;
                }
                else
                {
                    result.correct = false;                    
                    errorMessages.Add("The item does not exist in the DB");
                    result.message = errorMessages;

                    return result;
                }
            }
            catch (Exception e)
            {
                Result exceptionResult = new Result();
                exceptionResult.correct = false;

                errorMessages.Clear();
                log.Add(e.ToString());
                errorMessages.Add("Internal Server Error");

                exceptionResult.message = errorMessages;
                return exceptionResult;
            }
        }

        [HttpPut]
        [Route("/UpdateItemDelete")]
        public Result UpdateAdministratorPassword(IDResult sku)
        {

            Result result = new Result();
            List<String> errorMessages = new List<String>();
            try
            {
                var itemBD = dbContext.Items.FirstOrDefault(i => i.SKU.Equals(sku.ID));

                if (itemBD != null)
                {
                    itemBD.Status = "Deleted";
                    
                    dbContext.Items.Update(itemBD);
                    dbContext.SaveChanges();

                    List<String> successMessage = new List<String>();
                    successMessage.Add("The item has been successfully deleted");

                    result.correct = true;
                    result.message = successMessage;

                    return result;
                }
                else
                {
                    result.correct = false;                    
                    errorMessages.Add("The item does not exist in the DB");
                    result.message = errorMessages;

                    return result;
                }
            }
            catch (Exception e)
            {
                Result exceptionResult = new Result();
                exceptionResult.correct = false;

                errorMessages.Clear();
                log.Add(e.ToString());
                errorMessages.Add("Internal Server Error");

                exceptionResult.message = errorMessages;
                return exceptionResult;
            }


        }

        [HttpPut]
        [Route("/UpdateItem")]
        public Result UpdateItem(AuxiliaryAdministrationAddItem auxiliaryAdministrationAddItem)
        {

            Result result = new Result();
            List<String> errorMessages = new List<String>();
            try
            {
                var itemBD = dbContext.Items.FirstOrDefault(i => i.SKU.Equals(auxiliaryAdministrationAddItem.SKU));

                if (itemBD != null)
                {

                    itemBD.Description = auxiliaryAdministrationAddItem.Description;
                    itemBD.Name = auxiliaryAdministrationAddItem.Name;
                    itemBD.Price = auxiliaryAdministrationAddItem.Price;
                    itemBD.Category = auxiliaryAdministrationAddItem.Category;
                    itemBD.Status = auxiliaryAdministrationAddItem.Status;
                    itemBD.Stock = auxiliaryAdministrationAddItem.Stock;
                    itemBD.Gender = auxiliaryAdministrationAddItem.Gender;
                    itemBD.Care = auxiliaryAdministrationAddItem.Care;
       
                    dbContext.Items.Update(itemBD);
                    dbContext.SaveChanges();

                    List<string> colorsList =  auxiliaryAdministrationAddItem.Colors;       
                    List<string> imagesList =  auxiliaryAdministrationAddItem.Images;       
                    List<string> sizesList =  auxiliaryAdministrationAddItem.Sizes;    


                    var colorsListRemove = dbContext.Item_Colors.Where(
                        i => i.SKU == auxiliaryAdministrationAddItem.SKU).ToList();

                    foreach (Item_Color itemColor in colorsListRemove)
                    {
                        dbContext.Item_Colors.Remove(itemColor);
                    }

                    dbContext.SaveChanges();


                    foreach (var color in colorsList)
                    {
                        Item_Color item_Color = new Item_Color();
                        item_Color.SKU = auxiliaryAdministrationAddItem.SKU;
                        item_Color.Color = color;

                        dbContext.Item_Colors.Add(item_Color);                        
                    }
                    dbContext.SaveChanges();



                    var imagesListRemove = dbContext.Item_Images.Where(
                        i => i.SKU == auxiliaryAdministrationAddItem.SKU).ToList();

                    foreach (Item_Image itemImage in imagesListRemove)
                    {
                        dbContext.Item_Images.Remove(itemImage);
                    }

                    dbContext.SaveChanges();


                    foreach (var image in imagesList)
                    {
                        Item_Image item_Image = new Item_Image();
                        item_Image.SKU = auxiliaryAdministrationAddItem.SKU;
                        item_Image.ImageURL = image;

                        dbContext.Item_Images.Add(item_Image);                        
                    }

                    dbContext.SaveChanges();



                    var sizesListRemove = dbContext.Item_Sizes.Where(
                        i => i.SKU == auxiliaryAdministrationAddItem.SKU).ToList();

                    foreach (Item_Size itemSize in sizesListRemove)
                    {
                        dbContext.Item_Sizes.Remove(itemSize);
                    }
                    dbContext.SaveChanges();

                    foreach (var size in sizesList)
                    {
                        Item_Size item_Size = new Item_Size();
                        item_Size.SKU = auxiliaryAdministrationAddItem.SKU;
                        item_Size.Size = size;

                        dbContext.Item_Sizes.Add(item_Size);

                    }                    
                                                       
                    dbContext.SaveChanges();   


                    List<String> successMessage = new List<String>();
                    successMessage.Add("The item has been successfully updated");

                    result.correct = true;
                    result.message = successMessage;

                    return result;
                }
                else
                {
                    result.correct = false;                    
                    errorMessages.Add("The item does not exist in the DB");
                    result.message = errorMessages;

                    return result;
                }
            }
            catch (Exception e)
            {
                Result exceptionResult = new Result();
                exceptionResult.correct = false;

                errorMessages.Clear();
                log.Add(e.ToString());
                errorMessages.Add("Internal Server Error");

                exceptionResult.message = errorMessages;
                return exceptionResult;
            }


        }

        [HttpPost]
        [Route("/AddItem")]
        public Result AddItem(AuxiliaryAdministrationAddItem auxiliaryAdministrationAddItem)
        {

            List<String> errorMessages = new List<String>();

            try
            {
                Result result = new Result();

                List<String> errorFields = new List<String>();

                bool isCorrect = true;

                // Basic fields validations
                AuxiliaryAdministrationAddItem itemValidation = new AuxiliaryAdministrationAddItem();

                Result skuResult = itemValidation.validateAverageField(auxiliaryAdministrationAddItem.SKU, "SKU");

                if (!skuResult.correct)
                {
                    isCorrect = false;
                    result.correct = false;

                    if (skuResult.message != null)
                    {
                        errorMessages.Add(skuResult.message[0]);
                    }

                    errorFields.Add("sku");
                }

                Result descriptionResult = itemValidation.validateDescription(
                    auxiliaryAdministrationAddItem.Description);

                if (!descriptionResult.correct)
                {
                    isCorrect = false;
                    result.correct = false;

                    if (descriptionResult.message != null)
                    {
                        errorMessages.Add(descriptionResult.message[0]);
                    }

                    errorFields.Add("description");
                }

                Result nameResult = itemValidation.validateAverageField(auxiliaryAdministrationAddItem.Name, "Name");

                if (!nameResult.correct)
                {
                    isCorrect = false;
                    result.correct = false;

                    if (nameResult.message != null)
                    {
                        errorMessages.Add(nameResult.message[0]);
                    }

                    errorFields.Add("name");
                }

                Result priceResult = itemValidation.validatePrice(
                    auxiliaryAdministrationAddItem.Price);

                if (!priceResult.correct)
                {
                    isCorrect = false;
                    result.correct = false;

                    if (priceResult.message != null)
                    {
                        errorMessages.Add(priceResult.message[0]);
                    }

                    errorFields.Add("price");
                }

                Result stockResult = itemValidation.validateStock(
                    auxiliaryAdministrationAddItem.Stock, auxiliaryAdministrationAddItem.Status);

                if (!stockResult.correct)
                {
                    isCorrect = false;
                    result.correct = false;

                    if (stockResult.message != null)
                    {
                        errorMessages.Add(stockResult.message[0]);
                    }

                    errorFields.Add("stock");
                }

                Result careResult = itemValidation.validateCare(
                    auxiliaryAdministrationAddItem.Care);

                if (!careResult.correct)
                {
                    isCorrect = false;
                    result.correct = false;

                    if (careResult.message != null)
                    {
                        errorMessages.Add(careResult.message[0]);
                    }

                    errorFields.Add("care");
                }               


                bool exist = dbContext.Items.Where(i => i.SKU.Equals(
                    auxiliaryAdministrationAddItem.SKU)).Count() > 0;

                if (exist)
                {
                    isCorrect = false;
                    result.correct = false;
                    errorMessages.Add("There is already an item registered with that SKU");
                    errorFields.Add("sku");
                }

                List<string> colorsList =  auxiliaryAdministrationAddItem.Colors;                
                if (colorsList.Count <= 0)
                {
                    isCorrect = false;
                    result.correct = false;
                    errorMessages.Add("Colors List empty");
                    errorFields.Add("colors");
                }

                List<string> imagesList =  auxiliaryAdministrationAddItem.Images;                
                if (imagesList.Count <= 0)
                {
                    isCorrect = false;
                    result.correct = false;
                    errorMessages.Add("Images List empty");
                    errorFields.Add("images");
                }
                
                List<string> sizesList =  auxiliaryAdministrationAddItem.Sizes;                
                if (sizesList.Count <= 0)
                {
                    isCorrect = false;
                    result.correct = false;
                    errorMessages.Add("Sizes List empty");
                    errorFields.Add("sizes");
                }
                
                if (isCorrect)
                {

                    Item item = new Item();
                    item.SKU = auxiliaryAdministrationAddItem.SKU;
                    item.Description = auxiliaryAdministrationAddItem.Description;
                    item.Name = auxiliaryAdministrationAddItem.Name;
                    item.Price = auxiliaryAdministrationAddItem.Price;                   
                    item.Category = auxiliaryAdministrationAddItem.Category;                   
                    item.Status = auxiliaryAdministrationAddItem.Status;                   
                    item.Stock = auxiliaryAdministrationAddItem.Stock;                   
                    item.Gender = auxiliaryAdministrationAddItem.Gender;                   
                    item.Care = auxiliaryAdministrationAddItem.Care;                   
                    
                    dbContext.Items.Add(item);
                    dbContext.SaveChanges();

                    foreach (var color in colorsList)
                    {
                        Item_Color item_Color = new Item_Color();
                        item_Color.SKU = auxiliaryAdministrationAddItem.SKU;
                        item_Color.Color = color;

                        dbContext.Item_Colors.Add(item_Color);
                        // dbContext.SaveChanges();
                    }

                    foreach (var image in imagesList)
                    {
                        Item_Image item_Image = new Item_Image();
                        item_Image.SKU = auxiliaryAdministrationAddItem.SKU;
                        item_Image.ImageURL = image;

                        dbContext.Item_Images.Add(item_Image);
                        // dbContext.SaveChanges();
                    }

                    foreach (var size in sizesList)
                    {
                        Item_Size item_Size = new Item_Size();
                        item_Size.SKU = auxiliaryAdministrationAddItem.SKU;
                        item_Size.Size = size;

                        dbContext.Item_Sizes.Add(item_Size);
                        // dbContext.SaveChanges();
                    }                    
                                                       
                    dbContext.SaveChanges();

                    List<String> successMessage = new List<String>();
                    successMessage.Add("The item has been successfully saved");

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
                errorMessages.Add(e.ToString());

                result.message = errorMessages;
                return result;
            }

        } 


    }
}

