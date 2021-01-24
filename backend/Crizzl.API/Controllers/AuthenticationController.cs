using System.Threading.Tasks;
using Crizzl.Domain.DTOs.User;
using Crizzl.Domain.ViewModels;
using Crizzl.Infrastructure.Features.Users.Commands;
using Crizzl.Infrastructure.Features.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Crizzl.API.Controllers
{
    public class AuthenticationController : BaseController
    {

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(Login.Query loginQuery) =>
            await Mediator.Send(loginQuery);

        [HttpPost("register")]
        public async Task<ActionResult<UserDetailsForListDTO>> Register(Register.Command registerCommand) =>
            await Mediator.Send(registerCommand);
    }
}