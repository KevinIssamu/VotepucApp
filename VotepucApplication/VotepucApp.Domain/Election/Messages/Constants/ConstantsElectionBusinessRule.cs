namespace Domain.Election.Messages.Constants;

public class ConstantsElectionBusinessRule
{
    public const string StartDateAfterEndDate = "Start date must not be later than the end date";
    public const string EndDateCannotBeInThePast = "End date cannot be in the past";
    public const string ElectionMustHaveParticipants = "Election must have participants";
    public const string NewStartDateMustBeEqualOrAfterStartDate = "The new start date must be equal to or later than the already defined start date.";
    public const string CannotApproveRejectedElection = "Cannot approve rejected election";
    public const string CannotRejectApprovedElection = "Cannot reject approved election";
}