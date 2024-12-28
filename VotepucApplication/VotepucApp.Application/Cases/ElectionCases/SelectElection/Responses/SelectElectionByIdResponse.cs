using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.ElectionCases.SelectElection.Responses;

public record SelectElectionByIdResponse(int StatusCode, string Message, ElectionViewModel Election) : GenericResponse(StatusCode, Message);