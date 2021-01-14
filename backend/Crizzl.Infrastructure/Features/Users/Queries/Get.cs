using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Crizzl.Domain.DTOs.User;
using Crizzl.Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crizzl.Infrastructure.Features.Users.Queries
{
    public class Get
    {
        public class Query : IRequest<UserDetailsDTO>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, UserDetailsDTO>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IMapper _mapper;

            public Handler(DatabaseContext databaseContext, IMapper mapper)
            {
                _mapper = mapper;
                _databaseContext = databaseContext;
            }

            public async Task<UserDetailsDTO> Handle(Query query, CancellationToken cancellationToken)
            {
                var user = await _databaseContext.Users
                    .Include(p => p.Photos)
                    .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken: cancellationToken);

                var userToReturn = _mapper.Map<UserDetailsDTO>(user);

                return userToReturn;
            }
        }
    }
}