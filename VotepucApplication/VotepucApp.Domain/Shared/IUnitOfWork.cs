namespace Domain.Shared;

internal interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken);
}