
namespace Domain.Shared;

public abstract class BaseEntity
{
    public Guid Id { get; init; }
    public DateTimeOffset CreateAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; set; }
}