using MediatR;
using VotepucApp.Application.BusinessService;

namespace VotepucApp.Application.Cases.UseCases.UpdateUser;

public class UpdateUserHandler(IUserService userService) : IRequestHandler<UpdateUserRequest, UpdateUserResponse>
{
    public async Task<UpdateUserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var updateResult = await userService.UpdateAsync(request, cancellationToken);

        return updateResult.IsT1
            ? new UpdateUserResponse(request.Id, null, updateResult.AsT1)
            : new UpdateUserResponse(request.Id, updateResult.AsT0, null);
    }
}