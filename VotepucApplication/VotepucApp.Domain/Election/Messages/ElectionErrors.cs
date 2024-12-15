using Domain.Election.Messages.Constants;
using Domain.Shared.AppError;

namespace Domain.Election.Messages;

public record EndDateCannotBeInThePastError() : AppError(
    ConstantsElectionBusinessRule.EndDateCannotBeInThePast, AppErrorTypeEnum.BusinessRuleValidationFailure);
    
public record StartDateAfterEndDateError() : AppError(
    ConstantsElectionBusinessRule.StartDateAfterEndDate, AppErrorTypeEnum.BusinessRuleValidationFailure);
    
public record ElectionMustHaveParticipants() : AppError(
    ConstantsElectionBusinessRule.ElectionMustHaveParticipants, AppErrorTypeEnum.BusinessRuleValidationFailure);

public record ElectionTitleHasMaxLength() 
    : AppError(
        $"Election title must have a maximum length of {ConstantsElectionValidations.TitleMaxLength} characters", 
        AppErrorTypeEnum.ValidationFailure);

public record ElectionDescriptionHasMaxLength()
    : AppError(
        $"Election description must have a maximum length of {ConstantsElectionValidations.DescriptionMaxLength} characters",
        AppErrorTypeEnum.ValidationFailure);

public record ElectionInviteEmailHasMaxLength() 
    : AppError(
        $"Election email invite text must have a maximum length of {ConstantsElectionValidations.EmailInvitationText}",
        AppErrorTypeEnum.ValidationFailure);

public record NewStartDateMustBeEqualOrAfterStartDate() 
    : AppError(ConstantsElectionBusinessRule.NewStartDateMustBeEqualOrAfterStartDate, 
        AppErrorTypeEnum.BusinessRuleValidationFailure);

public record CannotApproveRejectedElectionError() 
    : AppError(ConstantsElectionBusinessRule.CannotApproveRejectedElection, 
        AppErrorTypeEnum.BusinessRuleValidationFailure);

public record CannotRejectApprovedElectionError()
    : AppError(ConstantsElectionBusinessRule.CannotApproveRejectedElection,
        AppErrorTypeEnum.BusinessRuleValidationFailure);        