using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Crizzl.Application.Helpers;
using Crizzl.Application.Interfaces;
using Crizzl.Application.Settings;
using Crizzl.Domain.DTOs.User;
using Crizzl.Domain.Entities;
using Crizzl.Infrastructure.Contexts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Crizzl.Infrastructure.Features.Users.Queries
{
    public class GetAll
    {
        public class Query : IRequest<IEnumerable<UserDetailsForListDTO>>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int MinAge { get; set; }
            public int MaxAge { get; set; }
            public string OrderBy { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<UserDetailsForListDTO>>
        {
            private readonly IMapper _mapper;
            private readonly DatabaseContext _databaseContext;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUserService _userService;

            public Handler(DatabaseContext databaseContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserService userService)
            {
                _userService = userService;
                _mapper = mapper;
                _databaseContext = databaseContext;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<IEnumerable<UserDetailsForListDTO>> Handle(Query query, CancellationToken cancellationToken)
            {
                var usersFromDB = _databaseContext.Users
                    .Include(p => p.Photos)
                    .OrderByDescending(u => u.LastActive)
                    .AsQueryable();
                var currentUser = await _databaseContext.Users
                    .SingleOrDefaultAsync(u => u.Username == _userService.GetCurrentUsername(), cancellationToken: cancellationToken);
                var genderParam = currentUser.Gender == "male" ? "female" : "male";

                usersFromDB = usersFromDB.Where(u => u.Id != currentUser.Id);
                usersFromDB = usersFromDB.Where(u => u.Gender == genderParam);

                if (query.MinAge != 18 || query.MaxAge != 99)
                {
                    var minDOB = DateTime.Today.AddYears(-query.MaxAge - 1);
                    var maxDOB = DateTime.Today.AddYears(-query.MinAge);
                    usersFromDB = usersFromDB.Where(u => u.DateOfBirth >= minDOB && u.DateOfBirth <= maxDOB);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    usersFromDB = query.OrderBy switch
                    {
                        "created" => usersFromDB.OrderByDescending(u => u.CreatedAt),
                        _ => usersFromDB.OrderByDescending(u => u.LastActive),
                    };
                }

                var paginatedUsers = await PagedList<User>.CreateAsync(usersFromDB, query.PageNumber, query.PageSize);
                var paginatedUsersToReturn = _mapper.Map<IEnumerable<UserDetailsForListDTO>>(paginatedUsers);

                _httpContextAccessor.HttpContext.Response.AddPagination(
                    paginatedUsers.PageNumber,
                    paginatedUsers.ItemsPerPage,
                    paginatedUsers.TotalItems,
                    paginatedUsers.TotalPages
                );

                return paginatedUsersToReturn;
            }
        }
    }
}