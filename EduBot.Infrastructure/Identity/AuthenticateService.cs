using EduBot.Application.Common.Interfaces;
using EduBot.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EduBot.Infrastructure.Identity {
    public class AuthenticateService : IAuthenticate {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public AuthenticateService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<bool> Authenticate(string email, string password) {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task<bool> RegisterUser(User user) {
            var applicationUser = new ApplicationUser {
                UserName = user.Email,
                Email = user.Email,
                Matricula = user.Matricula,
            };



            var result = await _userManager.CreateAsync(applicationUser, user.Password);

            if (result.Succeeded) {
                if (user.IsAdmin)
                {
                    _userManager.AddToRoleAsync(applicationUser, "ADMIN").Wait();
                }
                else
                {
                    _userManager.AddToRoleAsync(applicationUser, "USER").Wait();
                }

                await _signInManager.SignInAsync(applicationUser, isPersistent: false);
            }

            return result.Succeeded;
        }

        public async Task Logout() {
            await _signInManager.SignOutAsync();
        }
    }
}
