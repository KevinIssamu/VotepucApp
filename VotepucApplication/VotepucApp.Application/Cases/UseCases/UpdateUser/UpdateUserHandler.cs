using Domain.Shared.AppError;
using MediatR;
using OneOf.Types;
using VotepucApp.Application.BusinessService.UserService;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.UseCases.UpdateUser;

public class UpdateUserHandler(IUserService userService) : IRequestHandler<UpdateUserRequest, GenericResponse>
{
    public async Task<GenericResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var userExistsResult = await userService.SelectByIdAsync(request.Id, cancellationToken);

        if (userExistsResult.IsT1)
            return new GenericResponse(userExistsResult.AsT1.Type.ToHttpStatusCode(), userExistsResult.AsT1.Message);
        
        var updateResult = await userService.UpdateAsync(userExistsResult.AsT0, request, cancellationToken);
        
        return updateResult.IsT1
            ? new UpdateUserResponse(request.Id, updateResult.AsT1.Type.ToHttpStatusCode(), updateResult.AsT1.Message)
            : new UpdateUserResponse(request.Id, 200, updateResult.AsT0.Message);
    }
}