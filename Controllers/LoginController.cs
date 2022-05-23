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


    public class Result {
        public Boolean correct {get; set;}
        public List<String>? message{get; set;}
        public List<String>? field {get; set;}
        
    }

    private ZofyaContext dbContext;

    public LoginController(ZofyaContext dbContext){
        this.dbContext = dbContext;
    }

    [HttpPost]    
    public Result AddCustomer(CustomerData customerData) {

        List<String> errorMessages = new List<String>();        

        try
        {               
            Result result = new Result();

            List<String> errorFields = new List<String>();

            var isCorrect = true;        
            var exist = dbContext.Customers.Where(c => c.Email.Equals(customerData.Email)).Count() > 0;
            
            if(exist){
                isCorrect = false;
                result.correct = false;
                errorMessages.Add("There is already an account registered with that email");
                errorFields.Add("email");                
            }

            string password = customerData.Password;
            string rePassword = customerData.RePassword;

            if(!(password.Equals(rePassword))){
                isCorrect = false;                
                result.correct = false;
                errorMessages.Add("Passwords do not match");
                errorFields.Add("rePassword");                
            }           

            if(customerData.Agree.Equals("false")){
                isCorrect = false;                
                result.correct = false;
                errorMessages.Add("It is necessary to accept the terms and conditions");
                errorFields.Add("agree");                
            }

            if (isCorrect){

                Customer customer = new Customer();
                customer.Email = customerData.Email;
                customer.FullName = customerData.FullName;
                customer.Password = customerData.Password;
                customer.Phone = customerData.Phone;

                dbContext.Customers.Add(customer);
                dbContext.SaveChanges();

                List<String> successMessage = new List<String>();
                successMessage.Add("The account has been successfully registered");

                result.correct = true;  
                result.message = successMessage;


            } else{
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
            errorMessages.Add(e.ToString());            

            result.message = errorMessages;        
            return result;            
        }
        
    }

}