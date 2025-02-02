using MediatR;
using Microsoft.Extensions.Configuration;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Persistence.Interfaces;
using VotepucApp.Services.AuthenticationsServices;

namespace VotepucApp.Application.Cases.AuthCases.DeleteRole;

public class DeleteRoleHandler(IConfiguration config, IClaimsService claimsService) : BaseHandler(config), IRequestHandler<DeleteRoleRequest, GenericResponse>
{
    public async Task<GenericResponse> Handle(DeleteRoleRequest request, CancellationToken cancellationToken)
    {
        var deleteRoleResult = await claimsService.DeleteRoleAsync(request.RoleName);

        if (deleteRoleResult.IsT1)
            return CreateAppErrorResponse(deleteRoleResult.AsT1);
        
        return CreateSuccessResponse("Role deleted");
    }
}