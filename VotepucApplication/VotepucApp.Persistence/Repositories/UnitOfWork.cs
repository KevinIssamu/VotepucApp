using Domain.Shared;
using Domain.Shared.Interfaces;
using VotepucApp.Persistence.Context;

namespace VotepucApp.Persistence.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly AppDbContext _context = context;
    
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}