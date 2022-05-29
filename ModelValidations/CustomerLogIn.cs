using System.ComponentModel.DataAnnotations;

namespace ZofyaApi.ModelValidations
{
    public class CustomerLogIn{
        public string Email { get; set; }        
        public string Password { get; set; }        
    }
}