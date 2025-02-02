using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.AuthCases.Login;

public record LoginResponse(string Token, string RefreshToken, DateTime Expiration, int StatusCode, string Message)
    : GenericResponse(StatusCode, Message);