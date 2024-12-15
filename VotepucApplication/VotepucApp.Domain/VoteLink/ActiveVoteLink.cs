using Domain.Election.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.VoteLink.Enumerations;
using Domain.VoteLink.Messages;
using OneOf;

namespace Domain.VoteLink;

public class ActiveVoteLink : VoteLink
{
    public OneOf<AppSuccess, AppError> Disable()
    {
        if (Election.Progress != ElectionProgress.Finished) 
            return new InvalidProgressToDisableVoteLink();
        
        Status = VoteLinkStatus.Inactive;
        UpdatedAt = DateTimeOffset.Now;
        return new VoteLinkSuccessfullyDisabled();
    }
}