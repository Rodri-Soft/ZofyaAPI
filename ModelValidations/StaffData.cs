using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ZofyaApi.ModelValidations
{
    public class StaffData {

        public string RFC { get; set; } = null!;
        public string CURP { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public Result validateRFC(string rfc)
        {
            Result rfcResult = new Result();
            List<String> errorMessage = new List<String>();
            bool requiredValidation = string.IsNullOrEmpty(rfc);

            if (requiredValidation)
            {
                rfcResult.correct = false;
                errorMessage.Add("RFC Field Required");
                rfcResult.message = errorMessage;

                return rfcResult;
            }

            int lengthValidation = rfc.Length;
            const int MAX_LENGTH = 13;

            if (lengthValidation > MAX_LENGTH)
            {
                rfcResult.correct = false;
                errorMessage.Add("Maximum length of 13 characters");
                rfcResult.message = errorMessage;

                return rfcResult;
            }

            string pattern = @"^([A-ZÑ&]{3,4}) ?(?:- ?)?(\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])) ?(?:- ?)?([A-Z\d]{2})([A\d])$";
            bool patternValidation = Regex.IsMatch(rfc, pattern);

            if (!patternValidation)
            {

                rfcResult.correct = false;
                errorMessage.Add("Invalid RFC Format.");
                rfcResult.message = errorMessage;

                return rfcResult;
            }

            rfcResult.correct = true;
            return rfcResult;

        }

        public Result validateCURP(string curp)
        {
            Result curpResult = new Result();
            List<String> errorMessage = new List<String>();
            bool requiredValidation = string.IsNullOrEmpty(curp);

            if (requiredValidation)
            {
                curpResult.correct = false;
                errorMessage.Add("CURP Field Required");
                curpResult.message = errorMessage;

                return curpResult;
            }

            int lengthValidation = curp.Length;
            const int MAX_LENGTH = 18;

            if (lengthValidation > MAX_LENGTH)
            {
                curpResult.correct = false;
                errorMessage.Add("Maximum length of 18 characters");
                curpResult.message = errorMessage;

                return curpResult;
            }

            string pattern = @"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$";
            bool patternValidation = Regex.IsMatch(curp, pattern);

            if (!patternValidation)
            {

                curpResult.correct = false;
                errorMessage.Add("Invalid CURP Format.");
                curpResult.message = errorMessage;

                return curpResult;
            }

            curpResult.correct = true;
            return curpResult;

        }

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
                errorMessage.Add("Fullname Field Required");
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

            string pattern = @"^[0-9a-zA-ZÀ-ÿ\\u00f1\\u00d1]{1,}[0-9\sa-zA-ZÀ-ÿ\\u00f1\\u00d1.:',_-]{0,}$";
            bool patternValidation = Regex.IsMatch(fullname, pattern);

            if (!patternValidation)
            {

                fullnameResult.correct = false;
                errorMessage.Add("Invalid Fullname Format.");
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

