using Domain.UserAggregate.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VotepucApp.Application.Cases.AuthCases;
using VotepucApp.Application.Cases.AuthCases.AddUserToRole;
using VotepucApp.Application.Cases.AuthCases.CreateRole;
using VotepucApp.Application.Cases.AuthCases.Login;
using VotepucApp.Application.Cases.AuthCases.RefreshToken;
using VotepucApp.Application.Cases.AuthCases.Register;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
    UserManager<User> userManager,
    IMediator mediator) : ControllerBase
{
    [Authorize(Policy = "SuperAdm")]
    [HttpPost]
    [Route("CreateRole")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        var newRoleValidator = new CreateRoleValidator();

        var validationResult = await newRoleValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var responseBadRequest = new GenericResponse(400, validationResult.Errors.ToString());
            return StatusCode(responseBadRequest.StatusCode, responseBadRequest.Message);
        }

        var response = await mediator.Send(request);

        return StatusCode(response.StatusCode, response.Message);
    }
    
    [Authorize(Policy = "SuperAdm")]
    [HttpPost]
    [Route("AddUserToRole")]
    public async Task<IActionResult> AddUserToRole([FromBody] AddUserToRoleRequest request)
    {
        var validator = new AddUserToRoleValidator();

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var responseBadRequest = new GenericResponse(400, validationResult.Errors.ToString());
            return StatusCode(responseBadRequest.StatusCode, responseBadRequest.Message);
        }

        var response = await mediator.Send(request);

        return StatusCode(response.StatusCode, response.Message);
    }
    
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var validator = new LoginValidator();
        var validationResult = await validator.ValidateAsync(loginRequest);

        if (!validationResult.IsValid)
        {
            var responseBadRequest =
                new LoginResponse(null, null, null, 400, validationResult.Errors.ToString()!);
            return StatusCode(responseBadRequest.StatusCode, responseBadRequest.Message);
        }

        var response = await mediator.Send(loginRequest);

        if (response.StatusCode is >= 200 and < 300)
        {
            return Ok(new
            {
                response.Token,
                response.RefreshToken,
                response.Expiration,
                response.Message
            });
        }
        
        return StatusCode(response.StatusCode, response.Message);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var validator = new RegisterValidator();

        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var responseBadRequest = new GenericResponse(400, validationResult.Errors.ToString());
            return StatusCode(responseBadRequest.StatusCode, responseBadRequest.Message);
        }

        var response = await mediator.Send(request);

        return StatusCode(response.StatusCode, response.Message);
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var validator = new RefreshTokenValidator();

        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var responseBadRequest = new GenericResponse(400, validationResult.Errors.ToString());
            return StatusCode(responseBadRequest.StatusCode, responseBadRequest.Message);
        }

        var response = await mediator.Send(request);
        
        if (response.StatusCode is >= 200 and < 300)
        {
            return Ok(new
            {
                response.AccessToken,
                response.RefreshToken,
                response.Message
            });
        }
        
        return StatusCode(response.StatusCode, response.Message);
    }

    [Authorize(Policy = "Adm")]
    [HttpPost]
    [Route("revoke/{id:guid}")]
    public async Task<IActionResult> Revoke(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user is null) return BadRequest("User not found or invalid.");

        user.RefreshToken = null;

        await userManager.UpdateAsync(user);

        return NoContent();
    }
}