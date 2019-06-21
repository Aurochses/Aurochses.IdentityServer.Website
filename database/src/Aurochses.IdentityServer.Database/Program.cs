using Aurochses.Database.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace Aurochses.IdentityServer.Database
{
    public class Program : ProgramBase
    {
        public static void Main(string[] args)
        {
            Main<Startup, Service>(args);
        }

        // ReSharper disable once UnusedMember.Global
        // this method is required by dotnet ef migrations commands
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            CreateWebHostBuilder<Startup>(args);
    }
}