using Crizzl.Domain.DTOs.User;

namespace Crizzl.Domain.ViewModels
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public UserDetailsForListDTO User { get; set; }
    }
}