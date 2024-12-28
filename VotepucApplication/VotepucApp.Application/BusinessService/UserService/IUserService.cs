using Domain.ElectionAggregate.Election;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.UserAggregate.User;
using OneOf;
using VotepucApp.Application.Cases.UseCases.CreateUser;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.UpdateUser;

namespace VotepucApp.Application.BusinessService.UserService;

public interface IUserService : IService<User>
{
    Task<OneOf<List<Election>, AppError>> SelectUserElectionsAsync(SelectUserElectionsRequest request, CancellationToken cancellationToken);
    Task<OneOf<AppSuccess, AppError>> CreateAsync(User request, CancellationToken cancellationToken);
    Task<OneOf<AppSuccess, AppError>> UpdateAsync(User user, UpdateUserRequest request, CancellationToken cancellationToken);
}