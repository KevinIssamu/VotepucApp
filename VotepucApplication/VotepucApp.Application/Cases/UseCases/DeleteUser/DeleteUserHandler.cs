using Domain.Shared.AppError;
using MediatR;
using VotepucApp.Application.BusinessService.UserService;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.Cases.UseCases.Shared.Requests;

namespace VotepucApp.Application.Cases.UseCases.DeleteUser;

public class DeleteUserHandler(IUserService userService)
    : IRequestHandler<SelectUserByIdRequest<GenericResponse>, GenericResponse>
{
    public async Task<GenericResponse> Handle(SelectUserByIdRequest<GenericResponse> request,
        CancellationToken cancellationToken)
    {
        var deleteUserResult = await userService.Delete(request.Id, cancellationToken);

        if (deleteUserResult.IsT1)
        {
            return deleteUserResult.AsT1.Type == AppErrorTypeEnum.SystemError
                ? new GenericResponse(500, deleteUserResult.AsT1.Message)
                : new GenericResponse(404, deleteUserResult.AsT1.Message);
        }
        
        return new GenericResponse(200, deleteUserResult.AsT0.Message);
    }
}