using Microsoft.EntityFrameworkCore.Storage;

namespace Domain.Shared.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken? cancellationToken = null);
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}