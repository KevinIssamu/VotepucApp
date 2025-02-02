using VotepucApp.Services.Interfaces.ConfigInterfaces;

namespace VotepucApp.WebAPI.Settings;

public class SmtpSettings(IConfiguration configuration) : ISmtpSettings
{
    public string GetSmtpBaseUrl() 
        => configuration["SmtpSettings:BaseUrl"] ?? throw new ArgumentException("The 'SmtpSettings:BaseUrl' configuration is missing or empty.");

    public string GetSmtpServer() 
        => configuration["SmtpSettings:Server"] ?? throw new ArgumentException("The 'SmtpSettings:Server' configuration is missing or empty.");

    public string GetSmtpPort() 
        => configuration["SmtpSettings:Port"] ?? throw new ArgumentException("The 'SmtpSettings:Port' configuration is missing or empty.");

    public string GetSmtpSenderName() 
        => configuration["SmtpSettings:SenderName"] ?? throw new ArgumentException("The 'SmtpSettings:SenderName' configuration is missing or empty.");

    public string GetSmtpSenderEmail() 
        => configuration["SmtpSettings:SenderEmail"] ?? throw new ArgumentException("The 'SmtpSettings:SenderEmail' configuration is missing or empty.");

    public string GetSmtpUsername() 
        => configuration["SmtpSettings:Username"] ?? throw new ArgumentException("The 'SmtpSettings:Username' configuration is missing or empty.");

    public string GetSmtpPassword() 
        => configuration["SmtpSettings:Password"] ?? throw new ArgumentException("The 'SmtpSettings:Password' configuration is missing or empty.");

    public string GetEnableSsl() 
        => configuration["SmtpSettings:EnableSsl"] ?? throw new ArgumentException("The 'SmtpSettings:EnableSsl' configuration is missing or empty.");
}
