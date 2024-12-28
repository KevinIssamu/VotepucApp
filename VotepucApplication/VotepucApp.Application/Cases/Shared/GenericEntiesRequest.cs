using MediatR;

namespace VotepucApp.Application.Cases.Shared;

public record GenericEntiesRequest<T>(int Top = 50, int Skip = 0) : IRequest<T>;