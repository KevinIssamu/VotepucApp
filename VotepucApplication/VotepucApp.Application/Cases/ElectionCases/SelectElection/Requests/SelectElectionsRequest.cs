using MediatR;
using VotepucApp.Application.Cases.ElectionCases.SelectElection.Responses;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;

namespace VotepucApp.Application.Cases.ElectionCases.SelectElection.Requests;

public record SelectElectionsRequest(int Top = 50, int Skip = 0) : IRequest<SelectedElectionsResponse>;