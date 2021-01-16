using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Crizzl.Application.Interfaces;
using Crizzl.Domain.Entities;
using Crizzl.Domain.ViewModels;
using Crizzl.Infrastructure.Contexts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Crizzl.Infrastructure.Features.Photos.Commands
{
    public class Upload
    {
        public class Command : IRequest<PhotoDetails>
        {
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command, PhotoDetails>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IUserService _userService;
            private readonly IFileService _fileService;
            private readonly IMapper _mapper;

            public Handler(DatabaseContext databaseContext, IUserService userService, IFileService fileService, IMapper mapper)
            {
                _mapper = mapper;
                _databaseContext = databaseContext;
                _userService = userService;
                _fileService = fileService;
            }

            public async Task<PhotoDetails> Handle(Command command, CancellationToken cancellationToken)
            {
                var fileUpload = _fileService.UploadImage(command.File);
                var user = await _databaseContext.Users.SingleOrDefaultAsync(x => x.Username == _userService.GetCurrentUsername(), cancellationToken: cancellationToken);

                if (user.Username != _userService.GetCurrentUsername())
                    throw new Exception($"User { user.Username } is unauthorized");

                var photo = new Photo
                {
                    PublicId = fileUpload.PublicId,
                    URL = fileUpload.URL,
                };

                if (!user.Photos.Any(x => x.IsMain)) photo.IsMain = true;

                user.Photos.Add(photo);

                var photoSuccessfullyAdded = await _databaseContext.SaveChangesAsync(cancellationToken) > 0;

                if (photoSuccessfullyAdded)
                {
                    var photoToReturn = _mapper.Map<PhotoDetails>(photo);
                    return photoToReturn;
                }

                throw new Exception("Problem uploading photo");
            }
        }
    }
}