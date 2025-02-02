using System.Reflection;

namespace VotepucApp.Persistence.Context.Seeder;

public static class ReflectionHelper
{
    public static List<string?> GetConstantsFromClasses(params Type[] types)
    {
        var constants = new List<string?>();

        foreach (var type in types)
        {
            constants.AddRange(
                type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(field => field.IsLiteral && !field.IsInitOnly)
                    .Select(field => field.GetRawConstantValue()?.ToString())
                    .Where(value => value != null)
            );
        }

        return constants;
    }
}
