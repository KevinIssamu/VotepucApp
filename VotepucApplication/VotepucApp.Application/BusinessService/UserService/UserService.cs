using Domain.ElectionAggregate.Election;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.Shared.Interfaces;
using Domain.UserAggregate.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OneOf;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.Cases.UseCases.CreateUser;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.Cases.UseCases.UpdateUser;

namespace VotepucApp.Application.BusinessService.UserService;

public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IMediator mediator, UserManager<User> userManager) : IUserService
{
    public async Task<OneOf<AppSuccess, AppError>> CreateAsync(User user, CancellationToken cancellationToken)
    {
        var createResult = await userRepository.CreateAsync(user, cancellationToken);
        
        return createResult.IsT1 ? createResult.AsT1 : new AppSuccess("User created successfully");
    }

    public async Task<OneOf<User, AppError>> SelectByIdAsync(Guid id,
        CancellationToken cancellationToken)
    {
        var userExistsResult = await userRepository.SelectByIdAsync(id, cancellationToken);

        if (userExistsResult.IsT1)
            return userExistsResult.AsT1;

        if (userExistsResult.AsT0 == null)
            return new AppError($"User with ID '{id}' was not found.", AppErrorTypeEnum.NotFound);

        return userExistsResult.AsT0;
    }

    public async Task<OneOf<List<User>, AppError>> SelectPaginatedAsync(int skip, int top,
        CancellationToken cancellationToken)
    {
        var userResult = await userRepository.SelectPaginatedAsync(top, skip, cancellationToken);

        if (userResult.IsT1)
            return userResult.AsT1;

        if (userResult.AsT0.Count == 0)
            return new AppError("No users found for the given parameters.", AppErrorTypeEnum.NotFound);

        return userResult.AsT0;
    }

    public async Task<OneOf<User, AppError>> SelectByEmailAsync(SelectUserByEmailRequest request,
        CancellationToken cancellationToken)
    {
        var userExistsResult = await userRepository.SelectByEmailAsync(request.Email, cancellationToken);

        if (userExistsResult.IsT1)
            return userExistsResult.AsT1;

        if (userExistsResult.AsT0 == null)
            return new AppError($"User with email '{request.Email}' was not found.", AppErrorTypeEnum.NotFound);

        return userExistsResult.AsT0;
    }

    public async Task<OneOf<List<Election>, AppError>> SelectUserElectionsAsync(SelectUserElectionsRequest request,
        CancellationToken cancellationToken)
    {
        var userExistsResult = await userRepository.SelectByIdAsync(request.UserId, cancellationToken);
        
        if(userExistsResult.IsT1)
            return userExistsResult.AsT1;
        
        if(userExistsResult.AsT0 == null)
            return new AppError($"User with ID '{request.UserId}' was not found.", AppErrorTypeEnum.NotFound);
        
        var userElectionsResult =
            await userRepository.SelectUserElections(request.UserId, request.Skip, request.Take, cancellationToken);

        if (userElectionsResult.IsT1)
            return userElectionsResult.AsT1;

        return userElectionsResult.AsT0;
    }

    public async Task<OneOf<AppSuccess, AppError>> DeleteAsync(Guid id,
        CancellationToken cancellationToken)
    {
        var selectUserByIdResult = await userRepository.SelectByIdAsync(id, cancellationToken);

        if (selectUserByIdResult.IsT1)
            return selectUserByIdResult.AsT1;

        if (selectUserByIdResult.AsT0 == null)
            return new AppError($"User with ID '{id}' was not found.", AppErrorTypeEnum.NotFound);

        userRepository.Delete(selectUserByIdResult.AsT0, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new AppSuccess("User successfully deleted.");
    }

    public async Task<OneOf<AppSuccess, AppError>> UpdateAsync(User user, UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        if (user.Id != request.Id.ToString())
            return new AppError("The user ID and request ID must match.", AppErrorTypeEnum.BusinessRuleValidationFailure);

        var newUserName = request.UserRequestUpdated.UserName;

        if (user.UserName == newUserName)
            return new AppError($"The user name already is {newUserName}", AppErrorTypeEnum.ValidationFailure);

        var updateUserName = await userManager.SetUserNameAsync(user, newUserName);
        
        if(!updateUserName.Succeeded)
            return new AppError(updateUserName.Errors.ToString(), AppErrorTypeEnum.BusinessRuleValidationFailure);
        
        var updateUserResult = userRepository.Update(user, cancellationToken);

        if (updateUserResult.IsT1)
            return updateUserResult.AsT1;

        return new AppSuccess("User successfully updated.");
    }
}