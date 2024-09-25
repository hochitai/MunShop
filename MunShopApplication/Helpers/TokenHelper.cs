using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using System.Configuration;
using MunShopApplication.Configs;
using Microsoft.AspNetCore.Mvc;
using MunShopApplication.Entities;
using MunShopApplication.DTOs;

namespace DirectoryPermissionManagement.Helpers
{
    public class TokenHelper
    {
        public static string GenerateToken(User user, JWTConfig config)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                new Claim("id", user.Id.ToString())
            };
            var token = new JwtSecurityToken(config.Issuer,
                config.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static int GetUserIdFromToken(ClaimsIdentity identity)
        {
            if (identity == null)
            {
                return 0;
            }

            var userClaims = identity.Claims;
            var userId = Int32.Parse(userClaims.FirstOrDefault(x => x.Type == "id")?.Value);
            return userId;
        }

    }
}
