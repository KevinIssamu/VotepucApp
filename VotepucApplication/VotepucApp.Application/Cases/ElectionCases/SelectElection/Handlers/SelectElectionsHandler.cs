using Domain.Shared.AppError;
using MediatR;
using VotepucApp.Application.BusinessService.ElectionService;
using VotepucApp.Application.Cases.ElectionCases.SelectElection.Requests;
using VotepucApp.Application.Cases.ElectionCases.SelectElection.Responses;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.ViewModels;
using VotepucApp.Services.Interfaces;

namespace VotepucApp.Application.Cases.ElectionCases.SelectElection.Handlers;

public class SelectElectionsHandler(IElectionService electionService)
    : IRequestHandler<SelectElectionsRequest, SelectedElectionsResponse>
{
    public async Task<SelectedElectionsResponse> Handle(SelectElectionsRequest request,
        CancellationToken cancellationToken)
    {
        var selectUsersResult =
            await electionService.SelectPaginatedAsync(request.Skip, request.Top, cancellationToken);

        if (selectUsersResult.IsT1)
        {
            return selectUsersResult.AsT1.Type == AppErrorTypeEnum.SystemError
                ? new SelectedElectionsResponse(null, 500, selectUsersResult.AsT1.Message)
                : new SelectedElectionsResponse(null, 404, selectUsersResult.AsT1.Message);
        }

        if (selectUsersResult.AsT0.Count == 0 || selectUsersResult.AsT0 == null)
            return new SelectedElectionsResponse(null, 404, "No elections found.");

        var electionViewModel = selectUsersResult.AsT0
            .Select(x => new ElectionViewModel(x.Id, Guid.Parse(x.OwnerId), x.Title, x.Description,
                x.EmailInvitationText, x.MultiVote, x.ElectionStatus, x.Progress, x.StartDate, x.EndDate))
            .ToList();

        return new SelectedElectionsResponse(electionViewModel, 200, $"Total elections: {electionViewModel.Count}");
    }
}