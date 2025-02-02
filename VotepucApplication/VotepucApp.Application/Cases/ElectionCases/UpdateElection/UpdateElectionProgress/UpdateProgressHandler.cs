using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Election.Enumerations;
using Domain.Shared.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using VotepucApp.Application.BusinessService.ElectionService;
using VotepucApp.Application.BusinessService.VoteLinkService;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Services.RabbitMQ.Events;
using VotepucApp.Services.RabbitMQ.Interfaces;

namespace VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateElectionProgress;

public class UpdateProgressHandler(
    IElectionService electionService,
    VoteLinkService voteLinkService,
    IUnitOfWork unitOfWork,
    IEventPublisher eventPublisher,
    IConfiguration configuration
)
    : BaseHandler(configuration), IRequestHandler<UpdateProgressRequest, GenericResponse>
{
    private readonly IConfiguration _configuration = configuration;

    public async Task<GenericResponse> Handle(UpdateProgressRequest request, CancellationToken cancellationToken)
    {
        var electionResult = await electionService.SelectByIdTrackingAsync(request.ElectionId, cancellationToken);

        if (electionResult.IsT1)
            return CreateAppErrorResponse(electionResult.AsT1, null);

        if (electionResult.AsT0.ElectionStatus != ElectionStatusEnum.Approved)
        {
            return new GenericResponse(400, "Election must be approved.",
            [
                new Link("change-election-status", $"election/{request.ElectionId}/status", "PATCH", _configuration)
            ]);
        }

        var election = electionResult.AsT0;

        return request.Progress switch
        {
            ElectionProgressEnum.Active => await HandleActiveElectionAsync(election, cancellationToken),
            ElectionProgressEnum.Finish => await HandleFinishedElectionAsync(election, cancellationToken),
            _ => new GenericResponse(400, "Invalid election progress. Only 'Active' and 'Finish' progress are allowed.",
                null)
        };
    }

    private async Task<GenericResponse> HandleActiveElectionAsync(Election election,
        CancellationToken cancellationToken)
    {
        var startingResult = electionService.Starting(election);
        if (startingResult.IsT1)
            return CreateAppErrorResponse(startingResult.AsT1);

        var createLinksResult = await voteLinkService.CreateLinks(election, cancellationToken);
        if (createLinksResult.IsT1)
            return CreateAppErrorResponse(createLinksResult.AsT1);
        
        await unitOfWork.CommitAsync(cancellationToken);

        var sendEmailsEvent = new SendEmailsEvent(Guid.NewGuid(), createLinksResult.AsT0);

        await eventPublisher.Publish(sendEmailsEvent);

        await unitOfWork.CommitAsync(cancellationToken);

        var listLinks = new List<Link>()
        {
            new("finish-election", $"Election/{election.Id}/finish", "PATCH", _configuration),
            CreateCrudLink("get", $"election", election.Id.ToString()),
            CreateCrudLink("delete", $"Election", election.Id.ToString())
        };

        return new GenericResponse(202, $"TaskId: {sendEmailsEvent.TaskId}", listLinks);
    }

    private async Task<GenericResponse> HandleFinishedElectionAsync(Election election,
        CancellationToken cancellationToken)
    {
        var approvedElection = election.GetBehavior<ApprovedElectionBehavior>();

        if (approvedElection.IsT1)
            return CreateAppErrorResponse(approvedElection.AsT1);

        var finishResult = approvedElection.AsT0.Finish();

        if (finishResult.IsT1)
            return CreateAppErrorResponse(finishResult.AsT1);

        var voteLinks = await electionService.SelectElectionVoteLinksAsync(election.Id, cancellationToken);

        if (voteLinks.IsT1)
            return CreateAppErrorResponse(voteLinks.AsT1);

        var invalidateLinksResult = voteLinkService.InvalidateVoteLinks(voteLinks.AsT0);

        if (invalidateLinksResult.IsT1)
            return CreateAppErrorResponse(invalidateLinksResult.AsT1);

        await unitOfWork.CommitAsync(cancellationToken);

        var listLinks = new List<Link>()
        {
            CreateCrudLink("get", "election", election.Id.ToString()),
            CreateCrudLink("delete", "election", election.Id.ToString())
        };

        return CreateSuccessResponse(finishResult.AsT0.Message, listLinks);
    }
}