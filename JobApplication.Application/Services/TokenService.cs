using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobApplication.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateTokenAsync(
            ApplicationUser user,
            UserManager<ApplicationUser> userManager)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!)
            };
            if (user.CandidateId.HasValue)
            {
                authClaims.Add(
                    new Claim("CandidateId", user.CandidateId.Value.ToString()));
            }
            // Add RecruiterId claim if the user is a recruiter
            if (user.RecruiterId.HasValue)
            {
                authClaims.Add(
                    new Claim("RecruiterId", user.RecruiterId.Value.ToString()));
            }
            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddDays(
                    double.Parse(_configuration["JWT:DurationInDays"]!)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    authKey,
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}