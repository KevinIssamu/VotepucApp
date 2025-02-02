using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Election.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using VotepucApp.Application.BusinessService.ElectionService;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Services.Interfaces;

namespace VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateElectionStatus;

public class UpdateStatusHandler(IElectionService electionService, IUnitOfWork unitOfWork, IConfiguration configuration)
    : BaseHandler(configuration), IRequestHandler<UpdateStatusRequest, GenericResponse>
{
    public async Task<GenericResponse> Handle(UpdateStatusRequest request, CancellationToken cancellationToken)
    {
        var electionResult = await electionService.SelectByIdTrackingAsync(request.ElectionId, cancellationToken);
        
        if (electionResult.IsT1)
            return CreateAppErrorResponse(electionResult.AsT1,
                [CreateCrudLink("get", "election", request.ElectionId.ToString())]);

        if (electionResult.AsT0.ElectionStatus != ElectionStatusEnum.Pending)
            return new GenericResponse(400,"It is not possible to update the status of an election with a status other than pending.", null);

        var election = electionResult.AsT0;
        
        return request.Status switch
        {
            ElectionStatusEnum.Approved => await ApproveElectionAsync(election, cancellationToken),
            ElectionStatusEnum.Rejected => await RejectElectionAsync(election, cancellationToken),
            _ => new GenericResponse(400, "Invalid election status. Only 'Approved' and 'Rejected' statuses are allowed.", null)
        };
    }

    private async Task<GenericResponse> ApproveElectionAsync(Election election, CancellationToken cancellationToken)
    {
        var approveResult = electionService.Approve(election);

        if (approveResult.IsT1)
            return CreateAppErrorResponse(approveResult.AsT1, null);

        return await FinalizeElectionUpdateAsync(election, cancellationToken, "Election approved successfully.");
    }

    private async Task<GenericResponse> RejectElectionAsync(Election election, CancellationToken cancellationToken)
    {
        var rejectResult = electionService.Reject(election);

        if (rejectResult.IsT1)
            return CreateAppErrorResponse(rejectResult.AsT1, null);

        return await FinalizeElectionUpdateAsync(election, cancellationToken, rejectResult.AsT0.Message);
    }

    private async Task<GenericResponse> FinalizeElectionUpdateAsync(Election election, CancellationToken cancellationToken, string successMessage)
    {
        var updateResult = electionService.Update(election, cancellationToken);

        if (updateResult.IsT1)
            return CreateAppErrorResponse(updateResult.AsT1, null);

        await unitOfWork.CommitAsync(cancellationToken);
        return new GenericResponse(200, successMessage, 
            [
                CreateCrudLink("patch", "election", null)
            ]);
    }
}
