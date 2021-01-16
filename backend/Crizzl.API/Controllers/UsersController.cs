using System.Collections.Generic;
using System.Threading.Tasks;
using Crizzl.Domain.DTOs.User;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailsDTO>> Get(int id) => await Mediator.Send(new Get.Query { Id = id });

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailsForListDTO>>> GetAll() =>
            Ok(await Mediator.Send(new GetAll.Query { }));

        [HttpPut("update")]
        public async Task<ActionResult<Unit>> UpdateBio(Update.Command updateCommand) =>
            await Mediator.Send(updateCommand);
    }
}