using MediatR;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateVote;

public record UpdateVoteRequest(Guid ElectionId, Guid LinkId, List<Guid> CandidatesIds) : IRequest<GenericResponse>;