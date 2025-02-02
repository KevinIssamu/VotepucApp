using MediatR;
using Microsoft.Extensions.Configuration;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Persistence.Interfaces;

namespace VotepucApp.Application.Cases.AuthCases.CreateRole;

public class CreateRoleHandler(IClaimsService claimsService, IConfiguration configuration)
    : BaseHandler(configuration), IRequestHandler<CreateRoleRequest, GenericResponse>
{
    public async Task<GenericResponse> Handle(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var roleResult = await claimsService.CreateRoleAsync(request.RoleName);

        return roleResult.IsT1 
            ? CreateAppErrorResponse(roleResult.AsT1) 
            : CreateSuccessResponse(roleResult.AsT0.Message);
    }
}