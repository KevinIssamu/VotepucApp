using Domain.ElectionAggregate.Election;
using Domain.Shared;
using Domain.Shared.AppError;
using Domain.Shared.AppError.Constants;
using Domain.Shared.SharedValidators;
using Domain.UserAggregate.User.Enumerations;
using OneOf;

namespace Domain.UserAggregate.User;

public class User : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public TypeOfUserEnum TypeOfUser { get; private set; }
    public ICollection<Election>? Elections { get; private set; }

    public bool SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        if (name.Length >= ConstantsMaxLength.PersonNameMaxLength)
        {
            return false;
        }

        Name = name;
        UpdatedAt = DateTime.Now;

        return true;
    }

    public bool SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        if (email.Length >= ConstantsMaxLength.PersonEmailMaxLength)
        {
            return false;
        }

        Email = email;
        UpdatedAt = DateTimeOffset.Now;

        return true;
    }

    public bool SetPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        PasswordHash = password;
        UpdatedAt = DateTimeOffset.Now;

        return true;
    }

    public bool SetUserType(TypeOfUserEnum typeOfUser)
    {
        TypeOfUser = typeOfUser;
        UpdatedAt = DateTimeOffset.Now;

        return true;
    }

    public static class Factory
    {
        public static OneOf<User, AppError> Create(string name, string email, string passwordHash,
            ICollection<Election>? elections)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new AppError("Name cannot be null or empty", AppErrorTypeEnum.BusinessRuleValidationFailure);
            if (string.IsNullOrWhiteSpace(email))
                return new AppError("Email cannot be null or empty", AppErrorTypeEnum.BusinessRuleValidationFailure);
            if (string.IsNullOrWhiteSpace(passwordHash))
                return new AppError("Password cannot be null or empty", AppErrorTypeEnum.BusinessRuleValidationFailure);

            if (!GenericInvalidEmailValidator.IsValidEmail(email))
                return new AppError("Invalid email address.", AppErrorTypeEnum.BusinessRuleValidationFailure);

            if (name.Length > ConstantsMaxLength.PersonNameMaxLength)
                return new AppError($"Name cannot exceed {ConstantsMaxLength.PersonNameMaxLength} characters.",
                    AppErrorTypeEnum.BusinessRuleValidationFailure);

            return new User()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                PasswordHash = passwordHash,
                TypeOfUser = TypeOfUserEnum.Common,
                CreateAt = DateTimeOffset.Now
            };
        }
    }
}