using Domain.Shared.AppError;
using Microsoft.Extensions.Configuration;

namespace VotepucApp.Application.Cases.Shared;

public abstract class BaseHandler(IConfiguration config)
{
    protected static GenericResponse CreateAppErrorResponse(AppError error, List<Link>? links = null) =>
        new(error.Type.ToHttpStatusCode(), error.Message, links);

    protected static GenericResponse CreateSuccessResponse(string message, List<Link>? links = null) =>
        new(200, message, links);

    protected Link CreateCrudLink(string method, string entity, string? id)
    {
        try
        {
            return new Link($"{method.ToUpper()}-{entity}", $"{entity}/{id}", $"{method.ToUpper()}", config);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}