using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crizzl.Domain.DTOs.User;
using Crizzl.Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crizzl.Infrastructure.Features.Users.Queries
{
    public class GetAll
    {
        public class Query : IRequest<IEnumerable<UserDetailsForListDTO>> { }

        public class Handler : IRequestHandler<Query, IEnumerable<UserDetailsForListDTO>>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IMapper _mapper;

            public Handler(DatabaseContext databaseContext, IMapper mapper)
            {
                _mapper = mapper;
                _databaseContext = databaseContext;
            }

            public async Task<IEnumerable<UserDetailsForListDTO>> Handle(Query query, CancellationToken cancellationToken)
            {
                var users = await _databaseContext.Users
                    .Include(p => p.Photos)
                    .ProjectTo<UserDetailsForListDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken: cancellationToken);

                return users;
            }
        }
    }
}