using Domain.ElectionAggregate.Election;
using Domain.Shared;
using Domain.Shared.AppError;
using Domain.Shared.AppError.Constants;
using Domain.Shared.Constants;
using Domain.Shared.Interfaces;
using Domain.Shared.SharedValidators;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace Domain.UserAggregate.User;

public class User : IdentityUser, IAggregateRoot
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public ICollection<Election>? Elections { get; private set; }
    public DateTimeOffset CreateAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public static class Factory
    {
        public static OneOf<User, AppError> Create(string name, string email,
            ICollection<Election>? elections = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new AppError("Name cannot be null or empty", AppErrorTypeEnum.BusinessRuleValidationFailure);
            if (string.IsNullOrWhiteSpace(email))
                return new AppError("Email cannot be null or empty", AppErrorTypeEnum.BusinessRuleValidationFailure);

            return name.Length switch
            {
                > LengthProperties.PersonNameMaxLength => new AppError(
                    $"Name cannot exceed {LengthProperties.PersonNameMaxLength} characters.",
                    AppErrorTypeEnum.BusinessRuleValidationFailure),
                < LengthProperties.PersonNameMinLength => new AppError(
                    $"Name must contain at least {LengthProperties.PersonNameMinLength} characters.",
                    AppErrorTypeEnum.BusinessRuleValidationFailure),
                _ => new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = name,
                    Email = email,
                    CreateAt = DateTimeOffset.Now
                }
            };
        }
    }
}