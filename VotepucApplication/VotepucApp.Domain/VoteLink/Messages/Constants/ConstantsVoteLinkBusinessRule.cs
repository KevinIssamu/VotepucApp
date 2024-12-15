namespace Domain.VoteLink.Messages.Constants;

public class ConstantsVoteLinkBusinessRule
{
    public const string ExpirationDateIsBeforeElectionEndDate = "Expiration date cannot be before election end date";
    public const string InvalidStateOrProgressToActivateVoteLink = "Invalid election status or progress to activate vote link";
    public const string ShouldNotReactivateUsedVoteLink = "Unable to reactivate used link";
    public const string InvalidProgressToDisableVoteLink = "Invalid progress to disable vote link";
}