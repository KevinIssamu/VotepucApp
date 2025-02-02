using Domain.ElectionAggregate.Election.Enumerations;
using Domain.ElectionAggregate.Participant.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.AppError.Constants;
using Domain.Shared.AppSuccess;
using Domain.Shared.Constants;
using Domain.Shared.SharedValidators;
using OneOf;

namespace Domain.ElectionAggregate.Participant;

public class Candidate : Participant
{
    public int Votes { get; private set; }

    public OneOf<AppSuccess, AppError> SetVoted()
    {
        Votes++;
        UpdatedAt = DateTime.Now;
        return new AppSuccess("Vote registered successfully.");
    }

    public static class Factory
    {
        public static OneOf<Candidate, AppError> Create(string email, string name, Election.Election election)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new AppError("Name cannot be null or empty.", AppErrorTypeEnum.BusinessRuleValidationFailure);

            if (name.Length > LengthProperties.PersonNameMaxLength)
                return new AppError($"Name cannot exceed {LengthProperties.PersonNameMaxLength} characters.",
                    AppErrorTypeEnum.BusinessRuleValidationFailure);

            return new Candidate
            {
                Id = Guid.NewGuid(),
                Email = email,
                Name = name,
                TypeOfParticipant = TypeOfParticipantEnum.Candidate,
                ElectionId = election.Id,
                Election = election,
                Votes = 0,
                CreateAt = DateTimeOffset.Now
            };
        }
    }
}