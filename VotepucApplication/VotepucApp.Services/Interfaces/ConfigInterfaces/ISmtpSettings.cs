namespace VotepucApp.Services.Interfaces.ConfigInterfaces;

public interface ISmtpSettings
{
    string GetSmtpBaseUrl();
    string GetSmtpServer();
    string GetSmtpPort();
    string GetSmtpSenderName();
    string GetSmtpSenderEmail();
    string GetSmtpUsername();
    string GetSmtpPassword();
    string GetEnableSsl();
}