using EduBot.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EduBot.Infrastructure.Identity {
    public class SeedUserRoleInitial : ISeedUserRoleInitial {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public SeedUserRoleInitial(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void SeedUsers() {
            if (_userManager.FindByEmailAsync("usuario@localhost.com").Result == null) {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "usuario@localhost.com";
                user.Email = "usuario@localhost.com";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = _userManager.CreateAsync(user, "@Teste123").Result;

                if (result.Succeeded) {
                    _userManager.AddToRoleAsync(user, "User").Wait();
                }
            }

            if (_userManager.FindByEmailAsync("admin@localhost.com").Result == null) {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "admin@localhost.com";
                user.Email = "admin@localhost.com";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = _userManager.CreateAsync(user, "@Teste123").Result;

                if (result.Succeeded) {
                    _userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }

        public void SeedRoles() {
            if (!_roleManager.RoleExistsAsync("User").Result) {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }

            if (!_roleManager.RoleExistsAsync("Admin").Result) {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
        }
    }
}
