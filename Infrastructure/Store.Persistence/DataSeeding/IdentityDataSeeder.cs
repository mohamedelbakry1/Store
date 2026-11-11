using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Domain.Contracts;
using Store.Persistence.Identity.Data.Contexts;
using Store.Persistence.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.DataSeeding
{
    internal class IdentityDataSeeder(IdentityStoreDbContext _dbContext,
                                      UserManager<AppUser> _userManager,
                                      RoleManager<IdentityRole> _roleManager,
                                      ILogger<IdentityDataSeeder> _logger) : IDataSeeder
    {
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(ct);
                if (pendingMigrations.Any())
                    await _dbContext.Database.MigrateAsync(ct);

                if (!await _roleManager.Roles.AnyAsync(ct))
                {
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                if (!await _userManager.Users.AnyAsync(ct))
                {
                    var superAdmin = new AppUser()
                    {
                        DisplayName = "Mohamed Elbakry",
                        Email = "MohamedElbakry333@gmail.com",
                        UserName = "MohamedElbakry",
                        PhoneNumber = "01552013062"
                    };

                    var admin = new AppUser()
                    {
                        DisplayName = "Osama Mohamed",
                        Email = "OsamaMohamed555@gmail.com",
                        UserName = "OsamaMohamed",
                        PhoneNumber = "01064897412"
                    };

                    var createSuperAdmin = await _userManager.CreateAsync(superAdmin, "P@ssw0rd");
                    var createAdmin = await _userManager.CreateAsync(admin, "P@ssw0rd");
                    if (createSuperAdmin.Succeeded)
                        await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                    else
                    {
                        var Errors = string.Join(", ", createSuperAdmin.Errors.Select(e => e.Description));
                        _logger.LogWarning($"Can not create super admin user. Errors: {Errors}");
                    }

                    if (createAdmin.Succeeded)
                        await _userManager.AddToRoleAsync(admin, "Admin");
                    else
                    {
                        var Errors = string.Join(", ", createAdmin.Errors.Select(e => e.Description));
                        _logger.LogWarning($"Can not create admin user. Errors: {Errors}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Identity Data Seed Failed!");
            }
        }
    }
}
