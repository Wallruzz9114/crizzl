using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Crizzl.Domain.DTOs.User;
using Crizzl.Domain.Entities;
using Crizzl.Domain.ViewModels;
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
            public UserParameters UserParameters { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.UserParameters.Username).NotEmpty();
                RuleFor(x => x.UserParameters.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.UserParameters.Password).Password();
                RuleFor(x => x.UserParameters.Gender).NotEmpty();
                RuleFor(x => x.UserParameters.Alias).NotEmpty();
                RuleFor(x => x.UserParameters.DateOfBirth).NotEmpty();
                RuleFor(x => x.UserParameters.City).NotEmpty();
                RuleFor(x => x.UserParameters.Country).NotEmpty();
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
                if (await _databaseContext.Users.AnyAsync(x => x.Email == command.UserParameters.Email, CancellationToken.None))
                    throw new Exception($"No Accounts Registered with { command.UserParameters.Email }.");

                if (await _databaseContext.Users.AnyAsync(x => x.Username == command.UserParameters.Username, CancellationToken.None))
                    throw new Exception($"Username { command.UserParameters.Username } already in use.");

                PasswordHasher.GeneratePasswordHash(command.UserParameters.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var newUser = new User
                {
                    Username = command.UserParameters.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Email = command.UserParameters.Email,
                    Gender = command.UserParameters.Gender,
                    DateOfBirth = command.UserParameters.DateOfBirth,
                    Alias = command.UserParameters.Alias,
                    CreatedAt = command.UserParameters.CreatedAt,
                    LastActive = command.UserParameters.LastActive,
                    City = command.UserParameters.City,
                    Country = command.UserParameters.Country
                };

                await _databaseContext.Users.AddAsync(newUser, cancellationToken);
                var creationAttemptSucceeded = await _databaseContext.SaveChangesAsync(cancellationToken) > 0;

                if (creationAttemptSucceeded)
                {
                    var userToReturn = _mapper.Map<UserDetailsForListDTO>(newUser);
                    return userToReturn;
                }

                throw new Exception("Problem while registering user");
            }
        }
    }
}