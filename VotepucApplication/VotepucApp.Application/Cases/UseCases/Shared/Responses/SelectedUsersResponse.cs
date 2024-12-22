using Domain.Shared.AppError;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.Shared.Responses;

public record SelectedUsersResponse(List<UserResponseViewModel>? Users, AppError? ErrorMessage);