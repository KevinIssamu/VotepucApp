using MediatR;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.AuthCases.DeleteRole;

public record DeleteRoleRequest(string RoleName) : IRequest<GenericResponse>;