using System.IdentityModel.Tokens.Jwt;
using Domain.UserAggregate.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using VotepucApp.Services.Interfaces.ConfigInterfaces;

namespace VotepucApp.Application.Cases.AuthCases.RefreshToken;

public class RefreshTokenHandler(ITokenService tokenService, IJwtSettings jwtSettings, UserManager<User> userManager)
    : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
{
    public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var principal = tokenService.GetPrincipalFromToken(request.AccessToken, jwtSettings);

        var username = principal.Identity!.Name;
        var user = await userManager.FindByNameAsync(username!);

        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return new RefreshTokenResponse(null, null, 400, "Invalid access token/refresh token.");

        var newAccessToken = tokenService.GenerateAccessToken(principal.Claims.ToList(), jwtSettings);

        var newRefreshToken = tokenService.GenerateUserRefreshToken();
        user.RefreshToken = newRefreshToken;

        return new RefreshTokenResponse(new JwtSecurityTokenHandler().WriteToken(newAccessToken), newRefreshToken, 200,
            "Token updated successfully.");
    }
}