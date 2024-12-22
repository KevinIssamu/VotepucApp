using Domain.ElectionAggregate.Election.Enumerations;
using Domain.ElectionAggregate.VoteLink.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using OneOf;

namespace Domain.ElectionAggregate.VoteLink;

public class InactiveVoteLink : VoteLink
{
    public OneOf<AppSuccess, AppError> ActivateVoteLink()
    {
        if (Election.Status != ElectionStatusEnum.Approved ||
            Election.Progress != ElectionProgressEnum.Active ||
            Election.EndDate < DateTimeOffset.UtcNow)
        {
            return new AppError("Cannot activate vote link. Invalid election state or progress.", AppErrorTypeEnum.BusinessRuleValidationFailure);
        }

        if (WasUtilized)
            return new AppError("Cannot reactivate a vote link that was already used.", AppErrorTypeEnum.BusinessRuleValidationFailure);

        Status = VoteLinkStatusEnum.Active;
        UpdatedAt = DateTimeOffset.Now;
        return new AppSuccess("Vote link successfully activated.");
    }

    public static class Factory
    {
        public static OneOf<InactiveVoteLink, AppError> Create(string token, bool wasUtilized, DateTimeOffset expirationDate, Election.Election election)
        {
            if (string.IsNullOrWhiteSpace(token))
                return new AppError("Token cannot be null or empty.", AppErrorTypeEnum.BusinessRuleValidationFailure);

            if (!Guid.TryParse(token, out _))
                return new AppError("Invalid token format. Token must be a valid GUID.", AppErrorTypeEnum.BusinessRuleValidationFailure);

            if (election.Status != ElectionStatusEnum.Approved ||
                election.Progress != ElectionProgressEnum.Active ||
                expirationDate < DateTimeOffset.UtcNow)
            {
                wasUtilized = false;
            }

            if (expirationDate < election.EndDate)
                return new AppError("Expiration date cannot be before the election's end date.", AppErrorTypeEnum.BusinessRuleValidationFailure);

            return new InactiveVoteLink
            {
                Token = token,
                WasUtilized = wasUtilized,
                ExpirationDate = expirationDate,
                ElectionId = election.Id,
                Election = election,
                CreateAt = DateTimeOffset.Now
            };
        }
    }
}