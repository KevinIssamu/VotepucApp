using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VotepucApp.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected async Task<ActionResult<TResponse>> HandleRequest<TRequest, TResponse>(
        TRequest request,
        IMediator mediator,
        IValidator<TRequest>? validator = null,
        CancellationToken cancellationToken = default)
        where TRequest : class
    {
        if (validator != null)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
        }

        var response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}