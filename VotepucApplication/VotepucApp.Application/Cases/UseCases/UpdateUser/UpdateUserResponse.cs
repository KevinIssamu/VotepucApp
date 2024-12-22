using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;

namespace VotepucApp.Application.Cases.UseCases.UpdateUser;

public record UpdateUserResponse(Guid Id, AppSuccess? Success, AppError? Error);