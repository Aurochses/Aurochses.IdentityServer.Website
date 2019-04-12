using Aurochses.Database.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Aurochses.IdentityServer.Database
{
    public class Program : ProgramBase
    {
        public static void Main(string[] args)
        {
            Main<Startup, Service>(args);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            CreateWebHostBuilder<Startup>(args)
                .ConfigureLogging(
                    (context, builder) =>
                    {
                        builder.AddConfiguration(context.Configuration.GetSection("Logging"))
                            .AddConsole()
                            .AddDebug();
                    }
                );
    }
}