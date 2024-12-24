using Arvant.Application.Common.Interfaces;
using Arvant.Domain.Constants;
using Arvant.Domain.Entities;
using Arvant.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Arvant.Infrastructure.Context;

public static class InitializerExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app) {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<ArvantContextInitializer>();
        await initializer.InitialiseAsync();
        await initializer.SeedAsync();
    }
}

public class ArvantContextInitializer(ILogger<ArvantContextInitializer> logger, AppDbContext context, 
    UserManager<ApplicationUser> userManager, IIdentityService identityService) 
{
    public async Task InitialiseAsync() {
        try {
            await context.Database.MigrateAsync();
        } catch (Exception ex) {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }
    
    public async Task SeedAsync() {
        try {
            await AddUser("supervisor", "supervisor@arvant.com", "Supervisor1!", [RoleNames.Admin, RoleNames.Member]);
            await AddUser("b_liusik", "b_liusik@arvant.com", "B_liusik1!", [RoleNames.Member]);
        }
        catch (Exception ex) {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task AddUser(string userName, string email, string password, IEnumerable<string> roles) {
        if (userManager.Users.All(u => u.UserName != userName)) {
            var result = await identityService.CreateUserAsync(userName, password);
            var createdUser = await userManager.FindByNameAsync(userName);
            if (createdUser == null) {
                throw new InvalidOperationException($"Couldn't create {userName} user");
            }
            foreach (var role in roles) {
                await userManager.AddToRoleAsync(createdUser, role);
            }
            await context.InternalUsers.AddAsync(new User {
                Id = result.Data,
                FirstName = userName,
                LastName = userName,
                Name = $"{userName} {userName}",
                Login = userName,
                Email = email
            });
            await context.SaveChangesAsync();
        }
    }
}
