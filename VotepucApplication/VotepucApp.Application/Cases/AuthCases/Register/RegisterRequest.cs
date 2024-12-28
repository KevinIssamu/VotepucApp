using MediatR;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.AuthCases.Register;

public sealed record RegisterRequest(string Name, string Email, string Password, string ConfirmPassword) : IRequest<GenericResponse>;