using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;

namespace VotepucApp.Application.Cases.UseCases.Shared.Responses;

public record OperationUserResponse(AppSuccess? SuccessMessage, AppError? ErrorMessage);