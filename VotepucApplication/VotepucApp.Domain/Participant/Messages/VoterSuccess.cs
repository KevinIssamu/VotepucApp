using Domain.Participant.Messages.Constants;
using Domain.Shared.AppSuccess;

namespace Domain.Participant.Messages;

public record VoteSetSuccessfully() : AppSuccess(ConstantsVoterSuccess.VoteSetSuccessfully, AppSuccessTypeEnum.Success);