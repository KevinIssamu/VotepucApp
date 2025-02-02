using Domain.Shared.AppError;
using MediatR;
using VotepucApp.Application.BusinessService;
using VotepucApp.Application.BusinessService.UserService;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.SelectUser.Responses;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.SelectUser.Handlers;

public class SelectUserElectionsHandler(IUserService userService)
    : IRequestHandler<SelectUserElectionsRequest, GenericResponse>
{
    public async Task<GenericResponse> Handle(SelectUserElectionsRequest request,
        CancellationToken cancellationToken)
    {
        var userExistsResult = await userService.SelectByIdAsNoTrackingAsync(request.UserId, cancellationToken);
        if(userExistsResult.IsT1)
            return new GenericResponse(userExistsResult.AsT1.Type.ToHttpStatusCode(), userExistsResult.AsT1.Message);
        
        var userElectionsResult = await userService.SelectUserElectionsAsync(request, cancellationToken);

        if (userElectionsResult.IsT1)
            return new GenericResponse(userElectionsResult.AsT1.Type.ToHttpStatusCode(), userElectionsResult.AsT1.Message);

        var userElectionsViewModel = userElectionsResult.AsT0.Select(x => new ElectionViewModel(x.Id,
                Guid.Parse(x.OwnerId),
                x.Title, x.Description, x.EmailInvitationText, x.MultiVote, x.ElectionStatus, x.Progress, x.StartDate,
                x.EndDate))
            .ToList();

        return new SelectedUserElectionResponse(userElectionsViewModel, 200, "Elections found.");
    }
}