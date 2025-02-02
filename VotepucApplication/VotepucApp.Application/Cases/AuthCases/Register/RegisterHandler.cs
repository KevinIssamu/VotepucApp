using Domain.UserAggregate.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.AuthCases.Register;

public class RegisterHandler(UserManager<User> userManager) : IRequestHandler<RegisterRequest, GenericResponse>
{
    public async Task<GenericResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        var userExists = await userManager.FindByEmailAsync(request.Email);

        if (userExists != null)
            return new GenericResponse(409, "User already exists.");

        if (request.Password != request.ConfirmPassword)
            return new GenericResponse(400, "Passwords do not match.");

        var user = User.Factory.Create(request.Name, request.Email, null);
        if (user.IsT1)
            return new GenericResponse(400, user.AsT1.Message);

        var result = await userManager.CreateAsync(user.AsT0, request.Password);

        return !result.Succeeded
            ? new GenericResponse(500, "User Creation failed.")
            : new GenericResponse(201, "User created successfully.");
    }
}