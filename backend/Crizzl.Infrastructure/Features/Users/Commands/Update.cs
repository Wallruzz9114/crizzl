using System;
using System.Threading;
using System.Threading.Tasks;
using Crizzl.Application.Interfaces;
using Crizzl.Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crizzl.Infrastructure.Features.Users.Commands
{
    public class Update
    {
        public class Command : IRequest
        {
            public string Bio { get; set; }
            public string DatingTarget { get; set; }
            public string Interests { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
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

                if (user.Username != _userService.GetCurrentUsername())
                    throw new Exception($"User { user.Username } is unauthorized to update profile");

                user.Bio = command.Bio ?? user.Bio;
                user.DatingTarget = command.DatingTarget ?? user.Bio;
                user.Interests = command.Interests ?? user.Bio;
                user.City = command.City ?? user.Bio;
                user.Country = command.Country ?? user.Bio;

                var updateIsSuccessful = await _databaseContext.SaveChangesAsync(cancellationToken) > 0;

                if (updateIsSuccessful) return Unit.Value;

                throw new Exception($"Problem while updating user with id: { user.Id }");
            }
        }
    }
}