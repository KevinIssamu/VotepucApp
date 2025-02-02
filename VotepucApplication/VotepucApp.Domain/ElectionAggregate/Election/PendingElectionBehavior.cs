using Domain.ElectionAggregate.Election.Constants;
using Domain.ElectionAggregate.Election.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using OneOf;

namespace Domain.ElectionAggregate.Election;

public class PendingElectionBehavior(Election election)
{
    public OneOf<AppSuccess, AppError> SetTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            return new AppError("Title cannot be null or whitespace.", AppErrorTypeEnum.BusinessRuleValidationFailure);
        if (newTitle.Length > ConstantsElectionValidations.TitleMaxLength)
            return new AppError($"Title cannot exceed {ConstantsElectionValidations.TitleMaxLength} characters.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        election.Title = newTitle;
        return new AppSuccess("Title updated successfully.");
    }

    public void SetParticipants(List<Participant.Participant> newParticipants)
    {
        election.Participants = newParticipants;
    }

    public OneOf<AppSuccess, AppError> SetDescription(string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
            return new AppError("Description cannot be null or whitespace.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);
        if (newDescription.Length > ConstantsElectionValidations.DescriptionMaxLength)
            return new AppError(
                $"Description cannot exceed {ConstantsElectionValidations.DescriptionMaxLength} characters.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        election.Description = newDescription;
        return new AppSuccess("Description updated successfully.");
    }

    public OneOf<AppSuccess, AppError> SetInviteEmailText(string newInviteEmailText)
    {
        if (string.IsNullOrWhiteSpace(newInviteEmailText))
            return new AppError("Invite email text cannot be null or whitespace.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);
        if (newInviteEmailText.Length > ConstantsElectionValidations.EmailInvitationTextMaxLength)
            return new AppError(
                $"Invite email text cannot exceed {ConstantsElectionValidations.EmailInvitationTextMaxLength} characters.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        election.EmailInvitationText = newInviteEmailText;
        return new AppSuccess("Invite email text updated successfully.");
    }

    public OneOf<AppSuccess, AppError> SetStartDate(DateTimeOffset newStartDate)
    {
        if (newStartDate < election.StartDate)
            return new AppError("New start date must be equal to or after the current start date.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);
        if (newStartDate > election.EndDate)
            return new AppError("Start date cannot be after the end date.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        election.StartDate = newStartDate;
        return new AppSuccess("Start date updated successfully.");
    }

    public OneOf<AppSuccess, AppError> SetEndDate(DateTimeOffset newEndDate)
    {
        if (newEndDate < election.StartDate)
            return new AppError("End date cannot be earlier than the start date.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        election.EndDate = newEndDate;
        return new AppSuccess("End date updated successfully.");
    }

    public OneOf<AppSuccess, AppError> Approve()
    {
        if (election.ElectionStatus != ElectionStatusEnum.Pending)
            return new AppError($"election with {election.ElectionStatus} status cannot be approved.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        election.ElectionStatus = ElectionStatusEnum.Approved;
        return new AppSuccess("Election approved successfully.");
    }

    public OneOf<AppSuccess, AppError> Reject()
    {
        if (election.ElectionStatus == ElectionStatusEnum.Approved)
            return new AppError("Cannot reject an election that has already been approved.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        election.ElectionStatus = ElectionStatusEnum.Rejected;
        return new AppSuccess("Election rejected successfully.");
    }
}