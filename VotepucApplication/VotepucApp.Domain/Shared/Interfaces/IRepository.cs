using OneOf;

namespace Domain.Shared.Interfaces;

public interface IRepository<T> where T : IAggregateRoot
{
    Task<OneOf<List<T>, AppError.AppError>> SelectPaginatedAsync(int top, int skip, CancellationToken cancellationToken);
    Task<OneOf<T, AppError.AppError>> SelectByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<OneOf<AppSuccess.AppSuccess, AppError.AppError>> CreateAsync(T aggregateRoot, CancellationToken cancellationToken);
    OneOf<AppSuccess.AppSuccess, AppError.AppError> Update(T aggregateRoot, CancellationToken cancellationToken);
    OneOf<AppSuccess.AppSuccess, AppError.AppError> Delete(T aggregateRoot, CancellationToken cancellationToken);
}