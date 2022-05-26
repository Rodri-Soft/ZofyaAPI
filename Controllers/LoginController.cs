using Microsoft.AspNetCore.Mvc;
using ZofyaApi.Models;
using System;
using System.Collections.Generic;

namespace ZofyaApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{

    public class CustomerData {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string Phone { get; set; }
        public string Agree {get; set;}
    }

    public class CustomerLogIn{
        public string Email { get; set; }        
        public string Password { get; set; }        
    }


    public class Result {
        public Boolean correct {get; set;}
        public List<String>? message{get; set;}
        public List<String>? field {get; set;}
        public Customer? logInCustomer {get; set;}
        
    }

    private ZofyaContext dbContext;
    private Log log = new Log();

    public LoginController(ZofyaContext dbContext){
        this.dbContext = dbContext;
    }

    [HttpPost]    
    [Route("/Registration")]
    public Result AddCustomer(CustomerData customerData) {

        List<String> errorMessages = new List<String>();        

        try
        {               
            Result result = new Result();

            List<String> errorFields = new List<String>();

            bool isCorrect = true;        
            bool exist = dbContext.Customers.Where(c => c.Email.Equals(customerData.Email)).Count() > 0;
            
            if(exist)
            {
                isCorrect = false;
                result.correct = false;
                errorMessages.Add("There is already an account registered with that email");
                errorFields.Add("email");                
            }

            string password = customerData.Password;
            string rePassword = customerData.RePassword;

            if(!(password.Equals(rePassword)))
            {
                isCorrect = false;                
                result.correct = false;
                errorMessages.Add("Passwords do not match");
                errorFields.Add("rePassword");                
            }           

            if(customerData.Agree.Equals("false"))
            {
                isCorrect = false;                
                result.correct = false;
                errorMessages.Add("It is necessary to accept the terms and conditions");
                errorFields.Add("agree");                
            }

            if (isCorrect)
            {                

                Customer customer = new Customer();
                customer.Email = customerData.Email;
                customer.FullName = customerData.FullName;
                customer.Phone = customerData.Phone;                

                string customerPassword = Encrypt.GetSHA256(customerData.Password);
                customer.Password = customerPassword;

                dbContext.Customers.Add(customer);
                dbContext.SaveChanges();
                
                ShoppingCart shoppingCart = new ShoppingCart();
                shoppingCart.IsEmpty = true;
                shoppingCart.TotalBalance = 0;                                

                var customerShoppingCart = (from cust in dbContext.Customers
                                            where cust.Email == customerData.Email
                                            select cust).FirstOrDefault();       
                
                if (customerShoppingCart != null)
                {
                    shoppingCart.IDUser = customerShoppingCart.IDUser;

                    WishList customerWishList = new WishList();
                    customerWishList.Name = "Favorites";
                    customerWishList.IDUser = customerShoppingCart.IDUser;
                    
                    dbContext.ShoppingCarts.Add(shoppingCart);
                    dbContext.WishLists.Add(customerWishList);
                    dbContext.SaveChanges();

                    List<String> successMessage = new List<String>();
                    successMessage.Add("The account has been successfully registered");

                    result.correct = true;  
                    result.message = successMessage;

                }
                else
                {
                    result.correct = false;
                }
                
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

    [HttpPost]    
    [Route("/LogIn")]
    public Result LogIn(CustomerLogIn customerLogIn) {

        List<String> errorMessages = new List<String>();        

        try
        {               
            Result result = new Result();            

            bool isCorrect = true;      

            string customerPassword = Encrypt.GetSHA256(customerLogIn.Password);  
            
            bool existEmail = dbContext.Customers.Where(c => c.Email == customerLogIn.Email &&
                                                            c.Password == customerPassword).Count() > 0;
                                                
            if (!existEmail)
            {
                isCorrect = false;
                result.correct = false;
                errorMessages.Add("Invalid nickname and/or password");
            }                                                  

            if (isCorrect)
            {

                var customer = (from cust in dbContext.Customers
                               where cust.Email == customerLogIn.Email
                               select cust).FirstOrDefault();                                                

                result.correct = true;           

                List<String> successMessage = new List<String>();
                successMessage.Add("Successful login");
                result.message = successMessage;

                result.logInCustomer = customer;       


            } 
            else
            {
                result.correct = false;     
                result.message = errorMessages;                          
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

}