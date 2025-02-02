using MediatR;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.UpdateUser;

public record UpdateUserRequest(Guid Id, UserRequestViewModel UserRequestUpdated) : IRequest<UpdateUserResponse>;