using Domain.ElectionAggregate.Election;
using Domain.Shared;
using Domain.UserAggregate.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VotepucApp.Persistence.Context;
using VotepucApp.Persistence.Repositories;

namespace VotepucApp.Persistence;

public static class ServiceExtensions
{
    public static void ConfigurePersistenceApp(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        services.AddDbContext<ReadDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserAggregateRepository>();
        services.AddScoped<IElectionRepository, ElectionAggregateRepository>();
    }
}