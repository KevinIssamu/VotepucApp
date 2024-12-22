using MediatR;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;

namespace VotepucApp.Application.Cases.UseCases.CreateUser;

public record CreateUserRequest(string Name, string Email, string Password) : IRequest<UserResponse>;