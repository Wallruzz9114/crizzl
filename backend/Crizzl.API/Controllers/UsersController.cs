using System.Collections.Generic;
using System.Threading.Tasks;
using Crizzl.Domain.DTOs.User;
using Crizzl.Domain.ViewModels;
using Crizzl.Infrastructure.Features.Users.Commands;
using Crizzl.Infrastructure.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crizzl.API.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(Login.Query loginQuery) =>
            await Mediator.Send(loginQuery);

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDetailsForListDTO>> Register(Register.Command registerCommand) =>
            await Mediator.Send(registerCommand);

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailsDTO>> Get(int id) => await Mediator.Send(new Get.Query { Id = id });

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailsForListDTO>>> GetAll() =>
            Ok(await Mediator.Send(new GetAll.Query { }));

        [HttpPut("update")]
        public async Task<ActionResult<Unit>> UpdateBio(Update.Command updateCommand) =>
            await Mediator.Send(updateCommand);
    }
}