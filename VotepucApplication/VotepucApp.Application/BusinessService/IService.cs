using Domain.ElectionAggregate.Election;
using Domain.Shared;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using OneOf;
using VotepucApp.Application.Cases.UseCases.CreateUser;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.Cases.UseCases.UpdateUser;

namespace VotepucApp.Application.BusinessService;

public interface IService<T> where T : IAggregateRoot
{
    Task<OneOf<T, AppError>> CreateAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken);
    Task<OneOf<T, AppError>> SelectByIdAsync(SelectUserByIdRequest<UserResponse> createUserRequest, CancellationToken cancellationToken);
    Task<OneOf<List<T>, AppError>> SelectPaginatedAsync(SelectUsersRequest request, CancellationToken cancellationToken);
    Task<OneOf<AppSuccess, AppError>> DeleteAsync(SelectUserByIdRequest<OperationUserResponse> request, CancellationToken cancellationToken);
    Task<OneOf<AppSuccess, AppError>> UpdateAsync(UpdateUserRequest request, CancellationToken cancellationToken);
}