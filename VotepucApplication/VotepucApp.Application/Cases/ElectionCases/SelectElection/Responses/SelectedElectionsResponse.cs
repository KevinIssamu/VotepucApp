using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.ElectionCases.SelectElection.Responses;

public record SelectedElectionsResponse(List<ElectionViewModel>? Elections, int StatusCode, string Message)
    : GenericResponse(StatusCode, Message);