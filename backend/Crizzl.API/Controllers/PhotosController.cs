using System.Threading.Tasks;
using Crizzl.Domain.ViewModels;
using Crizzl.Infrastructure.Features.Photos.Commands;
using Crizzl.Infrastructure.Features.Photos.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crizzl.API.Controllers
{
    public class PhotosController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<PhotoDetails>> Get(int id) => await Mediator.Send(new Get.Query { Id = id });

        [HttpPost("upload")]
        public async Task<ActionResult<PhotoDetails>> Upload([FromForm] Upload.Command uploadCommand) =>
            await Mediator.Send(uploadCommand);

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Unit>> Delete(int id) => await Mediator.Send(new Delete.Command { Id = id });

        [HttpPost("setasmain/{id}")]
        public async Task<ActionResult<Unit>> SetAsMain(int id) => await Mediator.Send(new SetAsMain.Command { Id = id });
    }
}