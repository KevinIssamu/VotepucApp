using System.Reflection;
using Domain.UserAggregate.User;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VotepucApp.Application.AuthenticationsServices;
using VotepucApp.Application.AuthenticationsServices.Interfaces;
using VotepucApp.Application.BusinessService;
using VotepucApp.Application.BusinessService.ElectionService;
using VotepucApp.Application.BusinessService.UserService;

namespace VotepucApp.Application.ServiceExtensions;

public static class ServiceExtensions
{
    public static void ConfigureApplicationApp(this IServiceCollection services)
    {
        services.AddMediatR(configuration => 
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IElectionService, ElectionService>();
        services.AddTransient<ClaimsService>();
    }
}