using Domain.Shared.AppError;
using Domain.Shared.Interfaces;
using MediatR;
using VotepucApp.Application.BusinessService.UserService;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.Cases.UseCases.Shared.Requests;

namespace VotepucApp.Application.Cases.UseCases.DeleteUser;

public class DeleteUserHandler(IUserService userService, IUnitOfWork unitOfWork)
    : IRequestHandler<SelectUserByIdRequest<GenericResponse>, GenericResponse>
{
    public async Task<GenericResponse> Handle(SelectUserByIdRequest<GenericResponse> request,
        CancellationToken cancellationToken)
    {
        var userExistsResult = await userService.SelectByIdAsNoTrackingAsync(request.Id, cancellationToken);
        if(userExistsResult.IsT1)
            return new GenericResponse(userExistsResult.AsT1.Type.ToHttpStatusCode(), userExistsResult.AsT1.Message);
        
        var deleteUserResult = userService.Delete(userExistsResult.AsT0, cancellationToken);

        if (deleteUserResult.IsT1)
            return new GenericResponse(userExistsResult.AsT1.Type.ToHttpStatusCode(), userExistsResult.AsT1.Message);
        
        await unitOfWork.CommitAsync(cancellationToken);
        
        return new GenericResponse(204, deleteUserResult.AsT0.Message);
    }
}