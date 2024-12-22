using MediatR;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;

namespace VotepucApp.Application.Cases.UseCases.Shared.Requests;

public record SelectUserByIdRequest<T>(Guid Id) : IRequest<T>;