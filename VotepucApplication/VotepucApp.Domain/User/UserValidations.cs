using System.Text.RegularExpressions;

namespace Domain.User;

public class UserValidations
{
    public static bool IsValidEmail(string email)
    {
        const string emailRegex = """^(?(")(".+?"@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w]))+@))""" +
                                  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-0-9a-zA-Z]*[0-9a-zA-Z]*\.)+[a-zA-Z]{2,}))$";

        return Regex.IsMatch(email, emailRegex, RegexOptions.IgnoreCase);
    }
}