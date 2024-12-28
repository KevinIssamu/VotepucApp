using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Election.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.Interfaces;
using MediatR;
using VotepucApp.Application.BusinessService.ElectionService;
using VotepucApp.Application.Cases.ElectionCases.Shared;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateElectionStatus;

public class UpdateStatusHandler(IElectionService electionService, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateStatusRequest, GenericResponse>
{
    public async Task<GenericResponse> Handle(UpdateStatusRequest request, CancellationToken cancellationToken)
    {
        var getElectionByIdResult = await electionService.SelectByIdAsync(request.ElectionId, cancellationToken);

        if (getElectionByIdResult.IsT1)
        {
            return getElectionByIdResult.AsT1.Type == AppErrorTypeEnum.SystemError
                ? new GenericResponse(500, getElectionByIdResult.AsT1.Message)
                : new GenericResponse(404, getElectionByIdResult.AsT1.Message);
        }

        var election = getElectionByIdResult.AsT0;

        if (election.Status != ElectionStatusEnum.Pending)
            return new GenericResponse(400, $"Only pending elections can have their status changed.");

        var pendingElection = (PendingElection)getElectionByIdResult.AsT0;
        if (request.Status == ElectionStatusEnum.Approved)
        {
            var approveElectionResult = pendingElection.Approve();
            
            return approveElectionResult.IsT1
                ? new GenericResponse(400, approveElectionResult.AsT1.Message)
                : new GenericResponse(200, approveElectionResult.AsT0.Message);
        }

        var rejectElectionResult = pendingElection.Reject();

        if (rejectElectionResult.IsT1)
            return new GenericResponse(400, rejectElectionResult.AsT1.Message);

        await unitOfWork.CommitAsync(cancellationToken);
        return new GenericResponse(200, rejectElectionResult.AsT0.Message);
    }
}