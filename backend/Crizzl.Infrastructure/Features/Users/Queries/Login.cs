using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Crizzl.Application.Interfaces;
using Crizzl.Domain.DTOs.User;
using Crizzl.Domain.ViewModels;
using Crizzl.Infrastructure.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crizzl.Infrastructure.Features.Users.Queries
{
    public class Login
    {
        public class Query : IRequest<AuthenticationResponse>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, AuthenticationResponse>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IAuthenticationService _authenticationService;
            private readonly IMapper _mapper;

            public Handler(DatabaseContext databaseContext, IAuthenticationService authenticationService, IMapper mapper)
            {
                _mapper = mapper;
                _authenticationService = authenticationService;
                _databaseContext = databaseContext;
            }

            public async Task<AuthenticationResponse> Handle(Query query, CancellationToken cancellationToken)
            {
                var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Username == query.Username.ToLower(), cancellationToken: cancellationToken);

                if (user == null) throw new Exception($"Could not find user: { user.Username }");

                var correctPassword = _authenticationService.CorrectPassword(query.Password, user.PasswordHash, user.PasswordSalt);

                if (!correctPassword) throw new Exception($"Password for user { user.Username } is incorrect");

                var authenticationResponse = new AuthenticationResponse
                {
                    Token = _authenticationService.CreateToken(user),
                    User = _mapper.Map<UserDetailsDTO>(user)
                };

                return authenticationResponse;
            }
        }
    }
}