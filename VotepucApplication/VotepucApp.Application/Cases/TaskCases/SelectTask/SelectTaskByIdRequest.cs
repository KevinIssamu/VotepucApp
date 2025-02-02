using MediatR;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.TaskCases.SelectTask;

public sealed record SelectTaskByIdRequest(Guid TaskId) : IRequest<GenericResponse>;