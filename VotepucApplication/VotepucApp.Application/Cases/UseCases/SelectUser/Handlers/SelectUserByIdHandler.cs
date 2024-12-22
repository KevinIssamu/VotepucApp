using Domain.Shared.AppError;
using Domain.UserAggregate.User;
using MediatR;
using VotepucApp.Application.BusinessService;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.SelectUser.Handlers;

public class SelectUserByIdHandler(IUserService userService) : IRequestHandler<SelectUserByIdRequest<UserResponse>, UserResponse>
{
    public async Task<UserResponse> Handle(SelectUserByIdRequest<UserResponse> request, CancellationToken cancellationToken)
    {
        var selectUserByIdResult = await userService.SelectByIdAsync(request, cancellationToken);

        if (selectUserByIdResult.IsT1)
            return new UserResponse(null, selectUserByIdResult.AsT1);

        if (selectUserByIdResult.AsT0 == null)
            return new UserResponse(null, new AppError("User not found", AppErrorTypeEnum.NotFound));

        var user = selectUserByIdResult.AsT0;

        return new UserResponse(new UserResponseViewModel(user.Id, user.Email, user.Name, user.TypeOfUser), null);
    }
}