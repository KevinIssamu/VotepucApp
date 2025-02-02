using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.ElectionCases.SelectElection.Responses;

public record SelectElectionCandidatesResponse(int StatusCode, string Message, List<CandidateViewModel> Candidates) : GenericResponse(StatusCode, Message);