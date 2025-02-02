using Domain.ElectionAggregate.Election;
using Domain.UserAggregate.User;
using OneOf;

namespace Domain.Shared.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<OneOf<List<Election>, AppError.AppError>> SelectUserElections(Guid userId, int skip, int take, CancellationToken cancellationToken);
}