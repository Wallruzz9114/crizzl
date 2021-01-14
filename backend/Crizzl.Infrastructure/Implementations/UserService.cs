using System.Linq;
using System.Security.Claims;
using Crizzl.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Crizzl.Infrastructure.Implementations
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public string GetCurrentUsername() =>
            _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}