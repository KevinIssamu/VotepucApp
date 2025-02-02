using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Participant;
using Domain.ElectionAggregate.VoteLink;
using Domain.UserAggregate.Permissions;
using Domain.UserAggregate.User;
using Microsoft.EntityFrameworkCore;

namespace VotepucApp.Persistence.Context;

public sealed class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Election> Elections { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<VoteLink> VoteLinks { get; set; }
    public DbSet<Permission> Permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        throw new InvalidOperationException("Operações de gravação não são permitidas neste contexto de leitura.");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("Operações de gravação não são permitidas neste contexto de leitura.");
    }
}