using EduBot.Application.Interactors.Login;
using EduBot.Application.Interactors.Register;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using EduBot.Application.Interactors.Logout;
using EduBot.Api.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EduBot.Api.Controllers {
    public class AuthController : ApiController {
        private readonly IConfiguration _configuration;
        public AuthController(IMediator mediator, IConfiguration configuration)
            : base(mediator)
        {
            _configuration = configuration;
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login([FromBody] LoginCommand userInfo) {
            ErrorOr<Unit> result = await CommandAsync(userInfo)
                .ConfigureAwait(false);

            if(result.IsError) {
                return Problem(result.Errors);
            }

            UserToken userToken = GenerateToken(userInfo);

            return Ok(userToken);
        }

        [HttpPost("CreatedUser")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterCommand userInfo) {
            ErrorOr<Unit> result = await CommandAsync(userInfo)
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Created($"Usuário {userInfo.Email} criado com sucesso");
        }

        [HttpPost("LogoutUser")]
        public async Task<IActionResult> LogoutUser([FromBody] LogoutCommand userInfo)
        {
            ErrorOr<Unit> result = await CommandAsync(userInfo)
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Ok($"Usuário {userInfo.Email} deslogado com sucesso");
        }

        private UserToken GenerateToken(LoginCommand userInfo) {
            try {
                var claims = new[]
                {
                    new Claim("email", userInfo.Email),
                    new Claim("umValor", "OUTROVALOR"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var privateKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? ""));

                var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

                var expiration = DateTime.UtcNow.AddMinutes(60);

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: expiration,
                    signingCredentials: credentials
                    );

                return new UserToken() {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = expiration
                };
            }
            catch(Exception ex) {
                throw;
            }
            
        }
    }
}
