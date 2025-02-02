using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.VoteLink;

namespace VotepucApp.Services.RabbitMQ.Events;

public sealed record SendEmailsEvent(Guid TaskId, List<VoteLink> VoteLinks);