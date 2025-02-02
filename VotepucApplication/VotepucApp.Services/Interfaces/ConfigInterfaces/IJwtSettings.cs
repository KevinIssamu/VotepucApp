namespace VotepucApp.Services.Interfaces.ConfigInterfaces;

public interface IJwtSettings
{
    string GetTokenValidityInMinutes();
    string GetRefreshTokenValidityInMinutes();
    string GetSecretKey();
    string GetValidIssuer();
    string GetValidAudience();
}