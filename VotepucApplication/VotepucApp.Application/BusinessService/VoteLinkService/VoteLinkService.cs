using System.Security.Claims;
using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Participant;
using Domain.ElectionAggregate.VoteLink;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.Shared.Interfaces;
using OneOf;
using VotepucApp.Services.Interfaces.ConfigInterfaces;

namespace VotepucApp.Application.BusinessService.VoteLinkService;

public class VoteLinkService(IElectionRepository electionRepository, IVoteLinkRepository voteLinkRepository, ITokenService tokenService, IJwtSettings config)
{
    public async Task<OneOf<VoteLink, AppError>> GetVoteLinkTracking(Guid voteLinkId, CancellationToken cancellationToken)
    {
        var selectVoteLinkResult = await voteLinkRepository.GetVoteLinkByIdTrackingAsync(voteLinkId, cancellationToken);

        if (selectVoteLinkResult.IsT1)
            return selectVoteLinkResult.AsT1;
        
        if(selectVoteLinkResult.AsT0 is null)
            return new AppError("No link found", AppErrorTypeEnum.NotFound);
        
        return selectVoteLinkResult.AsT0;
    }
    public async Task<OneOf<List<VoteLink>, AppError>> CreateLinks(Election election, CancellationToken cancellationToken)
    {
        var participantsResult = await electionRepository.SelectVoters(election.Id, cancellationToken);

        if (participantsResult.IsT1)
            return participantsResult.AsT1;

        var links = new List<VoteLink>();
        var claims = new List<Claim>
        {
            new("ElectionId", election.Id.ToString()),
            new("WasUtilized", false.ToString()),
        };

        foreach (var participant in participantsResult.AsT0)
        {
            claims.Add(new Claim(ClaimTypes.Email, participant.Email));

            var token = tokenService.GenerateVoteRefreshToken(claims, config, election.EndDate);
            
            if(token.IsT1)
                return token.AsT1;
            
            links.Add(new VoteLink(token.AsT0, election));
        }
        
        var createLinksResult = await voteLinkRepository.CreateAsync(links, cancellationToken);
        
        if(createLinksResult.IsT1)
            return createLinksResult.AsT1;

        return links;
    }

    public OneOf<AppSuccess, AppError> InvalidateVoteLinks(List<VoteLink> links)
    {
        foreach (var link in links)
        {
            var invalidateLink = link.RemoveToken();

            if (invalidateLink.IsT1)
                return invalidateLink.AsT1;
        }

        return new AppSuccess("Links invalidated");
    }
}