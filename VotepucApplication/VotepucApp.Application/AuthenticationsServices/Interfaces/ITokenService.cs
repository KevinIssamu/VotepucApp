using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Shared.AppError;
using Microsoft.Extensions.Configuration;
using OneOf;

namespace VotepucApp.Application.AuthenticationsServices.Interfaces;

public interface ITokenService
{
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IJwtSettings config);

    string GenerateUserRefreshToken();

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IJwtSettings config);

    OneOf<JwtSecurityToken, AppError> GenerateVoteRefreshToken(List<Claim> claims, IJwtSettings config,
        DateTimeOffset expirationDate);
}