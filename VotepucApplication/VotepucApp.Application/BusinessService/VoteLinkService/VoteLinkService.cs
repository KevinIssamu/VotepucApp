using System.Security.Claims;
using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Participant;
using Domain.ElectionAggregate.VoteLink;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.Shared.Interfaces;
using OneOf;
using VotepucApp.Application.AuthenticationsServices.Interfaces;

namespace VotepucApp.Application.BusinessService.VoteLinkService;

public class VoteLinkService(IElectionRepository electionRepository, ITokenService tokenService, IJwtSettings config, IUnitOfWork unitOfWork)
{
    public async Task<OneOf<List<VoteLink>, AppError>> CreateLinks(Election election, CancellationToken cancellationToken)
    {
        var participantsResult = await electionRepository.SelectElectionParticipants(election.Id, cancellationToken);

        if (participantsResult.IsT1)
            return participantsResult.AsT1;

        var links = new List<VoteLink>();
        var claims = new List<Claim>
        {
            new Claim("ElectionId", election.Id.ToString()),
            new Claim("WasUtilized", false.ToString()),
        };

        foreach (var participant in participantsResult.AsT0)
        {
            claims.Add(new Claim(ClaimTypes.Email, participant.Email));
            links.Add(new VoteLink(
                tokenService.GenerateVoteRefreshToken(claims, config, election.EndDate).ToString(),
                election));
        }

        return links;
    }
}