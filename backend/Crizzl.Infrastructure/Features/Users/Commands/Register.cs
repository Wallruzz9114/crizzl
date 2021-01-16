using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Crizzl.Domain.DTOs.User;
using Crizzl.Domain.Entities;
using Crizzl.Infrastructure.Contexts;
using Crizzl.Infrastructure.Helpers;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crizzl.Infrastructure.Features.Users.Commands
{
    public class Register
    {
        public class Command : IRequest<UserDetailsForListDTO>
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Gender { get; set; }
            public string Alias { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty().Password();
                RuleFor(x => x.Gender).NotEmpty();
                RuleFor(x => x.Alias).NotEmpty();
                RuleFor(x => x.DateOfBirth).NotEmpty();
                RuleFor(x => x.City).NotEmpty();
                RuleFor(x => x.Country).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, UserDetailsForListDTO>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IMapper _mapper;

            public Handler(DatabaseContext databaseContext, IMapper mapper)
            {
                _mapper = mapper;
                _databaseContext = databaseContext;
            }

            public async Task<UserDetailsForListDTO> Handle(Command command, CancellationToken cancellationToken)
            {
                if (await _databaseContext.Users.AnyAsync(x => x.Email == command.Email, CancellationToken.None))
                    throw new Exception($"No Accounts Registered with { command.Email }.");

                if (await _databaseContext.Users.AnyAsync(x => x.Username == command.Username, CancellationToken.None))
                    throw new Exception($"Username { command.Username } already in use.");

                PasswordHasher.GeneratePasswordHash(command.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var newUser = new User
                {
                    Username = command.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Email = command.Email,
                    Gender = command.Gender,
                    DateOfBirth = command.DateOfBirth,
                    Alias = command.Alias,
                    CreatedAt = DateTime.Now,
                    LastActive = DateTime.Now,
                    City = command.City,
                    Country = command.Country
                };

                await _databaseContext.Users.AddAsync(newUser, cancellationToken);
                var creationAttemptSucceeded = await _databaseContext.SaveChangesAsync(cancellationToken) > 0;

                if (creationAttemptSucceeded)
                {
                    var userToReturn = _mapper.Map<UserDetailsForListDTO>(newUser);
                    return userToReturn;
                }

                throw new Exception("Problem while registering new user");
            }
        }
    }
}