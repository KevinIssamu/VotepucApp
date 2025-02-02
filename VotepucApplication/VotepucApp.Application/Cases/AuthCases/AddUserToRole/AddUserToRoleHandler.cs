using Domain.UserAggregate.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using VotepucApp.Application.BusinessService.UserService;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Persistence.Interfaces;
using VotepucApp.Services.AuthenticationsServices;

namespace VotepucApp.Application.Cases.AuthCases.AddUserToRole;

public class AddUserToRoleHandler(IUserService userService, IClaimsService claimsService, IConfiguration configuration)
    : BaseHandler(configuration), IRequestHandler<AddUserToRoleRequest, GenericResponse>
{
    public async Task<GenericResponse> Handle(AddUserToRoleRequest request, CancellationToken cancellationToken)
    {
        var user = await userService.SelectUserByEmailAsync(request.Email);

        if (user.IsT1)
            return CreateAppErrorResponse(user.AsT1);

        var result = await claimsService.AddUserToRoleAsync(user.AsT0, request.RoleName);

        return result.IsT1
            ? CreateAppErrorResponse(result.AsT1)
            : CreateSuccessResponse(result.AsT0.Message);
    }
}