using MediatR;
using VotepucApp.Application.Cases.UseCases.SelectUser.Responses;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;

namespace VotepucApp.Application.Cases.UseCases.SelectUser.Requests;

public record SelectUserElectionsRequest(Guid UserId, int Take, int Skip) : IRequest<SelectedUserElectionResponse>;