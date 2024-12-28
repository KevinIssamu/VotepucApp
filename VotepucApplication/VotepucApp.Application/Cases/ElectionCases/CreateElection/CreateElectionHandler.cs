using System.Security.Claims;
using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Participant;
using Domain.ElectionAggregate.Participant.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.Interfaces;
using Domain.UserAggregate.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using VotepucApp.Application.AuthenticationsServices;
using VotepucApp.Application.BusinessService.ElectionService;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.ElectionCases.CreateElection;

public class CreateElectionHandler(
    IElectionService electionService,
    UserManager<User> userManager,
    ClaimsService claimsService,
    IElectionRepository electionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AuthenticatedRequest<CreateElectionRequest, GenericResponse>, GenericResponse>
{
    public async Task<GenericResponse> Handle(AuthenticatedRequest<CreateElectionRequest, GenericResponse> request,
        CancellationToken cancellationToken)
    {
        var user = await claimsService.GetUserByClaims(request.User);

        if (user.IsT1)
            return new GenericResponse(401, user.AsT1.Message);

        var election = PendingElection.Factory.Create(request.Payload.Title, request.Payload.Description,
            request.Payload.InvitationMessage, request.Payload.AllowMultipleVotes, request.Payload.StartDate,
            request.Payload.EndDate, null, user.AsT0);

        if (election.IsT1)
            return new GenericResponse(400, election.AsT1.Message);

        var (candidateSuccesses, candidateErrors) = request.Payload.Participantes
            .Where(p => p.TypeOfParticipant == TypeOfParticipantEnum.Candidate)
            .Select(c => Candidate.Factory.Create(c.Email, c.Name, election.AsT0))
            .Aggregate(
                (Valid: new List<Candidate>(), Errors: new List<AppError>()),
                (acc, next) =>
                {
                    if (next.IsT0) acc.Valid.Add(next.AsT0);
                    if (next.IsT1) acc.Errors.Add(next.AsT1);
                    return acc;
                });

        var (voterSuccesses, voterErrors) = request.Payload.Participantes
            .Where(p => p.TypeOfParticipant == TypeOfParticipantEnum.Voter)
            .Select(p => Voter.Factory.Create(p.Email, p.Name, election.AsT0))
            .Aggregate(
                (Valid: new List<Voter>(), Errors: new List<AppError>()),
                (acc, next) =>
                {
                    if (next.IsT0) acc.Valid.Add(next.AsT0);
                    if (next.IsT1) acc.Errors.Add(next.AsT1);
                    return acc;
                });

        if (candidateErrors.Count > 0 || voterErrors.Count > 0)
        {
            var allErrors = new List<AppError>();

            allErrors.AddRange(candidateErrors);
            allErrors.AddRange(voterErrors);

            return new GenericResponse(400, allErrors.ToString());
        }

        var participants = new List<Participant>();
        participants.AddRange(candidateSuccesses);
        participants.AddRange(voterSuccesses);

        election.AsT0.SetParticipants(participants);

        await electionRepository.CreateAsync(election.AsT0, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        
        return new GenericResponse(201, "Election created successfully.");
    }
}