using System;
using System.Threading.Tasks;
using Crizzl.Application.Interfaces;
using Crizzl.Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Crizzl.Infrastructure.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        private readonly IUserService _userService;
        private readonly DatabaseContext _databaseContext;

        public LogUserActivity(IUserService userService, DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var username = _userService.GetCurrentUsername();
            var user = await _databaseContext.Users.SingleOrDefaultAsync(x => x.Username == username);

            user.LastActive = DateTime.Now;

            await _databaseContext.SaveChangesAsync();
        }
    }
}