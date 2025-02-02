using Domain.Shared.Interfaces;
using Domain.UserAggregate.Permissions;
using Domain.UserAggregate.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using VotepucApp.Persistence.Context.Seeder.Consts;
using VotepucApp.Persistence.Context.Seeder.Permissions;
using VotepucApp.Persistence.Interfaces;

namespace VotepucApp.Persistence.Context.Seeder;

public class UserSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserSeeder> _logger;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IClaimsService _claimsService;

    public UserSeeder(
        RoleManager<IdentityRole> roleManager,
        UserManager<User> userManager,
        IUnitOfWork unitOfWork,
        ILogger<UserSeeder> logger,
        IPermissionRepository permissionRepository,
        IClaimsService claimsService)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _permissionRepository = permissionRepository;
        _claimsService = claimsService;
    }

    public async Task SeedAsync()
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        _logger.LogInformation("Seed started {DateTime}", DateTime.UtcNow);

        var errors = new List<string>();

        try
        {
            var roles = ReflectionHelper.GetConstantsFromClasses(typeof(Roles));

            await CreateRolesAsync(roles);

            var allPermissionsName = ReflectionHelper.GetConstantsFromClasses(typeof(AuthPermissions),
                typeof(ElectionPermissions), typeof(UserPermissions), typeof(TaskPermissions));

            var duplicatePermissions = allPermissionsName
                .GroupBy(p => p)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicatePermissions.Count != 0)
            {
                _logger.LogWarning("Permissões duplicadas encontradas: {Duplicates}",
                    string.Join(", ", duplicatePermissions));
            }

            var admPermissionsName = ReflectionHelper.GetConstantsFromClasses(typeof(UserPermissions),
                typeof(TaskPermissions), typeof(ElectionPermissions));

            var electionManagerPermissionsName = ReflectionHelper.GetConstantsFromClasses(typeof(ElectionPermissions));

            if (!await _permissionRepository.AnyAsync())
            {
                var allPermissions = allPermissionsName.Select(name => new Permission(name, "")).ToList();
                var addPermissionsResult = await _permissionRepository.AddRangePermissionAsync(allPermissions);

                if (addPermissionsResult.IsT1)
                    errors.Add($"Erro ao adicionar permissões: {addPermissionsResult.AsT1.Message}");
            }

            var rolePermissions = new Dictionary<string, IEnumerable<string>>
            {
                { "SuperAdm", allPermissionsName },
                { "Adm", admPermissionsName },
                { "ElectionManager", electionManagerPermissionsName }
            };

            foreach (var (role, permissions) in rolePermissions)
            {
                var permissionObjects = permissions.Select(p => new Permission(p, "")).ToList();
                var addPermissionsResult = await _claimsService.AddPermissionsToRoleAsync(role, permissionObjects);

                if (addPermissionsResult.IsT1)
                    errors.Add($"Erro ao adicionar permissões à role '{role}': {addPermissionsResult.AsT1.Message}");
            }

            var users = new[]
            {
                new
                {
                        UserName = "SuperAdmin", Email = "superadmin@gmail.com", Password = "Superadmin#123",
                    Role = "SuperAdm"
                },
                new { UserName = "Admin", Email = "admin@gmail.com", Password = "Admin#123", Role = "Adm" },
                new
                {
                    UserName = "ElectionManager", Email = "electionmanager@gmail.com", Password = "Electionmanager#123",
                    Role = "ElectionManager"
                },
            };

            foreach (var user in users)
                await CreateUserAsync(user.UserName, user.Email, user.Password, user.Role);

            if (errors.Count != 0)
                LogError("Seed concluído com erros: {Errors}", string.Join("; ", errors));
            else
                _logger.LogInformation("Seed concluído com sucesso.");

            await _unitOfWork.CommitAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            LogError("Erro durante o seed: {Error}", e.Message);
            throw;
        }
    }

    private async Task CreateRolesAsync(List<string> roles)
    {
        foreach (var role in roles)
        {
            if (await _roleManager.RoleExistsAsync(role)) continue;

            var roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
            if (!roleResult.Succeeded)
                LogError("Erro ao criar role '{Role}': {Errors}", role,
                    string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            else
                _logger.LogInformation("Role '{Role}' criada com sucesso.", role);
        }
    }

    private async Task CreateUserAsync(string userName, string email, string password, string role)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            _logger.LogWarning("Usuário com email '{Email}' já existe. Ignorando...", email);
            return;
        }

        var createUser = User.Factory.Create(userName, email);
        if (createUser.IsT1)
        {
            LogError($"Erro ao criar usuário '{userName}': {createUser.AsT1.Message}");
            return;
        }

        var user = createUser.AsT0;
        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            LogError("Erro ao salvar usuário '{UserName}': {Errors}", userName,
                string.Join(", ", createResult.Errors.Select(e => e.Description)));
            return;
        }

        var roleResult = await _userManager.AddToRoleAsync(user, role);
        if (!roleResult.Succeeded)
        {
            LogError("Erro ao atribuir role '{Role}' ao usuário '{UserName}': {Errors}", role, userName,
                string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            return;
        }

        _logger.LogInformation("Usuário '{UserName}' criado e atribuído à role '{Role}'.", userName, role);
    }

    private void LogError(string message, params object[] args)
    {
        _logger.LogError(message, args);
    }
}