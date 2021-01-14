using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Crizzl.Application.Interfaces;
using Crizzl.Domain.Entities;
using Crizzl.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Crizzl.Infrastructure.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly SymmetricSecurityKey _key;

        public AuthenticationService(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
        }

        public async Task<bool> UserExists(string username) =>
            await _databaseContext.Users.AnyAsync(x => x.Username == username);

        public bool CorrectPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < hash.Length; i++)
                if (hash[i] != passwordHash[i]) return false;

            return true;
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.NameId, user.Username) };
            var symmetricSecurityKey = _key;
            var signInCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signInCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }
    }
}