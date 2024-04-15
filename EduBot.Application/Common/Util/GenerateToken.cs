using EduBot.Application.Common.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EduBot.Application.Common.Util {
    public class GenerateToken {
        public static UserTokenDto GenerateUserToken(UserInfoResultDto userInfo, IConfiguration configuration) {
            var claims = new[]
            {
                new Claim("email", userInfo.Email ?? ""),
                new Claim("permissao", userInfo.Role ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var privateKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"] ?? ""));

            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(60);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
                );

            return new UserTokenDto() {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Email = userInfo.Email ?? "",
                Role = userInfo.Role ?? ""
            };
        }
    }
}
