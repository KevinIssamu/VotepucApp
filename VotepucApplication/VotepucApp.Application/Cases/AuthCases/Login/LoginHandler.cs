using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.UserAggregate.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using VotepucApp.Application.AuthenticationsServices.Interfaces;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.AuthCases.Login;

public class LoginHandler(
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    ITokenService tokenService,
    IJwtSettings jwtSettings) : IRequestHandler<LoginRequest, GenericResponse>
{
    public async Task<GenericResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            return new GenericResponse(401, "Incorrect email or password.");

        var userRoles = await userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var userRoleExists = await roleManager.RoleExistsAsync("CommonUser");
        if (!userRoleExists)
            await roleManager.CreateAsync(new IdentityRole("CommonUser"));

        await userManager.AddToRoleAsync(user, "CommonUser");

        var token = tokenService.GenerateAccessToken(authClaims, jwtSettings);

        var refreshToken = tokenService.GenerateUserRefreshToken();

        _ = int.TryParse(jwtSettings.GetRefreshTokenValidityInMinutes(), out var refreshTokenValidityInMinutes);

        user.RefreshToken = refreshToken;

        user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

        await userManager.UpdateAsync(user);

        return new LoginResponse(new JwtSecurityTokenHandler().WriteToken(token), refreshToken, token.ValidTo, 200,
            "User successfully logged in.");
    }
}