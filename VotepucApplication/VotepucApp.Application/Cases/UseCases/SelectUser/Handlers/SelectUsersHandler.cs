using Domain.Shared.AppError;
using Domain.UserAggregate.User;
using MediatR;
using VotepucApp.Application.BusinessService;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.SelectUser.Handlers;

public class SelectUsersHandler(IUserService userService) : IRequestHandler<SelectUsersRequest, SelectedUsersResponse>
{
    public async Task<SelectedUsersResponse> Handle(SelectUsersRequest request, CancellationToken cancellationToken)
    {
        var selectUsersResult = await userService.SelectPaginatedAsync(request, cancellationToken);

        if (selectUsersResult.IsT1)
            return new SelectedUsersResponse(null, selectUsersResult.AsT1);
        if(selectUsersResult.AsT0.Count == 0 || selectUsersResult.AsT0 == null)
            return new SelectedUsersResponse(null, new AppError("User not found.", AppErrorTypeEnum.NotFound));
        
        var usersViewModel = selectUsersResult.AsT0
            .Select(x => new UserResponseViewModel(x.Id, x.Email, x.Name, x.TypeOfUser))
            .ToList();
        
        return new SelectedUsersResponse(usersViewModel, null);
    }
}