using Domain.Shared.AppError;
using Domain.UserAggregate.User.Enumerations;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.Shared.Responses;

public sealed record UserResponse(UserResponseViewModel? User, AppError? AppError);