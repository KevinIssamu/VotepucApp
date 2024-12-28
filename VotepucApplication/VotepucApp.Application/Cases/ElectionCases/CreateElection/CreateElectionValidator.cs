using Domain.ElectionAggregate.Election.Constants;
using Domain.Shared.Constants;
using FluentValidation;

namespace VotepucApp.Application.Cases.ElectionCases.CreateElection;

public class CreateElectionValidator : AbstractValidator<CreateElectionRequest>
{
    public CreateElectionValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(ConstantsElectionValidations.TitleMaxLength)
            .NotEmpty();
        
        RuleFor(x => x.Description).
            MaximumLength(ConstantsElectionValidations.DescriptionMaxLength)
            .NotEmpty();
        
        RuleFor(x => x.InvitationMessage)
            .MaximumLength(ConstantsElectionValidations.EmailInvitationTextMaxLength)
            .NotEmpty();
        
        RuleFor(x => x.AllowMultipleVotes)
            .NotNull();
        
        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTimeOffset.Now)
            .NotEmpty();
        
        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .NotEmpty();
        
        RuleFor(x => x.Participantes)
            .NotEmpty()
            .Must(p => p.Count <= LengthProperties.MaxNumberOfParticipants)
            .WithMessage($"The number of participants cannot be greater than {LengthProperties.MaxNumberOfParticipants}")
            .Must(p => p.Select(participant => participant.Email).Distinct().Count() == p.Count)
            .WithMessage("The number of participants cannot be duplicated");
        
        RuleForEach(x => x.Participantes)
            .ChildRules(participant =>
            {
                participant.RuleFor(p => p.Name).NotEmpty().MaximumLength(LengthProperties.PersonNameMaxLength);
                participant.RuleFor(p => p.Email).NotEmpty().EmailAddress();
                participant.RuleFor(p => p.TypeOfParticipant).NotEmpty();
            });

    }
}