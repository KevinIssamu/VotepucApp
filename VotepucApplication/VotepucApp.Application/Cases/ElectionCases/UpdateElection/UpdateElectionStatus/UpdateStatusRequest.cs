using Domain.ElectionAggregate.Election.Enumerations;
using MediatR;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateElectionStatus;

public record UpdateStatusRequest(Guid ElectionId, ElectionStatusEnum Status) : IRequest<GenericResponse>;