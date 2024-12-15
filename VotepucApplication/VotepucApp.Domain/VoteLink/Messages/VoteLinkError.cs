using Domain.Shared.AppError;
using Domain.VoteLink.Messages.Constants;

namespace Domain.VoteLink.Messages;

public record TokenIsNotValid() : AppError(
    ConstantsVoteLinkValidation.TokenIsNotValid, AppErrorTypeEnum.ValidationFailure);

public record ExpirationDateIsBeforeElectionEndDate() : AppError(
    ConstantsVoteLinkBusinessRule.ExpirationDateIsBeforeElectionEndDate, AppErrorTypeEnum.BusinessRuleValidationFailure);

public record InvalidStateOrProgressToActivateVoteLink() : AppError(
    ConstantsVoteLinkBusinessRule.InvalidStateOrProgressToActivateVoteLink, AppErrorTypeEnum.BusinessRuleValidationFailure);

public record ShouldNotReactivateUsedVoteLink() : AppError(
    ConstantsVoteLinkBusinessRule.ShouldNotReactivateUsedVoteLink, AppErrorTypeEnum.BusinessRuleValidationFailure);

public record InvalidProgressToDisableVoteLink() : AppError(
    ConstantsVoteLinkBusinessRule.InvalidProgressToDisableVoteLink, AppErrorTypeEnum.BusinessRuleValidationFailure);
    
    