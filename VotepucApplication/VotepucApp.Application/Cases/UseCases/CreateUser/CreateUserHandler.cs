using Domain.Shared.AppError;
using Domain.Shared.Interfaces;
using MediatR;
using VotepucApp.Application.BusinessService.UserService;
using VotepucApp.Application.Cases.AuthCases.AddUserToRole;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.CreateUser;

public class CreateUserHandler(IUserService userService, IMediator mediator, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateUserRequest, UserResponse>
{
    public async Task<UserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var createUserResult = await userService.CreateAsync(request, cancellationToken);

        if (createUserResult.IsT1)
        {
            return createUserResult.AsT1.Type == AppErrorTypeEnum.SystemError
                ? new UserResponse(null, 500, createUserResult.AsT1.Message)
                : new UserResponse(null, 400, createUserResult.AsT1.Message);
        }

        var user = createUserResult.AsT0;

        var addUserToRoleRequest = new AddUserToRoleRequest(user.Email!, request.RoleName);
        var addUserToRoleMediator = await mediator.Send(addUserToRoleRequest, cancellationToken);

        return addUserToRoleMediator.StatusCode is >= 200 and < 300
            ? new UserResponse(new UserResponseViewModel(Guid.Parse(user.Id), user.Email!, user.UserName!),
                addUserToRoleMediator.StatusCode, addUserToRoleMediator.Message)
            : new UserResponse(null, addUserToRoleMediator.StatusCode, addUserToRoleMediator.Message);
    }
}