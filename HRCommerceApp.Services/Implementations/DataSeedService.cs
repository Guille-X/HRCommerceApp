using HRCommerceApp.Core.Enums;
using HRCommerceApp.Core.Models.Entities;
using HRCommerceApp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCommerceApp.Services.Implementations
{
    public class DataSeedService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeedService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDataAsync()
        {
            await SeedRolesAsync();
            await SeedAdminUserAsync();
            await SeedDepartamentosAsync();
        }

        private async Task SeedRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync(UserRole.Administrador))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.Administrador));

            if (!await _roleManager.RoleExistsAsync(UserRole.Operador))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.Operador));
        }

        private async Task SeedAdminUserAsync()
        {
            var adminEmail = "admin@hrcommerce.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "Sistema",
                    CreatedAt = DateTime.UtcNow,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "Admin123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRole.Administrador);
                }
            }

            var operatorEmail = "operador@hrcommerce.com";
            var operatorUser = await _userManager.FindByEmailAsync(operatorEmail);

            if (operatorUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = operatorEmail,
                    Email = operatorEmail,
                    FirstName = "Operador",
                    LastName = "Sistema",
                    CreatedAt = DateTime.UtcNow,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "Operador123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRole.Operador);
                }
            }
        }

        private async Task SeedDepartamentosAsync()
        {
            if (!await _context.Departamentos.AnyAsync())
            {
                var departamentos = new List<Departamento>
                {
                    new Departamento { Nombre = "Recursos Humanos", Presupuesto = 50000.00m, CreatedBy = "system" },
                    new Departamento { Nombre = "Ventas", Presupuesto = 75000.00m, CreatedBy = "system" },
                    new Departamento { Nombre = "Tecnología", Presupuesto = 60000.00m, CreatedBy = "system" }
                };

                await _context.Departamentos.AddRangeAsync(departamentos);
                await _context.SaveChangesAsync();
            }
        }
    }
}
