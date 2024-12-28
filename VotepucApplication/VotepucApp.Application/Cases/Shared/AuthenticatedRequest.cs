using System.Security.Claims;
using MediatR;

namespace VotepucApp.Application.Cases.Shared;

public record AuthenticatedRequest<T, TR>(ClaimsPrincipal User, T Payload) : IRequest<TR>;