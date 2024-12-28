using VotepucApp.Application.AuthenticationsServices.Interfaces;

namespace VotepucApp.WebAPI.Settings;

public class JwtSettings(IConfiguration configuration) : IJwtSettings
{
    public string GetTokenValidityInMinutes() => 
        configuration["JWT:TokenValidityInMinutes"] ?? throw new ArgumentException("TokenValidityInMinutes is required.");
    public string GetRefreshTokenValidityInMinutes() => 
        configuration["JWT:RefreshTokenValidityInMinutes"] ?? throw new ArgumentException("RefreshTokenValidityInMinutes is required.");
    public string GetSecretKey() =>
        configuration["JWT:SecretKey"] ?? throw new ArgumentException("SecretKey is required.");
    public string GetValidIssuer() =>
        configuration["JWT:ValidIssuer"] ?? throw new ArgumentException("ValidIssuer is required.");
    public string GetValidAudience() =>
        configuration["JWT:ValidAudience"] ?? throw new ArgumentException("ValidAudience is required.");
}