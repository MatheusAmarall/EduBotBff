using EduBot.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EduBot.Infrastructure.Identity {
    public class SeedUserRoleInitial : ISeedUserRoleInitial {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public SeedUserRoleInitial(RoleManager<ApplicationRole> roleManager) {
            _roleManager = roleManager;
        }

        public async Task SeedRoles() {
            if (!_roleManager.RoleExistsAsync("User").Result) {
                await _roleManager.CreateAsync(new ApplicationRole() { Name = "User" });
            }

            if (!_roleManager.RoleExistsAsync("Admin").Result) {
                await _roleManager.CreateAsync(new ApplicationRole() { Name = "Admin" });
            }
        }
    }
}
