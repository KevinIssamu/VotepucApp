using Domain.ElectionAggregate.Election;
using Domain.Shared;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.UserAggregate.User;
using Microsoft.EntityFrameworkCore;
using OneOf;
using VotepucApp.Persistence.Context;
using VotepucApp.Persistence.Repositories.Events;
using VotepucApp.Persistence.Repositories.Events.Enums;
using VotepucApp.Persistence.Repositories.Events.UserEvents;

namespace VotepucApp.Persistence.Repositories;

//Pretendo trocar o ORM
public class UserAggregateRepository(AppDbContext context, ReadDbContext readDbContext) : IUserRepository
{
    public async Task<OneOf<List<User>, AppError>> SelectPaginatedAsync(int top, int skip, CancellationToken ctCancellationToken)
    {
        try
        {
            var users = await readDbContext.Users.Skip(skip)
                .Take(top)
                .ToListAsync(cancellationToken: ctCancellationToken);

            return users;
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public async Task<OneOf<User, AppError>> SelectByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var userResult = await readDbContext.Users.FirstOrDefaultAsync(e => e.Id == id, cancellationToken: cancellationToken);

            return userResult;
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public async Task<OneOf<User, AppError>> SelectByEmailAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            var userResult = await readDbContext.Users.FirstOrDefaultAsync(e => e.Email == email, cancellationToken: cancellationToken);

            return userResult;
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public async Task<OneOf<List<Election>, AppError>> SelectUserElections(Guid userId, int skip, int take, CancellationToken cancellationToken)
    {
        try
        {
            var userResult = await readDbContext.Users.Where(x => x.Id == userId)
                .SelectMany(x => x.Elections!)
                .Skip(skip)
                .Take(take).ToListAsync(cancellationToken: cancellationToken);

            return userResult;
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public async Task<OneOf<AppSuccess, AppError>> CreateAsync(User aggregateRoot, CancellationToken cancellationToken)
    {
        try
        {
            await context.Users.AddAsync(aggregateRoot, cancellationToken);
            
            return new AppSuccess("User Created");
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public OneOf<AppSuccess, AppError> Update(User aggregateRoot, CancellationToken cancellationToken)
    {
        try
        {
            context.Users.Update(aggregateRoot);
            
            return new AppSuccess("User Updated");
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public OneOf<AppSuccess, AppError> Delete(User aggregateRoot, CancellationToken cancellationToken)
    {
        try
        {
            context.Users.Remove(aggregateRoot);
            
            return new AppSuccess("User Deleted");
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }
}