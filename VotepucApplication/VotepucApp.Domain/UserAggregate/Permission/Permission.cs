namespace Domain.UserAggregate.Permissions;

public class Permission(string name, string description)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
}