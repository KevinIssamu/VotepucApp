namespace VotepucApp.Application.AuthenticationsServices.Interfaces;

public interface IJwtSettings
{
    string GetTokenValidityInMinutes();
    string GetRefreshTokenValidityInMinutes();
    string GetSecretKey();
    string GetValidIssuer();
    string GetValidAudience();
}