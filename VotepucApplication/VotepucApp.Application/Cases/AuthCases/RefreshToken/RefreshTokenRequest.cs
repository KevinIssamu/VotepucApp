using MediatR;

namespace VotepucApp.Application.Cases.AuthCases.RefreshToken;

public sealed record RefreshTokenRequest(string AccessToken, string RefreshToken) : IRequest<RefreshTokenResponse>;