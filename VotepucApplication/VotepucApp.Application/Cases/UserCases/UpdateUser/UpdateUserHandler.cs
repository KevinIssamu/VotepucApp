using Domain.Shared.AppError;
using Domain.Shared.Interfaces;
using MediatR;
using OneOf.Types;
using VotepucApp.Application.BusinessService.UserService;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.UseCases.UpdateUser;

public class UpdateUserHandler(IUserService userService, IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserRequest, GenericResponse>
{
    public async Task<GenericResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var userExistsResult = await userService.SelectByIdAsNoTrackingAsync(request.Id, cancellationToken);

        if (userExistsResult.IsT1)
            return new GenericResponse(userExistsResult.AsT1.Type.ToHttpStatusCode(), userExistsResult.AsT1.Message);
        
        var updateResult = await userService.UpdateAsync(userExistsResult.AsT0, request, cancellationToken);
        
        if(updateResult.IsT1)
            return new GenericResponse(updateResult.AsT1.Type.ToHttpStatusCode(), updateResult.AsT1.Message);
        
        await unitOfWork.CommitAsync(cancellationToken);
        
        return new UpdateUserResponse(request.Id, 200, updateResult.AsT0.Message);
    }
}