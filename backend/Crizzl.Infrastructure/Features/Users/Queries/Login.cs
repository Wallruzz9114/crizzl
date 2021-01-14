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
            public LoginParameters LoginParameters { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.LoginParameters.Username).NotEmpty();
                RuleFor(x => x.LoginParameters.Password).NotEmpty();
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
                var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Username == query.LoginParameters.Username, cancellationToken: cancellationToken);

                if (user == null) throw new Exception($"Could not find user: { user.Username }");

                var authenticationResponse = new AuthenticationResponse
                {
                    Token = _authenticationService.CreateToken(user),
                    User = _mapper.Map<UserDetailsForListDTO>(user)
                };

                return authenticationResponse;
            }
        }
    }
}