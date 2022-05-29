using System.ComponentModel.DataAnnotations;
using ZofyaApi.Models;

namespace ZofyaApi.ModelValidations
{
    public class Result {
        public Boolean correct {get; set;}
        public List<String>? message{get; set;}
        public List<String>? field {get; set;}
        public Customer? logInCustomer {get; set;}
        
    }
}