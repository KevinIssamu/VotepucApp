using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using OneOf;

namespace VotepucApp.Services.Interfaces.ConfigInterfaces;

public interface ITokenService
{
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IJwtSettings config);

    string GenerateUserRefreshToken();

    ClaimsPrincipal GetPrincipalFromToken(string token, IJwtSettings config);

    OneOf<string, AppError> GenerateVoteRefreshToken(List<Claim> claims, IJwtSettings config,
        DateTimeOffset expirationDate);

    OneOf<AppSuccess, AppError> ValidateRefreshToken(string refreshToken, IJwtSettings config);
}