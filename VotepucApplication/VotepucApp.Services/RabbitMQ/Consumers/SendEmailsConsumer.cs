using Domain.ElectionAggregate.Election;
using Domain.Shared.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using VotepucApp.Services.Cache;
using VotepucApp.Services.Email;
using VotepucApp.Services.RabbitMQ.Events;

namespace VotepucApp.Services.RabbitMQ.Consumers;

public class SendEmailsConsumer(
    ILogger<SendEmailsConsumer> logger, 
    IEmailService emailService, 
    ICacheService cacheService, 
    IUnitOfWork unitOfWork, 
    IElectionRepository electionRepository)
    : IConsumer<SendEmailsEvent>
{
    public async Task Consume(ConsumeContext<SendEmailsEvent> @event)
    {
        logger.LogInformation("Processing SendEmailsEvent.");

        var message = @event.Message;
        
        await cacheService.SetAsync(message.TaskId.ToString(), "Accepted");
        
        try
        {
            if (message.VoteLinks.Count == 0)
            {
                await cacheService.SetAsync(message.TaskId.ToString(), "Failed");
                return;
            }
            
            var election = await electionRepository.SelectByIdTrackingAsync(message.VoteLinks[0].ElectionId, CancellationToken.None);
            if (election.IsT1)
            {
                await cacheService.SetAsync(message.TaskId.ToString(), "Failed");
                return;
            }
            
            var electionBehavior = election.AsT0.GetBehavior<ApprovedElectionBehavior>();
            if (electionBehavior.IsT1)
            {
                await cacheService.SetAsync(message.TaskId.ToString(), "Failed");
                return;
            }
            
            logger.LogInformation("Sending emails to {VoteLinksCount} recipients.", message.VoteLinks.Count);
            
            var sendEmailsResult = await emailService.SendEmailAsync(
                message.VoteLinks, 
                election.AsT0.EmailInvitationText, 
                election.AsT0.Title);

            if (sendEmailsResult.IsT1)
            {
                logger.LogWarning("Failed to send emails for election '{ElectionTitle}'.", election.AsT0.Title);
                await cacheService.SetAsync(message.TaskId.ToString(), "Failed");
            }
            else
            {
                var startElection = electionBehavior.AsT0.Start();
                if (startElection.IsT1)
                {
                    await cacheService.SetAsync(message.TaskId.ToString(), "Failed");
                    return;
                }

                await unitOfWork.CommitAsync();
                logger.LogInformation("Emails successfully sent for election '{ElectionTitle}'.", election.AsT0.Title);
                await cacheService.SetAsync(message.TaskId.ToString(), "Completed");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing SendEmailsEvent.");
            await cacheService.SetAsync(message.TaskId.ToString(), "Failed");
        }
    }
}
