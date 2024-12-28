using Domain.ElectionAggregate.Election;
using Domain.Shared;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.Shared.Interfaces;
using OneOf;
using VotepucApp.Application.Cases.UseCases.CreateUser;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.Cases.UseCases.UpdateUser;

namespace VotepucApp.Application.BusinessService;

public interface IService<T> where T : IAggregateRoot
{
    Task<OneOf<T, AppError>> SelectByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<OneOf<List<T>, AppError>> SelectPaginatedAsync(int skip, int top, CancellationToken cancellationToken);
    OneOf<AppSuccess, AppError> Delete(T entity, CancellationToken cancellationToken);
}