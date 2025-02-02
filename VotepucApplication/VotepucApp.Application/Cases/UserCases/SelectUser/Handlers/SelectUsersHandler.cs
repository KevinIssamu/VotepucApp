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
    : IRequestHandler<GenericEntiesRequest<GenericResponse>, GenericResponse>
{
    public async Task<GenericResponse> Handle(GenericEntiesRequest<GenericResponse> request,
        CancellationToken cancellationToken)
    {
        var selectUsersResult = await userService.SelectPaginatedAsync(request.Skip, request.Top, cancellationToken);

        if (selectUsersResult.IsT1)
            return new GenericResponse(selectUsersResult.AsT1.Type.ToHttpStatusCode(), selectUsersResult.AsT1.Message);

        var usersViewModelList = selectUsersResult.AsT0
            .Select(x => new UserResponseViewModel(Guid.Parse(x.Id), x.Email!, x.UserName!))
            .ToList();

        return new SelectedUsersResponse(usersViewModelList, 200, $"Total users: {usersViewModelList.Count}");
    }
}