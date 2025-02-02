using System.Text.Json.Serialization;
using Domain.ElectionAggregate.Election.Enumerations;
using Domain.Shared;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using OneOf;

namespace Domain.ElectionAggregate.VoteLink;

public class VoteLink : BaseEntity
{
    [JsonInclude]
    public string? Token { get; private set; }
    public Guid ElectionId { get; init; }
    public Election.Election Election { get; private set; }

    public OneOf<AppSuccess, AppError> SetToken(string token)
    {
        if(Election.Progress != ElectionProgressEnum.Active)
            return new AppError("Election is not active", AppErrorTypeEnum.BusinessRuleValidationFailure);
        Token = token;
        return new AppSuccess("Election token successfully set");
    }

    public OneOf<AppSuccess, AppError> RemoveToken()
    {
        Token = null;
        UpdatedAt = DateTimeOffset.Now;
        return new AppSuccess("Election token successfully removed");
    }

    public VoteLink() { }

    public VoteLink(string token, Election.Election election)
    {
        Id = Guid.NewGuid();
        Token = token;
        ElectionId = election.Id;
        Election = election;
        CreateAt = DateTimeOffset.Now;
    }
}