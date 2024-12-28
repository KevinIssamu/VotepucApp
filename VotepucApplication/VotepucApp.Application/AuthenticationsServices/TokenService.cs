using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Shared.AppError;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using VotepucApp.Application.AuthenticationsServices.Interfaces;

namespace VotepucApp.Application.AuthenticationsServices;

public class TokenService : ITokenService
{
    public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IJwtSettings config)
    {
        var privateKey = Encoding.UTF8.GetBytes(config.GetSecretKey());

        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(privateKey),
                SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(config.GetTokenValidityInMinutes())),
            Audience = config.GetValidAudience(),
            Issuer = config.GetValidIssuer(),
            SigningCredentials = signingCredentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return token;
    }
    
    public OneOf<JwtSecurityToken, AppError> GenerateVoteRefreshToken(List<Claim> claims, IJwtSettings config, DateTimeOffset expirationDate)
    {
        if (expirationDate <= DateTime.Now)
            return new AppError("expiration date must be in the future.", AppErrorTypeEnum.BusinessRuleValidationFailure);
        
        var privateKey = Encoding.UTF8.GetBytes(config.GetSecretKey());

        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(privateKey),
                SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expirationDate.DateTime,
            Audience = config.GetValidAudience(),
            Issuer = config.GetValidIssuer(),
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var refreshToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return refreshToken;
    }
    
    public string GenerateUserRefreshToken()
    {
        var secureRandomBytes = new byte[128];

        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(secureRandomBytes);

        var refreshToken = Convert.ToBase64String(secureRandomBytes);
        return refreshToken;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IJwtSettings config)
    {
        var secretKey = config.GetSecretKey();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}