using System;
using System.Diagnostics;
using System.IO;
using Crizzl.Infrastructure.Contexts;
using Crizzl.Infrastructure.Helpers;
using Crizzl.Infrastructure.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Crizzl.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
                var host = CreateHostBuilder(args).Build();
                using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;
                var databaseContext = services.GetRequiredService<DatabaseContext>();

                databaseContext.Database.Migrate();
                DataSeeder.Seed(databaseContext);
                host.Run();
            }
            catch (Exception exception)
            {
                Log.Warning(exception, "An error occured while seeding the DB");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseSerilog((hostingContext, loggingConfiguration) => loggingConfiguration
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Application", "crizzl")
                    .Enrich.WithProperty("MachineName", Environment.MachineName)
                    .Enrich.WithProperty("CurrentManagedThreadId", Environment.CurrentManagedThreadId)
                    .Enrich.WithProperty("OSVersion", Environment.OSVersion)
                    .Enrich.WithProperty("Version", Environment.Version)
                    .Enrich.WithProperty("UserName", Environment.UserName)
                    .Enrich.WithProperty("ProcessId", Environment.ProcessId)
                    .Enrich.WithProperty("ProcessName", Process.GetCurrentProcess().ProcessName)
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                    .WriteTo.File(
                        formatter: new LogTextFormatter(),
                        path: Path.Combine(
                            hostingContext.HostingEnvironment.ContentRootPath +
                            $"{Path.DirectorySeparatorChar}Logs{Path.DirectorySeparatorChar}",
                            $"crizzl_log_{DateTime.Now:yyyyMMdd}.txt"
                        )
                    ).ReadFrom.Configuration(hostingContext.Configuration)
                );

                webBuilder.UseStartup<Startup>();
            });
    }
}
