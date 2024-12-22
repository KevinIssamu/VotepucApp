using Domain.UserAggregate.User.Enumerations;

namespace VotepucApp.Application.ViewModels;

public record UserResponseViewModel(Guid Id, string Email, string UserName, TypeOfUserEnum UserType);