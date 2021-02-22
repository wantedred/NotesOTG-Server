using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotesOTG_Server.Models;
using NotesOTG_Server.Models.Contexts;
using NotesOTG_Server.Services.Interfaces;

namespace NotesOTG_Server.Services
{
    public class TokenService: Service<RefreshToken>, ITokenService
    {
        public TokenService(DatabaseContext context) : base(context)
        { }

        public async Task<RefreshToken> FindByToken(string refreshToken)
        {
            return await entity.SingleOrDefaultAsync(e => e.Token == refreshToken);
        }

        public bool UpdateToken(RefreshToken refreshToken)
        {
            var updateToken = entity.Update(refreshToken);
            return updateToken != null;
        }

        public bool RemoveToken(RefreshToken refreshToken)
        {
            var tokenRemoval = entity.Remove(refreshToken);
            return tokenRemoval != null;
        }

        public async Task<bool> SaveChanges()
        {
            var save = await base.Save();
            return save;
        }

        public string GeneratePrimaryToken(string userId)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userId)
            };
            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "http://localhost:5000",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }

        public async Task<string> IssueStandardRefresh(string token, string email)
        {
            var refreshToken = await FindByToken(token);
            if (refreshToken == null || !refreshToken.IsValid() || refreshToken.Email != email)
            {
                if (refreshToken != null)
                {
                    RemoveToken(refreshToken);
                    await SaveChanges();
                }
                return null;
            }
            
            refreshToken.Token = Guid.NewGuid().ToString();
            refreshToken.Provided = DateTime.Now;
            UpdateToken(refreshToken);
            await SaveChanges();
            return refreshToken.Token;
        }

        public async Task<string> IssueEmailRefresh(string email)
        {
            var refreshToken = new RefreshToken
            {
                Email = email,
                Token = Guid.NewGuid().ToString()
            };
            var refreshSearch = await entity.FirstOrDefaultAsync(m => m.Email == email);
            if (refreshSearch != null)
            {
                refreshSearch.Token = refreshToken.Token;
                entity.Update(refreshSearch);
            }
            else
            {
                await entity.AddAsync(refreshToken);
            }
            await SaveChanges();
            return refreshToken.Token;
        }
    }
}