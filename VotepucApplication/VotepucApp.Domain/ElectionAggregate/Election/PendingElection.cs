using Domain.ElectionAggregate.Election.Constants;
using Domain.ElectionAggregate.Election.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using OneOf;

namespace Domain.ElectionAggregate.Election;

public class PendingElection : Election
{
    public OneOf<AppSuccess, AppError> SetTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            return new AppError("Title cannot be null or whitespace.", AppErrorTypeEnum.BusinessRuleValidationFailure);
        if (newTitle.Length > ConstantsElectionValidations.TitleMaxLength)
            return new AppError($"Title cannot exceed {ConstantsElectionValidations.TitleMaxLength} characters.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        Title = newTitle;
        return new AppSuccess("Title updated successfully.");
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

        Description = newDescription;
        return new AppSuccess("Description updated successfully.");
    }

    public OneOf<AppSuccess, AppError> SetInviteEmailText(string newInviteEmailText)
    {
        if (string.IsNullOrWhiteSpace(newInviteEmailText))
            return new AppError("Invite email text cannot be null or whitespace.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);
        if (newInviteEmailText.Length > ConstantsElectionValidations.EmailInvitationText)
            return new AppError(
                $"Invite email text cannot exceed {ConstantsElectionValidations.EmailInvitationText} characters.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        EmailInvitationText = newInviteEmailText;
        return new AppSuccess("Invite email text updated successfully.");
    }

    public OneOf<AppSuccess, AppError> SetStartDate(DateTimeOffset newStartDate)
    {
        if (newStartDate < StartDate)
            return new AppError("New start date must be equal to or after the current start date.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);
        if (newStartDate > EndDate)
            return new AppError("Start date cannot be after the end date.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        StartDate = newStartDate;
        return new AppSuccess("Start date updated successfully.");
    }

    public OneOf<AppSuccess, AppError> SetEndDate(DateTimeOffset newEndDate)
    {
        if (newEndDate < StartDate)
            return new AppError("End date cannot be earlier than the start date.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        EndDate = newEndDate;
        return new AppSuccess("End date updated successfully.");
    }

    public OneOf<AppSuccess, AppError> Approve()
    {
        if (Status == ElectionStatusEnum.Rejected)
            return new AppError("Cannot approve an election that has been rejected.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        Status = ElectionStatusEnum.Approved;
        return new AppSuccess("Election approved successfully.");
    }

    public OneOf<AppSuccess, AppError> Reject()
    {
        if (Status == ElectionStatusEnum.Approved)
            return new AppError("Cannot reject an election that has already been approved.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        Status = ElectionStatusEnum.Rejected;
        return new AppSuccess("Election rejected successfully.");
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
            if (string.IsNullOrWhiteSpace(title))
                return new AppError("Title cannot be null or whitespace.",
                    AppErrorTypeEnum.BusinessRuleValidationFailure);
            if (string.IsNullOrWhiteSpace(description))
                return new AppError("Description cannot be null or whitespace.",
                    AppErrorTypeEnum.BusinessRuleValidationFailure);
            if (string.IsNullOrWhiteSpace(emailInvitationText))
                return new AppError("Email invitation text cannot be null or whitespace.",
                    AppErrorTypeEnum.BusinessRuleValidationFailure);

            if (participants.Count == 0)
                return new AppError("An election must have at least one participant.",
                    AppErrorTypeEnum.BusinessRuleValidationFailure);

            if (endDate < DateTimeOffset.Now)
                return new AppError("End date cannot be in the past.", AppErrorTypeEnum.BusinessRuleValidationFailure);
            if (startDate > endDate)
                return new AppError("Start date cannot be after the end date.",
                    AppErrorTypeEnum.BusinessRuleValidationFailure);

            return new PendingElection
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                EmailInvitationText = emailInvitationText,
                MultiVote = multiVote,
                StartDate = startDate,
                Status = ElectionStatusEnum.Pending,
                Progress = ElectionProgressEnum.Inactive,
                EndDate = endDate,
                Participants = participants,
                CreateAt = DateTimeOffset.Now
            };
        }
    }
}