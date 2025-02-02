using Domain.Shared.AppError;
using Domain.Shared.Interfaces;
using Domain.UserAggregate.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using VotepucApp.Application.BusinessService.UserService;
using VotepucApp.Application.Cases.AuthCases.AddUserToRole;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.CreateUser;

public class CreateUserHandler(IUserService userService, IMediator mediator, UserManager<User> userManager, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateUserRequest, GenericResponse>
{
    public async Task<GenericResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userExistsResult = await userManager.FindByEmailAsync(request.Email);
        
        if(userExistsResult != null)
            return new GenericResponse(400, "User already exists");
        
        var newUser = User.Factory.Create(request.Name, request.Email, null);

        if (newUser.IsT1)
            return new GenericResponse(newUser.AsT1.Type.ToHttpStatusCode(), newUser.AsT1.Message);
        
        var createUserResult = await userService.CreateAsync(newUser.AsT0, cancellationToken);

        if (createUserResult.IsT1)
            return new GenericResponse(createUserResult.AsT1.Type.ToHttpStatusCode(), createUserResult.AsT1.Message);

        var addUserToRoleRequest = new AddUserToRoleRequest(newUser.AsT0.Email!, request.RoleName);
        var addUserToRoleMediator = await mediator.Send(addUserToRoleRequest, cancellationToken);

        return addUserToRoleMediator.StatusCode is >= 200 and < 300
            ? new UserResponse(new UserResponseViewModel(Guid.Parse(newUser.AsT0.Id), newUser.AsT0.Email!, newUser.AsT0.UserName!),
                addUserToRoleMediator.StatusCode, addUserToRoleMediator.Message)
            : new UserResponse(null, addUserToRoleMediator.StatusCode, addUserToRoleMediator.Message);
    }
}