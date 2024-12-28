using Domain.ElectionAggregate.Election.Enumerations;
using Domain.ElectionAggregate.VoteLink.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using OneOf;

namespace Domain.ElectionAggregate.Election;

public class ElectionApproved : Election
{
    public OneOf<AppSuccess, AppError> Start()
    {
        if(Progress != ElectionProgressEnum.Inactive)
            return new AppError("Only inactive elections can be concluded.", AppErrorTypeEnum.BusinessRuleValidationFailure);
        Progress = ElectionProgressEnum.Active;
        return new AppSuccess("Election started successfully.");
    }
    
    public OneOf<AppSuccess, AppError> Finish()
    {
        if (Progress != ElectionProgressEnum.Active)
            return new AppError("Only active elections can be concluded.", AppErrorTypeEnum.BusinessRuleValidationFailure);
        Progress = ElectionProgressEnum.Finished;
        return new AppSuccess("Election finished successfully.");
    }

    public OneOf<AppSuccess, AppError> SetVoteLinks(List<VoteLink.VoteLink> voteLinks)
    {
        if (voteLinks.Count == 0)
            return new AppError("No vote links found", AppErrorTypeEnum.BusinessRuleValidationFailure);
        
        if (Status != ElectionStatusEnum.Approved)
            return new AppError("Invalid status", AppErrorTypeEnum.BusinessRuleValidationFailure);
        
        VoteLinks = voteLinks;
        return new AppSuccess("Links inserted in the election successfully");
    }
}