using Domain.Shared.AppError;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.SelectUser.Responses;

public record SelectedUserElectionResponse(List<ElectionViewModel>? Elections, int StatusCode, string Message)
    : GenericResponse(StatusCode, Message);