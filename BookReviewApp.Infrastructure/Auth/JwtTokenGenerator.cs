using BookReviewApp.Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.Infrastructure.Auth
{
    public class JwtTokenGenerator
    {
        public string GenerateToken(AppUser user, IList<string> roles)
        {
            var key = Environment.GetEnvironmentVariable("BookAPP_JWT_KEY");
            var issuer = Environment.GetEnvironmentVariable("BookAPP_JWT_ISSUER");
            var audience = Environment.GetEnvironmentVariable("BookAPP_JWT_AUDIENCE");

            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("BookAPP_JWT_KEY environment variable is missing.");

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
