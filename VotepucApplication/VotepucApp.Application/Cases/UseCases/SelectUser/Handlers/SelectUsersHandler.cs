using Domain.Shared.AppError;
using Domain.UserAggregate.User;
using MediatR;
using VotepucApp.Application.BusinessService;
using VotepucApp.Application.BusinessService.UserService;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.SelectUser.Handlers;

public class SelectUsersHandler(IUserService userService)
    : IRequestHandler<GenericEntiesRequest<SelectedUsersResponse>, SelectedUsersResponse>
{
    public async Task<SelectedUsersResponse> Handle(GenericEntiesRequest<SelectedUsersResponse> request,
        CancellationToken cancellationToken)
    {
        var selectUsersResult = await userService.SelectPaginatedAsync(request.Skip, request.Top, cancellationToken);

        if (selectUsersResult.IsT1)
        {
            return selectUsersResult.AsT1.Type == AppErrorTypeEnum.SystemError
                ? new SelectedUsersResponse(null, 500, selectUsersResult.AsT1.Message)
                : new SelectedUsersResponse(null, 404, selectUsersResult.AsT1.Message);
        }

        if (selectUsersResult.AsT0.Count == 0 || selectUsersResult.AsT0 == null)
            return new SelectedUsersResponse(null, 404, "No users found.");

        var usersViewModel = selectUsersResult.AsT0
            .Select(x => new UserResponseViewModel(Guid.Parse(x.Id), x.Email, x.UserName))
            .ToList();

        return new SelectedUsersResponse(usersViewModel, 200, $"Total users: {usersViewModel.Count}");
    }
}