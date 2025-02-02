namespace VotepucApp.Services.Email;

public class Email(string electionTitle, string inviteText, string? link)
{
    public string Subject { get; set; } = $"Participação na eleição: {electionTitle}";
    public string Link { get; set; } = link;
    public string Message { get; set; } = $"Olá!<br/><br/>Você está convidado a participar da eleição: {electionTitle}.<br/>{inviteText}<br/><br/>Para votar, acesse o link a seguir:<br/><a href='{link ?? ""}'>{link ?? ""}</a><br/><br/>O link para votação só poderá ser utilizado uma única vez, faça sua votação de forma consciente e escolha bem seu candidato.<br/><br/>Atenciosamente,<br/>Equipe de Eleições";
    
}