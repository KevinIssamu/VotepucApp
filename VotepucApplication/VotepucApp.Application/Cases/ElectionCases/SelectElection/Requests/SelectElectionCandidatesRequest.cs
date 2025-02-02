using MediatR;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.ElectionCases.SelectElection.Requests;

public record SelectElectionCandidatesRequest(Guid ElectionId, Guid LinkId) : IRequest<GenericResponse>;