using Domain.Shared;
using Domain.Shared.AppError;
using Domain.Shared.AppError.Constants;
using Domain.Shared.AppError.GenericErrors;
using Domain.Shared.AppSuccess;
using Domain.Shared.SharedValidators;
using Domain.User.Enumerations;
using Domain.User.Messages;
using Domain.User.Messages.Constants;
using OneOf;

namespace Domain.User;

public class User : BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public TypeOfUser TypeOfUser { get; private set; }
    public ICollection<Election.Election>? Elections { get; private set; }

    public OneOf<AppSuccess, AppError> SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return new GenericNullError(nameof(Name));
        if (name.Length >= ConstantsMaxLength.PersonNameMaxLength) return new NameExceededMaxLength();
        
        Name = name;
        UpdatedAt = DateTime.Now;
        return new UserNameUpdated();
    }
    
    public OneOf<AppSuccess, AppError> SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return new GenericNullError(nameof(Email));
        if (email.Length >= ConstantsMaxLength.PersonEmailMaxLength) return new NameExceededMaxLength();
        
        Email = email;
        UpdatedAt = DateTimeOffset.Now;
        return new UserEmailUpdated();
    }

    public OneOf<AppSuccess, AppError> SetPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return new GenericNullError(PasswordHash);
        
        PasswordHash = password;
        UpdatedAt = DateTimeOffset.Now;
        return new UserPasswordUpdated();
    }

    public OneOf<AppSuccess, AppError> SetUserType(TypeOfUser typeOfUser)
    {
        TypeOfUser = typeOfUser;
        return new UserTypeUpdated();
    }

    public static class Factory
    {
        public static OneOf<User, AppError> Create(string name, string email, string passwordHash, ICollection<Election.Election>? elections)
        {
            if (string.IsNullOrWhiteSpace(name)) return new GenericNullError(nameof(Name));
            if (string.IsNullOrWhiteSpace(email)) return new GenericNullError(nameof(Email));
            if (string.IsNullOrWhiteSpace(passwordHash)) return new GenericNullError(nameof(PasswordHash));
            
            var emailIsValid = GenericInvalidEmailValidator.IsValidEmail(email);
            if (emailIsValid.IsT1) return emailIsValid.AsT1;
            
            if (name.Length >= ConstantsMaxLength.PersonNameMaxLength) return new NameExceededMaxLength();

            return new User()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                PasswordHash = passwordHash,
                TypeOfUser = TypeOfUser.Common,
                CreateAt = DateTimeOffset.Now
            };
        }
    }
}