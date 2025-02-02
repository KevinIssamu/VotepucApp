using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.Cases.TaskCases.SelectTask;
using VotepucApp.Persistence.Context.Seeder.Permissions;

namespace VotepucApp.WebAPI.Controllers;

public class TaskController(IMediator mediator) : BaseController
{
    [Authorize(Policy = TaskPermissions.TaskGet)]
    [HttpGet("/tasks/status/{taskId:guid}")]
    public async Task<ActionResult<GenericResponse>> Get(Guid taskId)
    {
        var request = new SelectTaskByIdRequest(taskId);
        return await HandleRequest<SelectTaskByIdRequest, GenericResponse>(request, mediator);
    }
}