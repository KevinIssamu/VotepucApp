using System.Reflection;
using Domain.UserAggregate.User;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VotepucApp.Application.BusinessService;

namespace VotepucApp.Application.ServiceExtensions;

public static class ServiceExtensions
{
    public static void ConfigureApplicationApp(this IServiceCollection services)
    {
        services.AddMediatR(configuration => 
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddTransient<IUserService, UserService>();
    }
}