using Domain.Shared.AppError;
using MediatR;
using VotepucApp.Application.BusinessService;
using VotepucApp.Application.BusinessService.UserService;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.SelectUser.Responses;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.SelectUser.Handlers;

public class SelectUserElectionsHandler(IUserService userService)
    : IRequestHandler<SelectUserElectionsRequest, SelectedUserElectionResponse>
{
    public async Task<SelectedUserElectionResponse> Handle(SelectUserElectionsRequest request,
        CancellationToken cancellationToken)
    {
        var userElectionsResult = await userService.SelectUserElectionsAsync(request, cancellationToken);

        if (userElectionsResult.IsT1)
        {
            return userElectionsResult.AsT1.Type == AppErrorTypeEnum.SystemError
                ? new SelectedUserElectionResponse(null, 500, userElectionsResult.AsT1.Message)
                : new SelectedUserElectionResponse(null, 404, userElectionsResult.AsT1.Message);
        }

        if (userElectionsResult.AsT0 == null || userElectionsResult.AsT0.Count == 0)
            return new SelectedUserElectionResponse(null, 404, "Elections not found.");

        var userElectionsViewModel = userElectionsResult.AsT0.Select(x => new ElectionViewModel(x.Id,
                Guid.Parse(x.OwnerId),
                x.Title, x.Description, x.EmailInvitationText, x.MultiVote, x.Status, x.Progress, x.StartDate,
                x.EndDate))
            .ToList();

        return new SelectedUserElectionResponse(userElectionsViewModel, 200, "Elections found.");
    }
}