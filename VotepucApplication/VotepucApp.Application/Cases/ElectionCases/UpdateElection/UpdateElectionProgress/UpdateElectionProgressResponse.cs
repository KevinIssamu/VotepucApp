using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateElectionProgress;

public record UpdateElectionProgressResponse(int StatusCode, string Message, Guid TaskId, List<Link> Links) : GenericResponse(StatusCode, Message, Links);