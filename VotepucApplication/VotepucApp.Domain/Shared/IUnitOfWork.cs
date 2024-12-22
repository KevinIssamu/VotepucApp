namespace Domain.Shared;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}