using Domain.ElectionAggregate.VoteLink;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using OneOf;

namespace VotepucApp.Services.Email;

public interface IEmailService
{
    Task<OneOf<AppSuccess, AppError>> SendEmailAsync(List<VoteLink> links, string inviteText, string electionTitle);
}