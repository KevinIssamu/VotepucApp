using Domain.UserAggregate.User.Enumerations;

namespace VotepucApp.Application.ViewModels;

public record UserRequestViewModel(string Email, string UserName, TypeOfUserEnum UserType);