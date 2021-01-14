using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Crizzl.Domain.ViewModels;
using Crizzl.Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crizzl.Infrastructure.Features.Photos.Queries
{
    public class Get
    {
        public class Query : IRequest<PhotoDetails>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, PhotoDetails>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IMapper _mapper;

            public Handler(DatabaseContext databaseContext, IMapper mapper)
            {
                _mapper = mapper;
                _databaseContext = databaseContext;
            }

            public async Task<PhotoDetails> Handle(Query query, CancellationToken cancellationToken)
            {
                var photo = await _databaseContext.Photos.FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken: cancellationToken);

                if (photo != null)
                {
                    var photoToReturn = _mapper.Map<PhotoDetails>(photo);
                    return photoToReturn;
                }

                throw new Exception($"Couldn't find photo with id: { query.Id }");
            }
        }
    }
}