using Domain.Participant.Messages.Constants;
using Domain.Shared.AppError;

namespace Domain.Participant.Messages;

public record ElectionStatusIsInvalidToSetVote() : AppError(
    ConstantsVoterErrors.ElectionStatusIsInvalidToSetVote, AppErrorTypeEnum.BusinessRuleValidationFailure);