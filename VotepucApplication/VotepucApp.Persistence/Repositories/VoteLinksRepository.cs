using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.VoteLink;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using OneOf;
using VotepucApp.Persistence.Context;

namespace VotepucApp.Persistence.Repositories;

public class VoteLinksRepository(AppDbContext context, ReadDbContext readDbContext) : IVoteLinkRepository
{
    public async Task<OneOf<AppSuccess, AppError>> CreateAsync(List<VoteLink> voteLinks,
        CancellationToken cancellationToken)
    {
        try
        {
            await context.VoteLinks.AddRangeAsync(voteLinks, cancellationToken);

            return new AppSuccess("VoteLinks created successfully");
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public async Task<OneOf<VoteLink, AppError>> GetVoteLinkByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return await readDbContext.VoteLinks.SingleOrDefaultAsync(v => v.Id == id, cancellationToken);
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }
    
    public async Task<OneOf<VoteLink, AppError>> GetVoteLinkByIdTrackingAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return await context.VoteLinks.SingleOrDefaultAsync(v => v.Id == id, cancellationToken);
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }
}