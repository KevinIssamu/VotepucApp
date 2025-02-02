using System.Security.Claims;
using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Participant;
using Domain.ElectionAggregate.Participant.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.Interfaces;
using Domain.UserAggregate.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using VotepucApp.Application.BusinessService.ElectionService;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.ViewModels;
using VotepucApp.Persistence.Interfaces;
using VotepucApp.Services.AuthenticationsServices;
using VotepucApp.Services.Interfaces;

namespace VotepucApp.Application.Cases.ElectionCases.CreateElection;

public class CreateElectionHandler(
    IClaimsService claimsService,
    IElectionService electionService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AuthenticatedRequest<CreateElectionViewModel, GenericResponse>, GenericResponse>
{
    public async Task<GenericResponse> Handle(AuthenticatedRequest<CreateElectionViewModel, GenericResponse> request,
        CancellationToken cancellationToken)
    {
        var user = await claimsService.GetUserByClaims(request.User);

        if (user.IsT1)
            return new GenericResponse(user.AsT1.Type.ToHttpStatusCode(), user.AsT1.Message);

        var createElectionResult = await electionService.CreateAsync(request.Payload.Title, request.Payload.Description,
            request.Payload.InvitationMessage, request.Payload.AllowMultipleVotes, request.Payload.StartDate,
            request.Payload.EndDate, user.AsT0, request.Payload.Participants, cancellationToken);
        
        if(createElectionResult.IsT1)
            return new GenericResponse(createElectionResult.AsT1.Type.ToHttpStatusCode(), createElectionResult.AsT1.Message);
        
        await unitOfWork.CommitAsync(cancellationToken);
        
        return new GenericResponse(201, "Election created successfully.");
    }
}