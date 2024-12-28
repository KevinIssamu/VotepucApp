using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Election.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.Interfaces;
using MediatR;
using VotepucApp.Application.BusinessService.ElectionService;
using VotepucApp.Application.BusinessService.VoteLinkService;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateElectionProgress;

public class UpdateProgressHandler(IElectionService electionService, VoteLinkService voteLinkService, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProgressRequest, GenericResponse>
{
    public async Task<GenericResponse> Handle(UpdateProgressRequest request, CancellationToken cancellationToken)
    {
        var electionExistsResult = await electionService.SelectByIdAsync(request.ElectionId, cancellationToken);

        if (electionExistsResult.IsT1)
        {
            return electionExistsResult.AsT1.Type == AppErrorTypeEnum.SystemError
                ? new GenericResponse(500, electionExistsResult.AsT1.Message)
                : new GenericResponse(404, electionExistsResult.AsT1.Message);
        }

        var election = electionExistsResult.AsT0;
        
        if (request.Progress == ElectionProgressEnum.Active)
        {
            var approveResult = electionService.Start(election);
            
            if (approveResult.IsT1)
                return new GenericResponse(400, approveResult.AsT1.Message);

            var createLinksResult = await voteLinkService.CreateLinks(election, cancellationToken);

            if (electionExistsResult.IsT1)
            {
                return electionExistsResult.AsT1.Type == AppErrorTypeEnum.BusinessRuleValidationFailure
                    ? new GenericResponse(400, electionExistsResult.AsT1.Message)
                    : new GenericResponse(500, electionExistsResult.AsT1.Message);
            }

            var electionApproved = (ElectionApproved)election;
            
            var updateElection = electionService.UpdateAsync()
            electionApproved.SetVoteLinks(createLinksResult.AsT0);
            await unitOfWork.CommitAsync(cancellationToken);
                    
            return new GenericResponse(200, approveResult.AsT0.Message);
        }

        var finishResult = approvedElection.Finish();
        return finishResult.IsT1
            ? new GenericResponse(400, finishResult.AsT1.Message)
            : new GenericResponse(200, finishResult.AsT0.Message);
    }
}