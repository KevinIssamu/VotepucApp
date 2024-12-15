using Domain.Election.Enumerations;
using Domain.Shared;
using Domain.Shared.AppError;
using Domain.Shared.AppError.GenericErrors;
using Domain.Shared.AppSuccess;
using Domain.VoteLink.Enumerations;
using Domain.VoteLink.Messages;
using OneOf;

namespace Domain.VoteLink;

public class InactiveVoteLink : VoteLink
{
    public OneOf<AppSuccess, AppError> ActivateVoteLink()
    {
        if (Election.Status != ElectionStatus.Approved ||
            Election.Progress != ElectionProgress.Active ||
            Election.EndDate < DateTimeOffset.UtcNow)
        {
            return new InvalidStateOrProgressToActivateVoteLink();
        }

        if (WasUtilized) return new ShouldNotReactivateUsedVoteLink();
        
        Status = VoteLinkStatus.Active;
        UpdatedAt = DateTimeOffset.Now;
        return new VoteLinkSuccessfullyActivated();
    }
    
    public static class Factory
    {
        public static OneOf<InactiveVoteLink, AppError> Create(string token, bool wasUtilized, DateTimeOffset expirationDate, Election.Election election)
        {
            if (string.IsNullOrEmpty(token)) return new GenericNullError(nameof(Token));
            if (Guid.TryParse(token, out _)) return new TokenIsNotValid();
            
            if (election.Status != ElectionStatus.Approved || 
                election.Progress != ElectionProgress.Active || 
                expirationDate < DateTimeOffset.UtcNow) 
                wasUtilized = false;

            if (expirationDate < election.EndDate) return new ExpirationDateIsBeforeElectionEndDate();

            return new InactiveVoteLink()
            {
                Token = token,
                WasUtilized = wasUtilized,
                ExpirationDate = expirationDate,
                ElectionId = election.Id,
                Election = election,
                CreateAt = DateTimeOffset.Now,
            };
        }
    }
}