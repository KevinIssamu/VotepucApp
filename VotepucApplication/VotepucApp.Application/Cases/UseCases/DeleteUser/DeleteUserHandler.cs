using Domain.UserAggregate.User;
using MediatR;
using VotepucApp.Application.BusinessService;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;

namespace VotepucApp.Application.Cases.UseCases.DeleteUser;

public class DeleteUserHandler(IUserService userService)
    : IRequestHandler<SelectUserByIdRequest<OperationUserResponse>, OperationUserResponse>
{
    public async Task<OperationUserResponse> Handle(SelectUserByIdRequest<OperationUserResponse> request,
        CancellationToken cancellationToken)
    {
        var deleteUserResult = await userService.DeleteAsync(request, cancellationToken);

        return deleteUserResult.IsT1
            ? new OperationUserResponse(null, deleteUserResult.AsT1)
            : new OperationUserResponse(deleteUserResult.AsT0, null);
    }
}