using Domain.Shared.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using VotepucApp.Application.BusinessService.ElectionService;
using VotepucApp.Application.Cases.ElectionCases.Shared;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Services.Interfaces;

namespace VotepucApp.Application.Cases.ElectionCases.DeleteElection;

public class DeleteElectionHandler(
    IElectionService electionService,
    IUnitOfWork unitOfWork,
    IConfiguration configuration) : BaseHandler(configuration),
    IRequestHandler<ElectionIdRequest<GenericResponse>, GenericResponse>
{
    public async Task<GenericResponse> Handle(ElectionIdRequest<GenericResponse> request,
        CancellationToken cancellationToken)
    {
        var election = await electionService.SelectByIdAsNoTrackingAsync(request.Id, cancellationToken);

        if (election.IsT1)
            return CreateAppErrorResponse(election.AsT1);

        var deleteElection = electionService.Delete(election.AsT0, cancellationToken);

        if (deleteElection.IsT1)
            return CreateAppErrorResponse(deleteElection.AsT1);

        await unitOfWork.CommitAsync(cancellationToken);

        return new GenericResponse(204, deleteElection.AsT0.Message);
    }
}