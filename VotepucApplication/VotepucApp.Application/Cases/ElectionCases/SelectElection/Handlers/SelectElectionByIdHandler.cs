using Domain.Shared.AppError;
using MediatR;
using Microsoft.Extensions.Configuration;
using VotepucApp.Application.BusinessService.ElectionService;
using VotepucApp.Application.Cases.ElectionCases.SelectElection.Requests;
using VotepucApp.Application.Cases.ElectionCases.SelectElection.Responses;
using VotepucApp.Application.Cases.ElectionCases.Shared;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.ViewModels;
using VotepucApp.Services.Interfaces;

namespace VotepucApp.Application.Cases.ElectionCases.SelectElection.Handlers;

public class SelectElectionByIdHandler(IElectionService electionService, IConfiguration configuration)
    : BaseHandler(configuration), IRequestHandler<ElectionIdRequest<SelectElectionByIdResponse>, SelectElectionByIdResponse>
{
    public async Task<SelectElectionByIdResponse> Handle(ElectionIdRequest<SelectElectionByIdResponse> request,
        CancellationToken cancellationToken)
    {
        var selectElectionResult = await electionService.SelectByIdAsNoTrackingAsync(request.Id, cancellationToken);

        if (selectElectionResult.IsT1)
        {
            return selectElectionResult.AsT1.Type == AppErrorTypeEnum.SystemError
                ? new SelectElectionByIdResponse(500, selectElectionResult.AsT1.Message, null)
                : new SelectElectionByIdResponse(400, selectElectionResult.AsT1.Message, null);
        }

        if (selectElectionResult.AsT0 == null)
            return new SelectElectionByIdResponse(404, "Elections not found.", null);

        var election = selectElectionResult.AsT0;

        return new SelectElectionByIdResponse(200, "Elections found.",
            new ElectionViewModel(election.Id, Guid.Parse(election.OwnerId), election.Title, election.Description,
                election.EmailInvitationText, election.MultiVote, election.ElectionStatus, election.Progress,
                election.StartDate, election.EndDate));
    }
}