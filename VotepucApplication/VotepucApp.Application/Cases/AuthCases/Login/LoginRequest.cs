using MediatR;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.AuthCases.Login;

public sealed record LoginRequest(string Email, string Password) : IRequest<GenericResponse>;