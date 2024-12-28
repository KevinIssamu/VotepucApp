using Domain.Shared.AppError;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.Shared.Responses;

public record SelectedUsersResponse(List<UserResponseViewModel>? Users, int StatusCode, string Message)
    : GenericResponse(StatusCode, Message);