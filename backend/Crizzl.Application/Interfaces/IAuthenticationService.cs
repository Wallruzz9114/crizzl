using System.Threading.Tasks;
using Crizzl.Domain.Entities;

namespace Crizzl.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> UserExists(string username);
        bool CorrectPassword(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(User user);
    }
}