using Domain.ElectionAggregate.Election.Enumerations;
using FluentValidation;

namespace VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateElectionProgress;

public class UpdateProgressValidator : AbstractValidator<UpdateProgressRequest>
{
    public UpdateProgressValidator()
    {
        RuleFor(x => x.ElectionId).NotEmpty();
        RuleFor(x => x.Progress)
            .IsInEnum()
            .WithMessage("Invalid progress value. Allowed values are Active and Finished.")
            .Must(status => status != ElectionProgressEnum.Inactive)
            .WithMessage("You can only change the status to Active and Finished.");
    }
}