namespace Domain.Shared;

public interface IBaseRepository<T> where T : BaseEntity
{
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}