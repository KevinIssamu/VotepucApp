using Domain.Shared.AppSuccess;
using Domain.VoteLink.Messages.Constants;

namespace Domain.VoteLink.Messages;

public record VoteLinkSuccessfullyActivated() : AppSuccess(
    ConstantsVoteLinkSuccess.VoteLinkSuccessfullyActivated, AppSuccessTypeEnum.Success);
    
public record ExpirationDateSuccessfullyModified() : AppSuccess(
    ConstantsVoteLinkSuccess.ExpirationDateSuccessfullyModified, AppSuccessTypeEnum.Success);

public record VoteLinkSuccessfullyDisabled() : AppSuccess(
    ConstantsVoteLinkSuccess.VoteLinkSuccessfullyDisabled, AppSuccessTypeEnum.Success);   