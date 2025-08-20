using DSEB_projeto.Models;
using Microsoft.AspNetCore.Identity;

namespace DSEB_projeto.Services
{
    public class SeedUserRoleInitial : IDisposable
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedUserRoleInitial(UserManager<Usuario> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                role.NormalizedName = "USER";
                await _roleManager.CreateAsync(role);
            }

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                role.NormalizedName = "ADMIN";
                await _roleManager.CreateAsync(role);
            }
        }

        public async Task SeedUsersAsync()
        {
            if (await _userManager.FindByEmailAsync("admin@localhost") == null)
            {
                Usuario user = new Usuario();
                user.UserName = "admin@localhost";
                user.Email = "admin@localhost";
                user.Nome = "Administrador";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;

                IdentityResult result = await _userManager.CreateAsync(user, "Admin@123");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

        public void Dispose()
        {
            // Implementação do método Dispose
        }
    }
}
