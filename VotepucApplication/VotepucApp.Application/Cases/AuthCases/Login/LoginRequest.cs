using MediatR;

namespace VotepucApp.Application.Cases.AuthCases.Login;

public sealed record LoginRequest(string Email, string Password) : IRequest<LoginResponse>;