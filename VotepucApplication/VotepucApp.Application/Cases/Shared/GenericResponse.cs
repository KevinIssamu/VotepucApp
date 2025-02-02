
using VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateElectionProgress;

namespace VotepucApp.Application.Cases.Shared;

public record GenericResponse(int StatusCode, string? Message, List<Link>? Links = null);