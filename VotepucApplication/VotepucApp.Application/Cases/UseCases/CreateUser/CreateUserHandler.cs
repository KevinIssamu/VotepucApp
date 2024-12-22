using Domain.Shared;
using Domain.Shared.AppError;
using Domain.UserAggregate.User;
using MediatR;
using OneOf;
using VotepucApp.Application.BusinessService;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.CreateUser;

public class CreateUserHandler(IUserService userService)
    : IRequestHandler<CreateUserRequest, UserResponse>
{
    
    public async Task<UserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var createUserResult = await userService.CreateAsync(request, cancellationToken);

        if (createUserResult.IsT1)
            return new UserResponse(null, createUserResult.AsT1);

        var user = createUserResult.AsT0;

        return new UserResponse(new UserResponseViewModel(user.Id, user.Email, user.Name, user.TypeOfUser), null);
    }
}