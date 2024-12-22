using Domain.ElectionAggregate.Election;
using Domain.Shared;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.UserAggregate.User;
using OneOf;
using VotepucApp.Application.Cases.UseCases.CreateUser;
using VotepucApp.Application.Cases.UseCases.DeleteUser;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.Cases.UseCases.UpdateUser;

namespace VotepucApp.Application.BusinessService;

public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork) : IUserService
{
    public async Task<OneOf<User, AppError>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userExistsResult = await SelectByEmailAsync(new SelectUserByEmailRequest(request.Email), cancellationToken);

        if (userExistsResult.IsT0)
            return new AppError($"A user with email '{request.Email}' already exists.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        var userResult = User.Factory.Create(request.Name, request.Email, request.Password, null);

        if (userResult.IsT1)
            return userResult.AsT1;

        var user = userResult.AsT0;

        await userRepository.CreateAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return user;
    }

    public async Task<OneOf<User, AppError>> SelectByIdAsync(SelectUserByIdRequest<UserResponse> request,
        CancellationToken cancellationToken)
    {
        var userExistsResult = await userRepository.SelectByIdAsync(request.Id, cancellationToken);

        if (userExistsResult.IsT1)
            return userExistsResult.AsT1;

        if (userExistsResult.AsT0 == null)
            return new AppError($"User with ID '{request.Id}' was not found.", AppErrorTypeEnum.NotFound);

        return userExistsResult.AsT0;
    }

    public async Task<OneOf<List<User>, AppError>> SelectPaginatedAsync(SelectUsersRequest request,
        CancellationToken cancellationToken)
    {
        var userResult = await userRepository.SelectPaginatedAsync(request.Top, request.Skip, cancellationToken);

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

    public async Task<OneOf<AppSuccess, AppError>> DeleteAsync(SelectUserByIdRequest<OperationUserResponse> request,
        CancellationToken cancellationToken)
    {
        var selectUserByIdResult = await userRepository.SelectByIdAsync(request.Id, cancellationToken);

        if (selectUserByIdResult.IsT1)
            return selectUserByIdResult.AsT1;

        if (selectUserByIdResult.AsT0 == null)
            return new AppError($"User with ID '{request.Id}' was not found.", AppErrorTypeEnum.NotFound);

        userRepository.Delete(selectUserByIdResult.AsT0, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new AppSuccess("User successfully deleted.");
    }

    public async Task<OneOf<AppSuccess, AppError>> UpdateAsync(UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var findUserResult = await userRepository.SelectByIdAsync(request.Id, cancellationToken);

        if (findUserResult.IsT1)
            return findUserResult.AsT1;

        if (findUserResult.AsT0 == null)
            return new AppError($"User with ID '{request.Id}' was not found.", AppErrorTypeEnum.NotFound);

        var newUserName = findUserResult.AsT0.SetName(request.UserRequestUpdated.UserName);
        var newUserEmail = findUserResult.AsT0.SetEmail(request.UserRequestUpdated.Email);
        var newUserType = findUserResult.AsT0.SetUserType(request.UserRequestUpdated.UserType);

        if (!(newUserName && newUserEmail && newUserType))
            return new AppError("Dados inválidos.", AppErrorTypeEnum.BusinessRuleValidationFailure);

        userRepository.Update(findUserResult.AsT0, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new AppSuccess("User successfully updated.");
    }
}