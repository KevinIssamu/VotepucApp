using MediatR;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;

namespace VotepucApp.Application.Cases.UseCases.SelectUser.Requests;

public record SelectUserByEmailRequest(string Email) : IRequest<UserResponse>;