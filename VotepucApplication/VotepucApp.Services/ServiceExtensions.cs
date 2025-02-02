using Domain.Shared.Interfaces;
using Domain.UserAggregate.User;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VotepucApp.Persistence.Context;
using VotepucApp.Persistence.Context.Seeder;
using VotepucApp.Persistence.Interfaces;
using VotepucApp.Persistence.Repositories;
using VotepucApp.Services.AuthenticationsServices;
using VotepucApp.Services.Authorization;
using VotepucApp.Services.Authorization.Handlers;
using VotepucApp.Services.Cache;
using VotepucApp.Services.Email;
using VotepucApp.Services.Interfaces.ConfigInterfaces;
using VotepucApp.Services.RabbitMQ;
using VotepucApp.Services.RabbitMQ.Consumers;
using VotepucApp.Services.RabbitMQ.Interfaces;

namespace VotepucApp.Services;

public static class ServiceExtensions
{
    public static void ConfigurePersistenceApp(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        services.AddDbContext<ReadDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IElectionRepository, ElectionRepository>();
        services.AddScoped<IVoteLinkRepository, VoteLinksRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IClaimsService, ClaimsService>();
        services.AddTransient<ITokenService, TokenService>();

        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<SendEmailsConsumer>();
            
            configurator.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(new Uri("amqp://votepucapp-rabbitmq:5672/"), host =>
                {
                    host.Username("guest");
                    host.Password("guest");
                });

                cfg.ConfigureEndpoints(ctx);
                Console.WriteLine("✅ Consumer configurado e ouvindo mensagens...");
                cfg.ConfigureJsonSerializerOptions(options =>
                {
                    options.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    return options;
                });
            });
        });

        services.AddScoped<IEventPublisher, MassTransitEventPublisher>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "VotepucApp";
        });

        services.AddScoped<ICacheService, CacheService>();
    }

    public static async Task ExecuteSeederAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            var unitOfWork = services.GetRequiredService<IUnitOfWork>();
            var logger = services.GetRequiredService<ILogger<UserSeeder>>();
            var claimService = services.GetRequiredService<IClaimsService>();
            var permissionRepository = services.GetRequiredService<IPermissionRepository>();

            var seeder = new UserSeeder(roleManager, userManager, unitOfWork, logger, permissionRepository,
                claimService);
            await seeder.SeedAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao executar o seeder: {ex.Message}");
        }
    }
}