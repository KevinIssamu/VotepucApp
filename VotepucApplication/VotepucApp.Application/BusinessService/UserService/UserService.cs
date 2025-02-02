using Domain.ElectionAggregate.Election;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.Shared.Interfaces;
using Domain.UserAggregate.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OneOf;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.UpdateUser;

namespace VotepucApp.Application.BusinessService.UserService;

public class UserService(IUserRepository userRepository, UserManager<User> userManager) : IUserService
{
    public async Task<OneOf<AppSuccess, AppError>> CreateAsync(User user, CancellationToken cancellationToken)
    {
        var newUserResult = await userManager.CreateAsync(user);
        if (!newUserResult.Succeeded)
            return new AppError(newUserResult.Errors.ToString(), AppErrorTypeEnum.BusinessRuleValidationFailure);

        var createResult = await userRepository.CreateAsync(user, cancellationToken);

        return createResult.IsT1 ? createResult.AsT1 : new AppSuccess("User created successfully");
    }
    
    public async Task<OneOf<User, AppError>> SelectByIdTrackingAsync(Guid id,
        CancellationToken cancellationToken)
    {
        var userExistsResult = await userRepository.SelectByIdTrackingAsync(id, cancellationToken);

        if (userExistsResult.IsT1)
            return userExistsResult.AsT1;

        if (userExistsResult.AsT0 == null)
            return new AppError($"User with ID '{id}' was not found.", AppErrorTypeEnum.NotFound);

        return userExistsResult.AsT0;
    }
    
    public async Task<OneOf<User, AppError>> SelectByIdAsNoTrackingAsync(Guid id,
        CancellationToken cancellationToken)
    {
        var userExistsResult = await userRepository.SelectByIdAsNoTrackingAsync(id, cancellationToken);

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

        if (userResult.AsT0 == null || userResult.AsT0.Count == 0)
            return new AppError("No users found for the given parameters.", AppErrorTypeEnum.NotFound);

        return userResult.AsT0;
    }

    public async Task<OneOf<User, AppError>> SelectUserByEmailAsync(string email)
    {
        var userExistsResult = await userManager.FindByEmailAsync(email);

        if (userExistsResult == null)
            return new AppError($"User with email '{email}' was not found.", AppErrorTypeEnum.NotFound);

        return userExistsResult;
    }

    public async Task<OneOf<List<Election>, AppError>> SelectUserElectionsAsync(SelectUserElectionsRequest request,
        CancellationToken cancellationToken)
    {
        var userElectionsResult =
            await userRepository.SelectUserElections(request.UserId, request.Skip, request.Take, cancellationToken);

        if (userElectionsResult.IsT1)
            return userElectionsResult.AsT1;

        if (userElectionsResult.AsT0 != null || userElectionsResult.AsT0.Count == 0)
            return new AppError("User has no elections.", AppErrorTypeEnum.NotFound);
        
        return userElectionsResult.AsT0;
}

    public OneOf<AppSuccess, AppError> Delete(User user,
        CancellationToken cancellationToken)
    {
        var deleteUserResult = userRepository.Delete(user, cancellationToken);
        
        if(deleteUserResult.IsT1)
            return deleteUserResult.AsT1;
        
        return deleteUserResult.AsT0;
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