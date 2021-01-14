using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crizzl.Application.Interfaces;
using Crizzl.Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crizzl.Infrastructure.Features.Photos.Commands
{
    public class SetAsMain
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IUserService _userService;

            public Handler(DatabaseContext databaseContext, IUserService userService)
            {
                _databaseContext = databaseContext;
                _userService = userService;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var user = await _databaseContext.Users.SingleOrDefaultAsync(x => x.Username == _userService.GetCurrentUsername(), cancellationToken: cancellationToken);
                var photo = user.Photos.FirstOrDefault(x => x.Id == command.Id);

                if (photo == null) throw new Exception($"Couldn't find photo with id: { command.Id }");

                var currentMainPhoto = user.Photos.FirstOrDefault(x => x.IsMain);

                currentMainPhoto.IsMain = false;
                photo.IsMain = true;

                var updateIsSuccessful = await _databaseContext.SaveChangesAsync(cancellationToken) > 0;

                if (updateIsSuccessful) return Unit.Value;

                throw new Exception("Problem setting main photo");
            }
        }
    }
}