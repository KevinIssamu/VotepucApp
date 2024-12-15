using Domain.Election.Enumerations;
using Domain.Participant.Enumerations;
using Domain.Participant.Messages;
using Domain.Shared;
using Domain.Shared.AppError;
using Domain.Shared.AppError.Constants;
using Domain.Shared.AppSuccess;
using Domain.Shared.SharedValidators;
using OneOf;

namespace Domain.Participant;

public class Candidate : Participant
{
    public int Votes { get; private set; }
    
    public OneOf<AppSuccess, AppError> SetVoted()
    {
        if (Election.Progress != ElectionProgress.Active)
            return new ElectionStatusIsInvalidToSetVote();
        
        Votes++;
        UpdatedAt = DateTime.Now;
        return new VoteSetSuccessfully();
    }
    
    public static class Factory
    {
        public static OneOf<Candidate, AppError> Create(string email, string name, Election.Election election)
        {
            var emailIsValid = GenericInvalidEmailValidator.IsValidEmail(email);
            if (emailIsValid.IsT1) return emailIsValid.AsT1;

            if (name.Length > ConstantsMaxLength.PersonNameMaxLength) return new NameExceededMaxLength();

            return new Candidate()
            {
                Id = Guid.NewGuid(),
                Email = email,
                Name = name,
                TypeOfParticipant = TypeOfParticipant.Candidate,
                ElectionId = election.Id,
                Election = election,
                Votes = 0,
                CreateAt = DateTimeOffset.Now
            };
        }
    }
}