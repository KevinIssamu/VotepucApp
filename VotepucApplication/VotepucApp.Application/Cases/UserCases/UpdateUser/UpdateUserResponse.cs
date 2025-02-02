using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.UseCases.UpdateUser;

public record UpdateUserResponse(Guid Id, int StatusCode, string Message) : GenericResponse(StatusCode, Message);