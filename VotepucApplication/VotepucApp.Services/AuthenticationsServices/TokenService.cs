using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using VotepucApp.Services.Interfaces.ConfigInterfaces;

namespace VotepucApp.Services.AuthenticationsServices;

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
    
    public OneOf<string, AppError> GenerateVoteRefreshToken(List<Claim> claims, IJwtSettings config, DateTimeOffset expirationDate)
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
        return tokenHandler.WriteToken(refreshToken);
    }
    
    public string GenerateUserRefreshToken()
    {
        var secureRandomBytes = new byte[128];

        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(secureRandomBytes);

        var refreshToken = Convert.ToBase64String(secureRandomBytes);
        return refreshToken;
    }

    public ClaimsPrincipal GetPrincipalFromToken(string token, IJwtSettings config)
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

    public OneOf<AppSuccess, AppError> ValidateRefreshToken(string? refreshToken, IJwtSettings config)
    {
        if (refreshToken == null)
            return new AppError("Token is not valid", AppErrorTypeEnum.ValidationFailure);
                
        var tokenHandler = new JwtSecurityTokenHandler();
        
        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config.GetValidIssuer(),
                ValidAudience = config.GetValidAudience(),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSecretKey()))
            };
            
            var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out var validatedToken);
            
            if (validatedToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return new AppError("Invalid token signature.", AppErrorTypeEnum.ValidationFailure);
            }
            
            var wasUtilizedClaim = principal.Claims.FirstOrDefault(c => c.Type == "WasUtilized");
            if (wasUtilizedClaim != null && bool.TryParse(wasUtilizedClaim.Value, out var wasUtilized) && wasUtilized)
            {
                return new AppError("Token has already been utilized.", AppErrorTypeEnum.ValidationFailure);
            }

            return new AppSuccess("Token is valid.");
        }
        catch (SecurityTokenExpiredException)
        {
            return new AppError("Token has expired.", AppErrorTypeEnum.ValidationFailure);
        }
        catch (Exception ex)
        {
            return new AppError($"Token validation failed: {ex.Message}", AppErrorTypeEnum.ValidationFailure);
        }
    }
}