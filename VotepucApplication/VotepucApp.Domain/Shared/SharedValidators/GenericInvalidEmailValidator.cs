using System.Text.RegularExpressions;
using Domain.Shared.AppError.Constants;

namespace Domain.Shared.SharedValidators;

public class GenericInvalidEmailValidator
{
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        if (email.Length >= ConstantsMaxLength.PersonEmailMaxLength) return false;
        
        const string emailRegex = """^(?(")(".+?"@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w]))+@))""" +
                                  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-0-9a-zA-Z]*[0-9a-zA-Z]*\.)+[a-zA-Z]{2,}))$";

        return Regex.IsMatch(email, emailRegex, RegexOptions.IgnoreCase);
    }
}