namespace VotepucApp.Services.Cache;

public interface ICacheService
{
    Task SetAsync(string key, string value);
    Task<string> GetAsync(string key);
}