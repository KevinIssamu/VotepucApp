using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Participant;
using Domain.Shared;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using OneOf;
using VotepucApp.Persistence.Context;
using VotepucApp.Persistence.Repositories.Events.Enums;

namespace VotepucApp.Persistence.Repositories;

//Pretendo trocar o ORM e por isso estou utilizando repository
public class ElectionAggregateRepository(AppDbContext context, ReadDbContext readDbContext) : IElectionRepository
{
    public async Task<OneOf<List<Election>, AppError>> SelectPaginatedAsync(int top, int skip,
        CancellationToken cancellationToken)
    {
        try
        {
            var elections = await readDbContext.Elections
                .Skip(skip)
                .Take(top)
                .ToListAsync(cancellationToken);

            return elections;
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public async Task<OneOf<Election, AppError>> SelectByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var election = await readDbContext.Elections.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (election == null)
            {
                return new AppError("Election not found", AppErrorTypeEnum.NotFound);
            }

            return election;
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public async Task<OneOf<AppSuccess, AppError>> CreateAsync(Election aggregateRoot,
        CancellationToken cancellationToken)
    {
        try
        {
            await context.Elections.AddAsync(aggregateRoot, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return new AppSuccess("Election created successfully");
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public OneOf<AppSuccess, AppError> Update(Election aggregateRoot, CancellationToken cancellationToken)
    {
        try
        {
            context.Elections.Update(aggregateRoot);
            context.SaveChanges();

            return new AppSuccess("Election updated successfully");
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public OneOf<AppSuccess, AppError> Delete(Election aggregateRoot, CancellationToken cancellationToken)
    {
        try
        {
            context.Elections.Remove(aggregateRoot);

            return new AppSuccess("Election deleted successfully");
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public async Task<OneOf<List<Participant>, AppError>> SelectElectionParticipants(Guid electionId,
        CancellationToken cancellationToken)
    {
        try
        {
            return await readDbContext.Participants.Where(p => p.ElectionId == electionId)
                .ToListAsync(cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public async Task<OneOf<int, AppError>> CountElectionParticipants(Guid electionId,
        CancellationToken cancellationToken)
    {
        try
        {
            return await readDbContext.Participants.Where(p => p.ElectionId == electionId)
                .CountAsync(cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }
}