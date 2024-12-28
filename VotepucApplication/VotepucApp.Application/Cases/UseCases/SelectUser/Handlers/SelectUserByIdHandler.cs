using Domain.Shared.AppError;
using Domain.UserAggregate.User;
using MediatR;
using VotepucApp.Application.BusinessService;
using VotepucApp.Application.BusinessService.UserService;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.UseCases.SelectUser.Handlers;

public class SelectUserByIdHandler(IUserService userService)
    : IRequestHandler<SelectUserByIdRequest<GenericResponse>, GenericResponse>
{
    public async Task<GenericResponse> Handle(SelectUserByIdRequest<GenericResponse> request,
        CancellationToken cancellationToken)
    {
        var selectUserByIdResult = await userService.SelectByIdAsync(request.Id, cancellationToken);

        if (selectUserByIdResult.IsT1)
            return new GenericResponse(selectUserByIdResult.AsT1.Type.ToHttpStatusCode(), selectUserByIdResult.AsT1.Message);

        if (selectUserByIdResult.AsT0 == null)
            return new GenericResponse(404, "User Not Found");

        var user = selectUserByIdResult.AsT0;

        return new UserResponse(new UserResponseViewModel(Guid.Parse(user.Id), user.Email!, user.UserName!), 200,
            "User Found.");
    }
}