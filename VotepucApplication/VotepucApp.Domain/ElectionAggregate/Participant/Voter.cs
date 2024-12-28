using Domain.ElectionAggregate.Election.Enumerations;
using Domain.ElectionAggregate.Participant.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.AppError.Constants;
using Domain.Shared.AppSuccess;
using Domain.Shared.Constants;
using Domain.Shared.SharedValidators;
using OneOf;

namespace Domain.ElectionAggregate.Participant;

public class Voter : Participant
{
    public bool Voted { get; private set; }

    public OneOf<AppSuccess, AppError> SetVoted()
    {
        if (Election.Progress != ElectionProgressEnum.Active)
            return new AppError("Cannot set vote. Election is not active.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        Voted = true;
        UpdatedAt = DateTimeOffset.Now;
        return new AppSuccess("Vote registered successfully.");
    }

    public static class Factory
    {
        public static OneOf<Voter, AppError> Create(string email, string name, Election.Election election)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new AppError("Name cannot be null or empty.", AppErrorTypeEnum.BusinessRuleValidationFailure);

            if (name.Length > LengthProperties.PersonNameMaxLength)
                return new AppError($"Name cannot exceed {LengthProperties.PersonNameMaxLength} characters.",
                    AppErrorTypeEnum.BusinessRuleValidationFailure);

            return new Voter
            {
                Id = Guid.NewGuid(),
                Email = email,
                Name = name,
                TypeOfParticipant = TypeOfParticipantEnum.Voter,
                ElectionId = election.Id,
                Election = election,
                Voted = false,
                CreateAt = DateTimeOffset.Now
            };
        }
    }
}