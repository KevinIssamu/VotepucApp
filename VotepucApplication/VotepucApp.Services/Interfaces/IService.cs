using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.Shared.Interfaces;
using OneOf;

namespace VotepucApp.Services.Interfaces;

public interface IService<T> where T : IAggregateRoot
{
    Task<OneOf<T, AppError>> SelectByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken);
    Task<OneOf<T, AppError>> SelectByIdTrackingAsync(Guid id, CancellationToken cancellationToken);
    Task<OneOf<List<T>, AppError>> SelectPaginatedAsync(int skip, int top, CancellationToken cancellationToken);
    OneOf<AppSuccess, AppError> Delete(T entity, CancellationToken cancellationToken);
}