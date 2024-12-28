using Domain.ElectionAggregate.Election.Enumerations;
using MediatR;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateElectionProgress;

public record UpdateProgressRequest(Guid ElectionId, ElectionProgressEnum Progress) : IRequest<GenericResponse>;