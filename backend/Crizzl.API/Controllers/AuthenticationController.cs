using System.Threading.Tasks;
using Crizzl.Domain.DTOs.User;
using Crizzl.Domain.ViewModels;
using Crizzl.Infrastructure.Features.Users.Commands;
using Crizzl.Infrastructure.Features.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crizzl.API.Controllers
{
    public class AuthenticationController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(Login.Query loginQuery) =>
            await Mediator.Send(loginQuery);

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDetailsForListDTO>> Register(Register.Command registerCommand) =>
            await Mediator.Send(registerCommand);
    }
}