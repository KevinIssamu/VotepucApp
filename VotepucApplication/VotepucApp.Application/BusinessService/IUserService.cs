using Domain.ElectionAggregate.Election;
using Domain.Shared.AppError;
using Domain.UserAggregate.User;
using MediatR;
using OneOf;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;

namespace VotepucApp.Application.BusinessService;

public interface IUserService : IService<User>
{
    Task<OneOf<List<Election>, AppError>> SelectUserElectionsAsync(SelectUserElectionsRequest request, CancellationToken cancellationToken);
}