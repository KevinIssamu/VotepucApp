using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Participant;
using Domain.ElectionAggregate.VoteLink;
using Domain.UserAggregate.User;
using Microsoft.EntityFrameworkCore;

namespace VotepucApp.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Election> Elections { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<VoteLink> VoteLinks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}