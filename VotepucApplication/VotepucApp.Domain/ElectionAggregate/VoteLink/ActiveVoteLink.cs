using Domain.ElectionAggregate.Election.Enumerations;
using Domain.ElectionAggregate.VoteLink.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using OneOf;

namespace Domain.ElectionAggregate.VoteLink;

public class ActiveVoteLink : VoteLink
{
    public OneOf<AppSuccess, AppError> Disable()
    {
        if (Election.Progress != ElectionProgressEnum.Finished)
            return new AppError("Cannot disable the vote link. Election progress is not finished.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        Status = VoteLinkStatusEnum.Inactive;
        UpdatedAt = DateTimeOffset.Now;
        return new AppSuccess("Vote link successfully disabled.");
    }
}