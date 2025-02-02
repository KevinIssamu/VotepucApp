using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VotepucApp.Persistence.Context;

public class ContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=votepucapp-db,1433;Database=votepucdb4;User Id=sa;Password=Kevin#1234;TrustServerCertificate=True;");

        return new AppDbContext(optionsBuilder.Options);
    }
}