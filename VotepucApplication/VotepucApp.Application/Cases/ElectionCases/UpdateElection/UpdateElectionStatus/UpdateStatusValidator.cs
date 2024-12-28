using Domain.ElectionAggregate.Election.Enumerations;
using FluentValidation;
using VotepucApp.Application.Cases.ElectionCases.Shared;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateElectionStatus;

public class ElectionIdValidator : AbstractValidator<UpdateStatusRequest>
{
    public ElectionIdValidator()
    {
        RuleFor(x => x.ElectionId).NotEmpty();
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid status value. Allowed values are Approved and Rejected.")
            .Must(status => status != ElectionStatusEnum.Pending)
            .WithMessage("You can only change the status to Approved or Rejected.");
    }
}