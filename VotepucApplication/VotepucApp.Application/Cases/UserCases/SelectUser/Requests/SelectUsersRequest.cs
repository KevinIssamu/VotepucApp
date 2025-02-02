using MediatR;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;

namespace VotepucApp.Application.Cases.UseCases.SelectUser.Requests;

public record SelectUsersRequest(int Top = 50, int Skip = 0) : IRequest<SelectedUsersResponse>;