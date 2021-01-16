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
    public class Delete
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IUserService _userService;
            private readonly IFileService _fileService;

            public Handler(DatabaseContext databaseContext, IUserService userService, IFileService fileService)
            {
                _databaseContext = databaseContext;
                _userService = userService;
                _fileService = fileService;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var user = await _databaseContext.Users.SingleOrDefaultAsync(x => x.Username == _userService.GetCurrentUsername(), cancellationToken: cancellationToken);

                if (user.Username != _userService.GetCurrentUsername())
                    throw new Exception($"User { user.Username } is unauthorized");

                var photo = user.Photos.FirstOrDefault(x => x.Id == command.Id);

                if (photo == null) throw new Exception($"Couldn't find photo with id: { command.Id }");

                if (photo.IsMain)
                    throw new Exception($"You can't delete your main photo");

                var deleteAttemptResult = _fileService.DeleteFile(photo.PublicId);

                if (deleteAttemptResult == null) throw new Exception("Error: couldn't delete image");

                user.Photos.Remove(photo);

                var userSuccessfullyUpdated = await _databaseContext.SaveChangesAsync(cancellationToken) > 0;

                if (userSuccessfullyUpdated) return Unit.Value;

                throw new Exception("Problem while deleting image");
            }
        }
    }
}