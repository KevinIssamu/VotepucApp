using Domain.Election.Enumerations;
using Domain.Election.Messages;
using Domain.Election.Messages.Constants;
using Domain.Shared.AppError;
using Domain.Shared.AppError.Constants;
using Domain.Shared.AppError.GenericErrors;
using Domain.Shared.AppSuccess;
using OneOf;

namespace Domain.Election;

public class PendingElection : Election
{
    public OneOf<AppSuccess, AppError> SetTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle)) return new GenericNullError(nameof(Title));
        if (newTitle.Length > ConstantsElectionValidations.TitleMaxLength) return new ElectionTitleHasMaxLength();
        
        Title = newTitle;
        return new ElectionTileSuccessfullyUpdated();
    }

    public OneOf<AppSuccess, AppError> SetDescription(string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription)) return new GenericNullError(nameof(Description));
        if (newDescription.Length > ConstantsElectionValidations.DescriptionMaxLength) return new ElectionDescriptionHasMaxLength();
        
        Description = newDescription;
        return new ElectionDescriptionSuccessfullyUpdated();
    }
    
    public OneOf<AppSuccess, AppError> SetInviteEmailText(string newInviteEmailText)
    {
        if (string.IsNullOrWhiteSpace(newInviteEmailText)) return new GenericNullError(nameof(EmailInvitationText));
        if (newInviteEmailText.Length > ConstantsElectionValidations.EmailInvitationText) return new ElectionInviteEmailHasMaxLength();
        
        EmailInvitationText = newInviteEmailText;
        return new ElectionInviteEmailTextSuccessfullyUpdated();
    }

    public OneOf<AppSuccess, AppError> SetStartDate(DateTimeOffset newStartDate)
    {
        if (newStartDate < StartDate) return new NewStartDateMustBeEqualOrAfterStartDate();
        if (newStartDate > EndDate) return new StartDateAfterEndDateError();
        
        StartDate = newStartDate;
        return new ElectionStartDateSuccessfullyUpdated();
    }
    
    public OneOf<AppSuccess, AppError> SetEndDate(DateTimeOffset newEndDate)
    {
        if (newEndDate < StartDate) return new StartDateAfterEndDateError();
        
        EndDate = newEndDate;
        return new ElectionEndDateSuccessfullyUpdated();
    }

    public OneOf<AppSuccess, AppError> Approve()
    {
        if (Status.Equals(ElectionStatus.Rejected)) return new CannotApproveRejectedElectionError();
        
        Status = ElectionStatus.Approved;
        return new ElectionApprovedSuccessfully();
    }

    public OneOf<AppSuccess, AppError> Reject()
    {
        if(Status.Equals(ElectionStatus.Approved)) return new CannotRejectApprovedElectionError();
        
        Status = ElectionStatus.Rejected;
        return new ElectionRejectedSuccessfully();
    }
    
    public static class Factory
    {
        public static OneOf<PendingElection, AppError> Create(
            string title,
            string description,
            string emailInvitationText,
            bool multiVote,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            ICollection<Participant.Participant> participants)
        {
            if(string.IsNullOrWhiteSpace(title)) return new GenericNullError(nameof(Title));
            if(string.IsNullOrWhiteSpace(description)) return new GenericNullError(nameof(Description));
            if(string.IsNullOrWhiteSpace(emailInvitationText)) return new GenericNullError(nameof(EmailInvitationText));

            if (participants.Count == 0) return new ElectionMustHaveParticipants();

            if (endDate < DateTimeOffset.Now) return new EndDateCannotBeInThePastError();
            if (startDate > endDate) return new StartDateAfterEndDateError();

            return new PendingElection()
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                EmailInvitationText = emailInvitationText,
                MultiVote = multiVote,
                StartDate = startDate,
                Status = ElectionStatus.Pending,
                Progress = ElectionProgress.Inactive,
                EndDate = endDate,
                Participants = participants,
                CreateAt = DateTimeOffset.Now
            };
        }
    }
}