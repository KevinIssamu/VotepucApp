using Microsoft.Extensions.Configuration;

namespace VotepucApp.Application.Cases.Shared;

public sealed class Link
{
    private readonly string _rel;
    private readonly string _ref;
    private readonly string _method;
    public Link(string rel, string path, string method, IConfiguration configuration)
    {
        _rel = rel;
        var baseUrl = configuration["ApiSettings:BaseUrl"] 
                      ?? throw new NullReferenceException("ApiSettings:BaseUrl cannot be null.");
        
        _ref = $"{baseUrl}/api/{path}";
        _method = method;
    }
}