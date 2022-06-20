using System.Text.RegularExpressions;
using ZofyaApi.ModelValidations;

public class WishListData
{
  public string Name { get; set; } = null!;
  public int IDUser { get; set; }

  public Result ValidateWishlistName(string nameWishlist)
  {
    Result nameResult = new Result();
    List<String> errorMessage = new List<String>();
    bool requiredValidation = string.IsNullOrEmpty(nameWishlist);

    if (requiredValidation)
    {
      nameResult.correct = false;
      errorMessage.Add("Wishlist Name Field Required");
      nameResult.message = errorMessage;

      return nameResult;
    }

    int lengthValidation = nameWishlist.Length;
    const int MAX_LENGTH = 50;

    if (lengthValidation > MAX_LENGTH)
    {
      nameResult.correct = false;
      errorMessage.Add("Maximum length of 50 characters");
      nameResult.message = errorMessage;

      return nameResult;
    }

    string pattern = @"^[0-9a-zA-ZÀ-ÿ\\u00f1\\u00d1]{1,}[0-9\sa-zA-ZÀ-ÿ\\u00f1\\u00d1.:',_-]{0,}$";
    bool patternValidation = Regex.IsMatch(nameWishlist, pattern);

    if (!patternValidation)
    {
      nameResult.correct = false;
      errorMessage.Add("Invalid Street Name Format.");
      nameResult.message = errorMessage;

      return nameResult;
    }

    nameResult.correct = true;

    return nameResult;
  }
}