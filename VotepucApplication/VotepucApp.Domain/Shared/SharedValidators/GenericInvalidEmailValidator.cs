using System.Text.RegularExpressions;
using Domain.Shared.AppError;
using Domain.Shared.AppError.Constants;
using Domain.Shared.AppError.GenericErrors;
using Domain.Shared.AppSuccess;
using OneOf;

namespace Domain.Shared.SharedValidators;

public class GenericInvalidEmailValidator
{
    public static OneOf<AppSuccess.AppSuccess, AppError.AppError> IsValidEmail(string email)
    {
        if(string.IsNullOrWhiteSpace(email)) return new GenericNullError(nameof(email));
        if (email.Length >= ConstantsMaxLength.PersonEmailMaxLength) return new UserEmailExceededMaxLength();
        
        const string emailRegex = """^(?(")(".+?"@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w]))+@))""" +
                                  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-0-9a-zA-Z]*[0-9a-zA-Z]*\.)+[a-zA-Z]{2,}))$";

        if (Regex.IsMatch(email, emailRegex, RegexOptions.IgnoreCase)) return new EmailIsValid();

        return new UserEmailIsNotValid();
    }
}