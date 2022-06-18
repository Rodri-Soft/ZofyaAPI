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

    }
}

