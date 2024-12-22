using Domain.ElectionAggregate.Election.Enumerations;
using Domain.ElectionAggregate.VoteLink.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using OneOf;

namespace Domain.ElectionAggregate.Election;

public class ElectionApproved : Election
{
    public void Start()
    {
        Progress = ElectionProgressEnum.Active;
    }
    
    public void Finish()
    {
        Progress = ElectionProgressEnum.Finished;
    }

    public OneOf<AppSuccess, AppError> SetVoteLinks(List<VoteLink.VoteLink> voteLinks)
    {
        if (voteLinks.Count == 0)
            return new AppError("No vote links found", AppErrorTypeEnum.BusinessRuleValidationFailure);
        
        if (Status != ElectionStatusEnum.Approved)
            return new AppError("Invalid status", AppErrorTypeEnum.BusinessRuleValidationFailure);
        
        var hasActiveLinks = voteLinks.Any(v => v.Status != VoteLinkStatusEnum.Inactive);
        if (hasActiveLinks)
            return new AppError("Invalid status", AppErrorTypeEnum.BusinessRuleValidationFailure);
        
        VoteLinks = voteLinks;
        return new AppSuccess("Links inserted in the election successfully");
    }
}