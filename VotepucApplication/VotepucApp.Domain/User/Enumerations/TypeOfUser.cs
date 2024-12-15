using Domain.Shared;

namespace Domain.User.Enumerations;

public class TypeOfUser(int id, string name) : Enumeration(id, name)
{
    public static readonly TypeOfUser Common = new(1, nameof(Common));
    public static readonly TypeOfUser Admin = new(2, nameof(Admin));
}