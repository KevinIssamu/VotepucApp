using Domain.ElectionAggregate.Election.Constants;
using Domain.Shared.Constants;
using FluentValidation;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.ElectionCases.CreateElection;

public class CreateElectionValidator : AbstractValidator<AuthenticatedRequest<CreateElectionViewModel, GenericResponse>>
{
    public CreateElectionValidator()
    {
        RuleFor(x => x.Payload.Title)
            .MaximumLength(ConstantsElectionValidations.TitleMaxLength)
            .NotEmpty();
        
        RuleFor(x => x.Payload.Description).
            MaximumLength(ConstantsElectionValidations.DescriptionMaxLength)
            .NotEmpty();
        
        RuleFor(x => x.Payload.InvitationMessage)
            .MaximumLength(ConstantsElectionValidations.EmailInvitationTextMaxLength)
            .NotEmpty();
        
        RuleFor(x => x.Payload.AllowMultipleVotes)
            .NotNull();
        
        RuleFor(x => x.Payload.StartDate)
            .GreaterThanOrEqualTo(DateTimeOffset.Now)
            .NotEmpty();
        
        RuleFor(x => x.Payload.EndDate)
            .GreaterThanOrEqualTo(x => x.Payload.StartDate)
            .NotEmpty();
        
        RuleFor(x => x.Payload.Participants)
            .NotEmpty()
            .Must(p => p.Count <= LengthProperties.MaxNumberOfParticipants)
            .WithMessage($"The number of participants cannot be greater than {LengthProperties.MaxNumberOfParticipants}")
            .Must(p => p.Select(participant => participant.Email).Distinct().Count() == p.Count)
            .WithMessage("The number of participants cannot be duplicated");
        
        RuleForEach(x => x.Payload.Participants)
            .ChildRules(participant =>
            {
                participant.RuleFor(p => p.Name).NotEmpty().MaximumLength(LengthProperties.PersonNameMaxLength);
                participant.RuleFor(p => p.Email).NotEmpty().EmailAddress();
                participant.RuleFor(p => p.TypeOfParticipant).NotEmpty();
            });
    }
}