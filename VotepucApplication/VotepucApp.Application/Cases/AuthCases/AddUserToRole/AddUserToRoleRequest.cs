using MediatR;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.AuthCases.AddUserToRole;

public sealed record AddUserToRoleRequest(string Email, string RoleName) : IRequest<GenericResponse>;