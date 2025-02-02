using MediatR;
using Microsoft.Extensions.Configuration;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Services.Cache;

namespace VotepucApp.Application.Cases.TaskCases.SelectTask;

public class SelectTaskByIdHandler(ICacheService cacheService, IConfiguration configuration)
    : BaseHandler(configuration), IRequestHandler<SelectTaskByIdRequest, GenericResponse>
{
    private readonly IConfiguration _configuration = configuration;

    public async Task<GenericResponse> Handle(SelectTaskByIdRequest request, CancellationToken cancellationToken)
    {
        var task = await cacheService.GetAsync(request.TaskId.ToString());

        if (string.IsNullOrEmpty(task))
            return new GenericResponse(404, "Task not found");

        var listLinks = new List<Link>
        {
            new("self", $"tasks/status/{request.TaskId}", "GET", _configuration)
        };
        
        return new GenericResponse(200, task, listLinks);
    }
}