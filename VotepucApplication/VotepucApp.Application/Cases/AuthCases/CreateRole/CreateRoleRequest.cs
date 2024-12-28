using MediatR;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.AuthCases.CreateRole;

public record CreateRoleRequest(string RoleName) : IRequest<GenericResponse>;