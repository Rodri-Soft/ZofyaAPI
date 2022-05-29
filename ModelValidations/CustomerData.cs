using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ZofyaApi.ModelValidations
{
    public class CustomerData {
        
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string Phone { get; set; }
        public string Agree {get; set;}

        public Result validateEmail(string email)
        {
            Result emailResult = new Result();
            List<String> errorMessage = new List<String>();
            bool requiredValidation = string.IsNullOrEmpty(email);

            if (requiredValidation)
            {
                emailResult.correct = false;
                errorMessage.Add("Email Field Required");
                emailResult.message = errorMessage;

                return emailResult;
            }

            int lengthValidation = email.Length;
            const int MAX_LENGTH = 50;

            if (lengthValidation > MAX_LENGTH)
            {
                emailResult.correct = false;
                errorMessage.Add("Maximum length of 50 characters");
                emailResult.message = errorMessage;

                return emailResult;
            }

            string pattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*(\.[A-Za-z]{1,})$";
            bool patternValidation = Regex.IsMatch(email, pattern);

            if (!patternValidation)
            {

                emailResult.correct = false;
                errorMessage.Add("Invalid Email Format.");
                emailResult.message = errorMessage;

                return emailResult;
            }

            emailResult.correct = true;
            return emailResult;

        }

        public Result validateFullname(string fullname)
        {
            Result fullnameResult = new Result();
            List<String> errorMessage = new List<String>();
            bool requiredValidation = string.IsNullOrEmpty(fullname);

            if (requiredValidation)
            {
                fullnameResult.correct = false;
                errorMessage.Add("Full Name Field Required");
                fullnameResult.message = errorMessage;

                return fullnameResult;
            }

            int lengthValidation = fullname.Length;
            const int MAX_LENGTH = 100;

            if (lengthValidation > MAX_LENGTH)
            {
                fullnameResult.correct = false;
                errorMessage.Add("Maximum length of 100 characters");
                fullnameResult.message = errorMessage;

                return fullnameResult;
            }

            string pattern = @"^[0-9a-zA-ZÀ-ÿ\\u00f1\\u00d1]{1,}[0-9\\sa-zA-ZÀ-ÿ\\u00f1\\u00d1.:',_-]{0,}$";
            bool patternValidation = Regex.IsMatch(fullname, pattern);

            if (!patternValidation)
            {

                fullnameResult.correct = false;
                errorMessage.Add("Invalid Full Name Format.");
                fullnameResult.message = errorMessage;

                return fullnameResult;
            }

            fullnameResult.correct = true;
            return fullnameResult;

        }

        public Result validatePassword(string password)
        {
            Result passwordResult = new Result();
            List<String> errorMessage = new List<String>();
            bool requiredValidation = string.IsNullOrEmpty(password);

            if (requiredValidation)
            {
                passwordResult.correct = false;
                errorMessage.Add("Password Field Required");
                passwordResult.message = errorMessage;

                return passwordResult;
            }

            int lengthValidation = password.Length;
            const int MIN_LENGTH = 8;
            const int MAX_LENGTH = 16;

            if ((lengthValidation < MIN_LENGTH) || (lengthValidation > MAX_LENGTH))
            {
                passwordResult.correct = false;

                if (lengthValidation < MIN_LENGTH)
                {
                    errorMessage.Add("Minimum length of 8 characters");
                    passwordResult.message = errorMessage;
                }
                else
                {
                    errorMessage.Add("Maximum length of 16 characters");
                    passwordResult.message = errorMessage;
                }
                                
                return passwordResult;
            }

            // string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&#.$($)$-$_])[A-Za-z\d$@$!%*?&#.$($)$-$_]{8,16}$";
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&#.$($)$-$_])[A-Za-zÀ-ÿ\\u00f1\\u00d1\d$@$!%*?&#.$($)$-$_]{8,16}$";
            bool patternValidation = Regex.IsMatch(password, pattern);

            if (!patternValidation)
            {

                passwordResult.correct = false;
                errorMessage.Add("Invalid Password Format.");
                passwordResult.message = errorMessage;

                return passwordResult;
            }

            passwordResult.correct = true;
            return passwordResult;

        }

         public Result validatePhone(string phone)
        {
            Result phoneResult = new Result();
            List<String> errorMessage = new List<String>();
            bool requiredValidation = string.IsNullOrEmpty(phone);

            if (requiredValidation)
            {
                phoneResult.correct = false;
                errorMessage.Add("Phone Field Required");
                phoneResult.message = errorMessage;

                return phoneResult;
            }

            int lengthValidation = phone.Length;
            const int MAX_LENGTH = 10;

            if (lengthValidation != MAX_LENGTH)
            {
                phoneResult.correct = false;
                errorMessage.Add("Must be a 10 digit phone number!");
                phoneResult.message = errorMessage;

                return phoneResult;
            }

            string pattern = @"^[0-9]{10}$";
            bool patternValidation = Regex.IsMatch(phone, pattern);

            if (!patternValidation)
            {

                phoneResult.correct = false;
                errorMessage.Add("Invalid Phone Format.");
                phoneResult.message = errorMessage;

                return phoneResult;
            }

            phoneResult.correct = true;
            return phoneResult;

        }
    }

    

}

