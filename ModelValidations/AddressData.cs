using System.Text.RegularExpressions;
using ZofyaApi.ModelValidations;

public class AddressData
{
  public string City { get; set; } = null!;
  public string Colony { get; set; } = null!;
  public string InsideNumber { get; set; } = null!;
  public string OutSideNumber { get; set; } = null!;
  public string PostalCode { get; set; } = null!;
  public string StreetName { get; set; } = null!;
  public int IDCustomer { get; set; }

  public Result ValidateStreetName(string streetName)
  {
    Result streetNameResult = new Result();
    List<String> errorMessage = new List<String>();
    bool requiredValidation = string.IsNullOrEmpty(streetName);

    if (requiredValidation)
    {
      streetNameResult.correct = false;
      errorMessage.Add("Street Name Field Required");
      streetNameResult.message = errorMessage;

      return streetNameResult;
    }

    int lengthValidation = streetName.Length;
    const int MAX_LENGTH = 50;

    if (lengthValidation > MAX_LENGTH)
    {
      streetNameResult.correct = false;
      errorMessage.Add("Maximum length of 50 characters");
      streetNameResult.message = errorMessage;

      return streetNameResult;
    }

    string pattern = @"^[0-9a-zA-ZÀ-ÿ\\u00f1\\u00d1]{1,}[0-9\sa-zA-ZÀ-ÿ\\u00f1\\u00d1.:',_-]{0,}$";
    bool patternValidation = Regex.IsMatch(streetName, pattern);

    if (!patternValidation)
    {

      streetNameResult.correct = false;
      errorMessage.Add("Invalid Street Name Format.");
      streetNameResult.message = errorMessage;

      return streetNameResult;
    }

    streetNameResult.correct = true;

    return streetNameResult;
  }

  public Result ValidateColony(string colony)
  {
    Result colonyResult = new Result();
    List<String> errorMessage = new List<String>();
    bool requiredValidation = string.IsNullOrEmpty(colony);

    if (requiredValidation)
    {
      colonyResult.correct = false;
      errorMessage.Add("Colony Field Required");
      colonyResult.message = errorMessage;

      return colonyResult;
    }

    int lengthValidation = colony.Length;
    const int MAX_LENGTH = 50;

    if (lengthValidation > MAX_LENGTH)
    {
      colonyResult.correct = false;
      errorMessage.Add("Maximum length of 100 characters");
      colonyResult.message = errorMessage;

      return colonyResult;
    }

    string pattern = @"^[0-9a-zA-ZÀ-ÿ\\u00f1\\u00d1]{1,}[0-9\sa-zA-ZÀ-ÿ\\u00f1\\u00d1.:',_-]{0,}$";
    bool patternValidation = Regex.IsMatch(colony, pattern);

    if (!patternValidation)
    {

      colonyResult.correct = false;
      errorMessage.Add("Invalid Colony Format.");
      colonyResult.message = errorMessage;

      return colonyResult;
    }

    colonyResult.correct = true;
    
    return colonyResult;
  }

  public Result ValidateOutSideNumber(string outSideNumber)
  {
    Result outSideNumberResult = new Result();
    List<String> errorMessage = new List<String>();
    bool requiredValidation = string.IsNullOrEmpty(outSideNumber);

    if (requiredValidation)
    {
      outSideNumberResult.correct = false;
      errorMessage.Add("Out Side Number Field Required");
      outSideNumberResult.message = errorMessage;

      return outSideNumberResult;
    }

    int lengthValidation = outSideNumber.Length;
    const int MIN_LENGTH = 1;
    const int MAX_LENGTH = 5;

    if ((lengthValidation < MIN_LENGTH) || (lengthValidation > MAX_LENGTH))
    {
      outSideNumberResult.correct = false;

      if (lengthValidation < MIN_LENGTH)
      {
        errorMessage.Add("Minimum length of 1 characters");
        outSideNumberResult.message = errorMessage;
      }
      else
      {
        errorMessage.Add("Maximum length of 5 characters");
        outSideNumberResult.message = errorMessage;
      }

      return outSideNumberResult;
    }

    string pattern = @"(\d+)+$";
    bool patternValidation = Regex.IsMatch(outSideNumber, pattern);

    if (!patternValidation)
    {

      outSideNumberResult.correct = false;
      errorMessage.Add("Invalid Number Format.");
      outSideNumberResult.message = errorMessage;

      return outSideNumberResult;
    }

    outSideNumberResult.correct = true;
    return outSideNumberResult;

  }

  public Result ValidatePostalCode(string postalCode)
  {
    Result postalCodeResult = new Result();
    List<String> errorMessage = new List<String>();
    bool requiredValidation = string.IsNullOrEmpty(postalCode);

    if (requiredValidation)
    {
      postalCodeResult.correct = false;
      errorMessage.Add("Postal Code Field Required");
      postalCodeResult.message = errorMessage;

      return postalCodeResult;
    }

    int lengthValidation = postalCode.Length;
    const int MAX_LENGTH = 5;

    if (lengthValidation != MAX_LENGTH)
    {
      postalCodeResult.correct = false;
      errorMessage.Add("Must be a 5 digit postal code number!");
      postalCodeResult.message = errorMessage;

      return postalCodeResult;
    }

    string pattern = @"^[0-9]{5}$";
    bool patternValidation = Regex.IsMatch(postalCode, pattern);

    if (!patternValidation)
    {

      postalCodeResult.correct = false;
      errorMessage.Add("Invalid Postal Code Format.");
      postalCodeResult.message = errorMessage;

      return postalCodeResult;
    }

    postalCodeResult.correct = true;
    
    return postalCodeResult;
  }

  public Result ValidateCity(string city)
  {
    Result cityResult = new Result();
    List<String> errorMessage = new List<String>();
    bool requiredValidation = string.IsNullOrEmpty(city);

    if (requiredValidation)
    {
      cityResult.correct = false;
      errorMessage.Add("City Field Required");
      cityResult.message = errorMessage;

      return cityResult;
    }

    int lengthValidation = city.Length;
    const int MAX_LENGTH = 50;

    if (lengthValidation > MAX_LENGTH)
    {
      cityResult.correct = false;
      errorMessage.Add("Maximum length of 50 characters");
      cityResult.message = errorMessage;

      return cityResult;
    }

    string pattern = @"^[0-9a-zA-ZÀ-ÿ\\u00f1\\u00d1]{1,}[0-9\sa-zA-ZÀ-ÿ\\u00f1\\u00d1.:',_-]{0,}$";
    bool patternValidation = Regex.IsMatch(city, pattern);

    if (!patternValidation)
    {
      cityResult.correct = false;
      errorMessage.Add("Invalid City Format.");
      cityResult.message = errorMessage;

      return cityResult;
    }

    cityResult.correct = true;

    return cityResult;
  }
}
