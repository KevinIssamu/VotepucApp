using Domain.Shared.AppError;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.Shared.Responses;

public sealed record UserResponse(UserResponseViewModel? User, int StatusCode, string Message) : GenericResponse(StatusCode, Message);