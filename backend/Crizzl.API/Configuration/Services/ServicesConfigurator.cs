using System.Text;
using AutoMapper;
using Crizzl.API.Configuration.Services;
using Crizzl.Application.Interfaces;
using Crizzl.Application.Settings;
using Crizzl.Infrastructure.Contexts;
using Crizzl.Infrastructure.Features.Users.Commands;
using Crizzl.Infrastructure.Implementations;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API.Configuration.Services
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Crizzl API", Version = "v1" });
            });

            services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Register>());

            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DatabaseConnection")));

            services.AddMediatR(typeof(Register.Handler).Assembly);
            services.AddAutoMapper(typeof(Register.Handler));

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IUserService, UserService>();

            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
        }
    }
}