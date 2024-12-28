using MediatR;
using VotepucApp.Application.Cases.ElectionCases.SelectElection.Responses;

namespace VotepucApp.Application.Cases.ElectionCases.Shared;

public record ElectionIdRequest<T>(Guid Id) : IRequest<T>;