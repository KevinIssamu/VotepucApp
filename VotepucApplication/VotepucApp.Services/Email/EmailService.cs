using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using Domain.ElectionAggregate.VoteLink;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using OneOf;
using VotepucApp.Services.Interfaces.ConfigInterfaces;

namespace VotepucApp.Services.Email;

public class EmailService(ISmtpSettings smtpSettings, IJwtSettings jwtSettings, ITokenService tokenService)
    : IEmailService
{
    private async Task<OneOf<AppSuccess, AppError>> SendEmailToRecipient(SmtpClient smtpClient, string recipientEmail,
        string subject, string message)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings.GetSmtpSenderEmail(), smtpSettings.GetSmtpSenderName()),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(recipientEmail);

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
            return new AppSuccess("Email sent successfully");
        }
        catch (Exception ex)
        {
            return new AppError($"Error sending email: {ex.Message}", AppErrorTypeEnum.SystemError);
        }
    }

    public async Task<OneOf<AppSuccess, AppError>> SendEmailAsync(List<VoteLink> links, string inviteText,
        string electionTitle)
    {
        try
        {
            var smtpClient = new SmtpClient(smtpSettings.GetSmtpServer())
            {
                Port = int.Parse(smtpSettings.GetSmtpPort()),
                Credentials = new NetworkCredential(smtpSettings.GetSmtpSenderName(), smtpSettings.GetSmtpPassword()),
                EnableSsl = bool.Parse(smtpSettings.GetEnableSsl()),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            var electionId = links.First().ElectionId;

            var tasks = new List<Task>();

            foreach (var link in links)
            {
                var principal = tokenService.GetPrincipalFromToken(link.Token, jwtSettings);
                var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

                if (emailClaim == null || string.IsNullOrEmpty(emailClaim.Value))
                    return new AppError("email address is null", AppErrorTypeEnum.ValidationFailure);

                var email = new Email(electionTitle, inviteText,
                    $"{smtpSettings.GetSmtpBaseUrl()}/election/{electionId}/vote/{link.Id}");
                
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings.GetSmtpSenderEmail(), smtpSettings.GetSmtpSenderName()),
                    Subject = email.Subject,
                    Body = email.Message,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(emailClaim.Value);

                tasks.Add(SendEmailToRecipient(smtpClient, emailClaim.Value, email.Subject, mailMessage.Body));
            }

            await Task.WhenAll(tasks);

            return new AppSuccess("Email sent successfully");
        }
        catch (SmtpException smtpEx)
        {
            return new AppError($"SMTP Error: {smtpEx.Message}", AppErrorTypeEnum.SystemError);
        }
        catch (Exception ex)
        {
            return new AppError($"Error sending email: {ex.Message}", AppErrorTypeEnum.SystemError);
        }
    }
}