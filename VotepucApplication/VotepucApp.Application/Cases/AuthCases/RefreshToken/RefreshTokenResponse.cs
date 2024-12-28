using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.AuthCases.RefreshToken;

public record RefreshTokenResponse(string? AccessToken, string? RefreshToken, int StatusCode, string? Message)
    : GenericResponse(StatusCode, Message);