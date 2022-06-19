using System.Text.RegularExpressions;

namespace ZofyaApi.ModelValidations
{
    public class AuxiliaryAdministrationAddItem
    {
        public string SKU { get; set; } 
        public string Description { get; set; }      
        public string Name { get; set; } 
        public decimal Price { get; set; }
        public string Category { get; set; } 
        public string Status { get; set; } 
        public int Stock { get; set; }
        public string Gender { get; set; } 
        public string Care { get; set; }
        public List<string> Sizes { get; set; } = null!;
        public List<string> Colors { get; set; } = null!;
        public List<string> Images { get; set; } = null!;

        public Result validateAverageField(string field, string fieldName)
        {
            Result averageResult = new Result();
            List<String> errorMessage = new List<String>();
            bool requiredValidation = string.IsNullOrEmpty(field);

            if (requiredValidation)
            {
                averageResult.correct = false;
                errorMessage.Add(fieldName+" Field Required");
                averageResult.message = errorMessage;

                return averageResult;
            }

            int lengthValidation = field.Length;
            const int MAX_LENGTH = 50;

            if (lengthValidation > MAX_LENGTH)
            {
                averageResult.correct = false;
                errorMessage.Add("Maximum length of 50 characters");
                averageResult.message = errorMessage;

                return averageResult;
            }

            string pattern = @"^[0-9a-zA-ZÀ-ÿ\\u00f1\\u00d1]{1,}[0-9\sa-zA-ZÀ-ÿ\\u00f1\\u00d1.:',_-]{0,}$";
            bool patternValidation = Regex.IsMatch(field, pattern);

            if (!patternValidation)
            {

                averageResult.correct = false;
                errorMessage.Add( "Invalid "+ fieldName +" Format.");
                averageResult.message = errorMessage;

                return averageResult;
            }

            averageResult.correct = true;
            return averageResult;

        }

        public Result validateDescription(string description)
        {
            Result descriptionResult = new Result();
            List<String> errorMessage = new List<String>();
            bool requiredValidation = string.IsNullOrEmpty(description);

            if (requiredValidation)
            {
                descriptionResult.correct = false;
                errorMessage.Add("Description Field Required");
                descriptionResult.message = errorMessage;

                return descriptionResult;
            }

            int lengthValidation = description.Length;
            const int MAX_LENGTH = 100;

            if (lengthValidation > MAX_LENGTH)
            {
                descriptionResult.correct = false;
                errorMessage.Add("Maximum length of 100 characters");
                descriptionResult.message = errorMessage;

                return descriptionResult;
            }

            string pattern = @"^[0-9a-zA-ZÀ-ÿ\\u00f1\\u00d1]{1,}[0-9\sa-zA-ZÀ-ÿ\\u00f1\\u00d1.:',_-]{0,}$";
            bool patternValidation = Regex.IsMatch(description, pattern);

            if (!patternValidation)
            {

                descriptionResult.correct = false;
                errorMessage.Add("Invalid Description Format.");
                descriptionResult.message = errorMessage;

                return descriptionResult;
            }

            descriptionResult.correct = true;
            return descriptionResult;

        }

        public Result validatePrice(decimal price)
        {
            Result priceResult = new Result();
            List<String> errorMessage = new List<String>();
            bool requiredValidation = string.IsNullOrEmpty(price.ToString());

            if (requiredValidation)
            {
                priceResult.correct = false;
                errorMessage.Add("Price Field Required");
                priceResult.message = errorMessage;

                return priceResult;
            }
            
            const int MIN_PRICE = 99;

            if (price < MIN_PRICE)
            {
                priceResult.correct = false;
                errorMessage.Add("Minimum price of 99");
                priceResult.message = errorMessage;

                return priceResult;
            }          

            priceResult.correct = true;
            return priceResult;

        }

        public Result validateStock(int stock, string statusValue)
        {
            Result stockResult = new Result();
            List<String> errorMessage = new List<String>();
            bool requiredValidation = string.IsNullOrEmpty(stock.ToString());

            if (requiredValidation)
            {
                stockResult.correct = false;
                errorMessage.Add("Stock Field Required");
                stockResult.message = errorMessage;

                return stockResult;
            }
          
            if (stock == 0 && statusValue == "Available") {

                stockResult.correct = false;
                errorMessage.Add("There must be at least one item");
                stockResult.message = errorMessage;

                return stockResult;                
            }

            string pattern = @"^\d+$";
            bool patternValidation = Regex.IsMatch(stock.ToString(), pattern);

            if (!patternValidation)
            {

                stockResult.correct = false;
                errorMessage.Add("Only integers.");
                stockResult.message = errorMessage;

                return stockResult;
            }

            stockResult.correct = true;
            return stockResult;

        }

         public Result validateCare(string care)
        {
            Result careResult = new Result();
            List<String> errorMessage = new List<String>();
            bool requiredValidation = string.IsNullOrEmpty(care);

            if (requiredValidation)
            {
                careResult.correct = false;
                errorMessage.Add("Care Field Required");
                careResult.message = errorMessage;

                return careResult;
            }           

            string pattern = @"^[0-9a-zA-ZÀ-ÿ\\u00f1\\u00d1]{1,}[0-9\sa-zA-ZÀ-ÿ\\u00f1\\u00d1.:',_-]{0,}$";
            bool patternValidation = Regex.IsMatch(care, pattern);

            if (!patternValidation)
            {

                careResult.correct = false;
                errorMessage.Add("Invalid Care Format.");
                careResult.message = errorMessage;

                return careResult;
            }

            careResult.correct = true;
            return careResult;

        }



    }
}