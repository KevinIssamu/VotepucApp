using Domain.Shared.AppError;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.SelectUser.Responses;

public record SelectedUserElectionResponse(List<ElectionViewModel>? Elections, AppError? ErrorMessage);