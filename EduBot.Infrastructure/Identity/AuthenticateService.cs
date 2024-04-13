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

        public async Task<string> RegisterUser(User user) {
            var applicationUser = new ApplicationUser {
                UserName = user.Email,
                Email = user.Email,
                Matricula = user.Matricula,
            };

            var result = await _userManager.CreateAsync(applicationUser, user.Password);

            if(!result.Succeeded) {
                foreach (var error in result.Errors) {
                    switch (error.Code) {
                        case "DuplicateUserName":
                            error.Description = "Este email já está sendo utilizado.";
                            break;
                        case "PasswordTooShort":
                            error.Description = "A senha tem que ter pelo menos 6 caracteres.";
                            break;
                        case "PasswordRequiresNonAlphanumeric":
                            error.Description = "A senha tem que ter pelo menos 1 caracter especial.";
                            break;
                        case "PasswordRequiresDigit":
                            error.Description = "A senha tem que ter pelo menos 1 número.";
                            break;
                        case "PasswordRequiresLower":
                            error.Description = "A senha tem que ter pelo menos 1 letra minúscula.";
                            break;
                        case "PasswordRequiresUpper":
                            error.Description = "A senha tem que ter pelo menos 1 letra maiúscula.";
                            break;
                        case "PasswordRequiresUniqueChars":
                            error.Description = "A senha não pode ser vazia.";
                            break;
                    }
                }

                return result.Errors.First().Description;
            }

            if (user.IsAdmin)
            {
                _userManager.AddToRoleAsync(applicationUser, "ADMIN").Wait();
            }
            else
            {
                _userManager.AddToRoleAsync(applicationUser, "USER").Wait();
            }

            await _signInManager.SignInAsync(applicationUser, isPersistent: false);

            return "";
        }

        public async Task Logout() {
            await _signInManager.SignOutAsync();
        }
    }
}
