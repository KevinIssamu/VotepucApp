using Domain.ElectionAggregate.Election;
using Domain.UserAggregate.User;
using OneOf;

namespace Domain.Shared;

public interface IUserRepository : IRepository<User>
{
    Task<OneOf<User, AppError.AppError>> SelectByEmailAsync(string email, CancellationToken cancellationToken);
    Task<OneOf<List<Election>, AppError.AppError>> SelectUserElections(Guid userId, int skip, int take, CancellationToken cancellationToken);
}